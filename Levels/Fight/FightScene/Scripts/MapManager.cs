#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

public partial class MapManager : Node2D
{
	private List<List<Vector2I>> _cartesianCoords = [];
	private List<List<Vector2I>> _isometricCoords = [];
	private readonly List<Tile> _mapTiles = [];
	public List<Tile> MapTiles
    {
		get
		{
			return _mapTiles;
		}
    }

	private TileMapLayer _floorLayer;
	private TileMapLayer _entityLayer;
	private TileMapLayer _obstacleLayer;
	/// <summary>
	/// Слой отвечающий за отрисовку анимаций уничтожения сущности с слоя "Сущностей"
	/// </summary>
	private TileMapLayer _removeEntitiesEffectLayer;
	
	/// <summary>
	/// Ассоциативный массив для получения изометрической координаты по декартовой
	/// </summary>
	private readonly Dictionary<Vector2I, Tile> _tilesByCartesian = new();
	
	/// <summary>
	/// Ассоциативный массив для получения декартовой координаты по изометрической
	/// </summary>
	private readonly Dictionary<Vector2I, Tile> _tilesByIsometric = new();


	[Signal]
	public delegate void OnTileLeftButtonClickedEventHandler(Vector2I tile);
	
	[Signal]
	public delegate void OnTileRightButtonClickedEventHandler(Fight.BattleEntity battleEntity);


	/// <summary>
	/// Тайлики которые на данный момент выбраны для отображения
	/// </summary>
	private List<Tile> _selectedTilesForPlayerAction { get; set; } = [];

	public IReadOnlyCollection<Tile> SelectedTilesForPlayerAction => _selectedTilesForPlayerAction.ToList(); 
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && @event.IsPressed())
		{
			if (!mouseButton.Pressed) return;
			var tile = GetTileUnderMousePosition();
			if (mouseButton.ButtonIndex == MouseButton.Left)
			{
				if (tile != null && tile.BattleEntity == null) EmitSignalOnTileLeftButtonClicked(tile.CartesianPosition);
			}

		}
	}

	public override void _Ready()
	{
		_floorLayer = GetNode<TileMapLayer>("Floor");
		_entityLayer = GetNode<TileMapLayer>("Entities");
		_obstacleLayer = GetNode<TileMapLayer>("Obstacles");
		_removeEntitiesEffectLayer = GetNode<TileMapLayer>("RemoveEntitiesEffect");
		foreach (var instanceTile in BattleMapInitStateManager.Instance.Tiles)
		{
			_mapTiles.Add(instanceTile);
			_tilesByCartesian.Add(instanceTile.CartesianPosition, instanceTile);
			_tilesByIsometric.Add(instanceTile.IsometricPosition, instanceTile);
			DeselectTile(instanceTile);
			if (instanceTile.BattleEntity is not null || instanceTile.ObstacleEntity is not null) InitBattleEntityOnTile(instanceTile);
		}
	}
	
	public override void _Process(double delta)
	{
	}
	
	
	public Tile? GetTileByCartesianCoord(Vector2I cartesianCoord)
	{
		_tilesByCartesian.TryGetValue(cartesianCoord, out var tile);
		return tile;
	}

	public Tile? GetTileByIsometricCoord(Vector2I isometricCoord)
	{
		_tilesByIsometric.TryGetValue(isometricCoord, out var tile);
		return tile;
	}

	private void InitBattleEntityOnTile(Tile tile)
	{
		if (tile.BattleEntity is null && tile.ObstacleEntity is null) return;
		if (tile.BattleEntity is PlayerEntity playerEntity)
		{
			_entityLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(0, 0));
		}
		else if (tile.BattleEntity is EnemyEntity enemyEntity)
		{
			_entityLayer.SetCell(tile.IsometricPosition, 1, new Vector2I(0, (int)enemyEntity.EnemyId - 1));
		}

		if (tile.ObstacleEntity is not null)
		{
			_obstacleLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(0, (int)tile.ObstacleEntity.ObstacleCode));
		}
	}

	/// <summary>
	/// Перемещает сущность на тайл
	/// Если сущность стоит на тайле, то "стирает" её с предыдущего тайла
	/// </summary>
	/// <param name="targetTile">Тайл на который будет совершено перемещение</param>
	/// <param name="battleEntity">Сущность которая будет перемещаться</param>
	/// <returns></returns>
	public bool SetBattleEntityOnTile(Tile targetTile, BattleEntity battleEntity)
	{
		if (targetTile.BattleEntity is not null) return false;
		RemoveBattleEntityFromTile(battleEntity.Tile);
		targetTile.BattleEntity = battleEntity;
		battleEntity.Tile = targetTile;
		if (targetTile.BattleEntity is PlayerEntity)
		{
			_entityLayer.SetCell(targetTile.IsometricPosition, 0, new Vector2I(0, 0));
		}
		else if (targetTile.BattleEntity is EnemyEntity enemyEntity)
		{
			_entityLayer.SetCell(targetTile.IsometricPosition, 1, new Vector2I(0, (int)enemyEntity.EnemyId - 1));
		}
		
		return true;
	}
	
	public void SetObstacleOnTile(Tile tile, ObstacleEntity obstacle)
	{
		if (obstacle.Tile is not null) RemoveObstacleFromTile(tile);
		obstacle.Tile = tile;
		tile.ObstacleEntity = obstacle;
		if (tile.ObstacleEntity is ObstacleEntity)
		{
			_obstacleLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(0, (int)tile.ObstacleEntity.ObstacleCode));
		}
	}
	
	public void RemoveObstacleFromTile(Tile tile)
	{
		_obstacleLayer.EraseCell(tile.IsometricPosition);
		if (tile.ObstacleEntity is not null)
		{
			tile.ObstacleEntity.Tile = null;
			tile.ObstacleEntity = null;
		}
	}
	
	/// <summary>
	/// Снятие любой сущности с Tile.
	/// </summary>
	/// <param name="tile">Cущность с которой происходит снятие</param>
	/// <param name="isDead">Флажок указывающий на то, что сущность погибла. Если true, тогда будет проигрываться анимация уничтожения сущности</param>
	public void RemoveBattleEntityFromTile(Tile tile, bool isDead = false)
	{
		_entityLayer.EraseCell(tile.IsometricPosition);
		if (isDead) _ = PlayRemoveBattleEntityAnimation(tile.IsometricPosition);
		tile.BattleEntity = null;
	}

	public void SelectTileForCurrentPlayerEntityTurn(Tile tile)
	{
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I((int)TileTextureTypes.CurrentTurnEntity, 0));
	}
	
	public void SelectTileForCurrentEnemyEntityTurn(Tile tile)
	{
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I((int)TileTextureTypes.CurrentEnemyTurnEntity, 0));
	}
	
	public void SelectTileForMovement(Tile tile)
	{
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I((int)TileTextureTypes.SelectedToMove, 0));
	}

	public void SelectTileForAttack(Tile tile)
	{
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I((int)TileTextureTypes.Closed, 0));
	}
	
	public void DeselectTile(Tile tile)
	{
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I((int)TileTextureTypes.Opened, 0));
	}


	/// <summary>
	/// Метод для подсчёта и отрисовки возможных тайлов к передвижению воина игрока
	/// </summary>
	public void CalculateAndDrawPlayerEntitySpeedZone(PlayerEntity playerEntity)
	{
		var tilesToMove = PathFinder.FindTilesToMove(playerEntity.Tile, this, playerEntity.Speed);
		_selectedTilesForPlayerAction = tilesToMove;
		_selectedTilesForPlayerAction.Add(playerEntity.Tile);
		foreach (var tile in _selectedTilesForPlayerAction)
		{
			if (tile == playerEntity.Tile) SelectTileForCurrentPlayerEntityTurn(tile);
			else SelectTileForMovement(tile);
		}
	}

	/// <summary>
	/// Метод для просчёта и отрисовки возможных тайлов к атаке воином игрока
	/// </summary>
	public void CalculateAndDrawPlayerEntityAttackZone(PlayerEntity playerEntity, Tile targetTile)
	{
		var tileToAttack = PathFinder.FindTilesToAttack(playerEntity, targetTile, this);
		_selectedTilesForPlayerAction = tileToAttack;
		foreach (var tile in _selectedTilesForPlayerAction)
		{
			SelectTileForAttack(tile);
		}
	}
	/// <summary>
	/// Метод для просчёта и отрисовки возможных тайлов для применения скилла
	/// </summary>
	public void CalculateAndDrawPlayerEntitySkillZone(PlayerEntity playerEntity, Tile targetTile)
    {
		var tileBySkill = PathFinder.FindTilesBySkill(playerEntity, targetTile, this);
		_selectedTilesForPlayerAction = tileBySkill;
		foreach (var tile in _selectedTilesForPlayerAction)
        {
			SelectTileForAttack(tile);
        }
    }

	public void DrawEnemyEntityTilesToMove(List<Tile> tilesToMove, EnemyEntity enemyEntity)
	{
		foreach (var tile in tilesToMove)
		{
			if (tile == enemyEntity.Tile) SelectTileForCurrentEnemyEntityTurn(tile);
			else SelectTileForMovement(tile);
		}
	}

	public void DrawEnemyEntityTilesToAttack(List<Tile> tilesToAttack)
	{
		foreach (var tile in tilesToAttack)
		{
			SelectTileForAttack(tile);
		}
	}
	
	public void ClearDrawEnemyEntityTilesToAttack(List<Tile> tilesToAttack)
	{
		foreach (var tile in tilesToAttack)
		{
			DeselectTile(tile);
		}
	}
	
	public void ClearDrawEnemyEntityTilesToMove(List<Tile> tilesToMove)
	{
		foreach (var tile in tilesToMove)
		{
			DeselectTile(tile);
		}
	}

	public bool MovePlayerEntityInSpeedZone(PlayerEntity playerEntity)
	{
		var tile = GetTileInSelectedUnderMousePosition();
		return tile is not null && SetBattleEntityOnTile(tile, playerEntity);
	}

	public void ClearAllSelectedTiles()
	{
		foreach (var selectedTile in _selectedTilesForPlayerAction)
		{
			DeselectTile(selectedTile);
		}
		_selectedTilesForPlayerAction.Clear();
	}
	
	public Tile? GetTileUnderMousePosition()
	{
		var clickedCell = _floorLayer.LocalToMap(_floorLayer.GetLocalMousePosition() + new Vector2I(0, 16));
		return GetTileByIsometricCoord(clickedCell);
	}
	
	public Tile? GetTileInSelectedUnderMousePosition()
	{
		var tile = GetTileUnderMousePosition();
		if (tile is null) return null;
		if (!_selectedTilesForPlayerAction.Contains(tile)) return null;
		return tile;
	}


	public async Task PlayRemoveBattleEntityAnimation(Vector2I isometricPosition)
	{	
		_removeEntitiesEffectLayer.SetCell(isometricPosition, 0, new Vector2I(0, 0));
		await Task.Delay(500);
		_removeEntitiesEffectLayer.EraseCell(isometricPosition);
	}
}