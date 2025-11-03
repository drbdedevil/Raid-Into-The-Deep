#nullable enable
using System;
using System.Collections.Generic;
using Godot;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

public partial class MapManager : Node2D
{
	private List<List<Vector2I>> _cartesianCoords = [];
	private List<List<Vector2I>> _isometricCoords = [];
	private readonly List<Tile> _mapTiles = [];
	
	private TileMapLayer _floorLayer;
	private TileMapLayer _entityLayer;
	
	/// <summary>
	/// Ассоциативный массив для получения изометрической координаты по декартовой
	/// </summary>
	private readonly Dictionary<Vector2I, Tile> _tilesByCartesian = new();
	
	/// <summary>
	/// Ассоциативный массив для получения декартовой координаты по изометрической
	/// </summary>
	private readonly Dictionary<Vector2I, Tile> _tilesByIsometric = new();

	private Tile? _selectedTile = null;

	[Signal]
	public delegate void OnTileLeftButtonClickedEventHandler(Vector2I tile);
	
	[Signal]
	public delegate void OnTileRightButtonClickedEventHandler(Fight.BattleEntity battleEntity);


	private List<Tile> _selectedTilesForPlayerWarriorMove { get; set; } = [];
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			var tile = GetTileUnderMousePosition();
		}
		
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
		foreach (var instanceTile in BattleMapInitStateManager.Instance.Tiles)
		{
			_mapTiles.Add(instanceTile);
			_tilesByCartesian.Add(instanceTile.CartesianPosition, instanceTile);
			_tilesByIsometric.Add(instanceTile.IsometricPosition, instanceTile);
			DeselectTile(instanceTile);
			if (instanceTile.BattleEntity is not null) InitBattleEntityOnTile(instanceTile);
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
		if (tile.BattleEntity is null) return;
		if (tile.BattleEntity is PlayerEntity playerEntity)
		{
			_entityLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(0, 0));
		}
		else if (tile.BattleEntity is EnemyEntity enemyEntity)
		{
			_entityLayer.SetCell(tile.IsometricPosition, (int)enemyEntity.EnemyId, new Vector2I(0, 0));
		}
	}

	public bool SetBattleEntityOnTile(Tile tile, BattleEntity battleEntity)
	{
		if (tile.BattleEntity is not null) return false;
		RemoveBattleEntityOnTile(battleEntity.Tile);
		tile.BattleEntity = battleEntity;
		if (tile.BattleEntity is PlayerEntity playerEntity)
		{
			_entityLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(0, 0));
		}
		else if (tile.BattleEntity is EnemyEntity enemyEntity)
		{
			_entityLayer.SetCell(tile.IsometricPosition, (int)enemyEntity.EnemyId, new Vector2I(0, 0));
		}
		
		return true;
	}
	public void RemoveBattleEntityOnTile(Tile tile)
	{
		_entityLayer.EraseCell(tile.IsometricPosition);
		tile.BattleEntity = null;
	}
	
	public void SelectTile(Tile tile)
	{
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(1, 0));
	}
	
	public void DeselectTile(Tile tile)
	{
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(0, 0));
	}


	/// <summary>
	/// Метод для отрисовки возможных тайлов к передвижению воина игрока
	/// </summary>
	public void DrawPlayerEntitySpeedZone(PlayerEntity playerEntity)
	{
		foreach (var selectedTile in _selectedTilesForPlayerWarriorMove)
		{
			DeselectTile(selectedTile);	
		}
		_selectedTilesForPlayerWarriorMove.Clear();
		var tile = playerEntity.Tile;
		
		for (int i = 1; i <= playerEntity.Speed; i++)
		{
			var upPosition = tile.CartesianPosition + Vector2I.Up;
			var downPosition = tile.CartesianPosition + Vector2I.Down;
			var leftPosition = tile.CartesianPosition + Vector2I.Left;
			var rightPosition = tile.CartesianPosition + Vector2I.Right;
			
			var upTile = GetTileByCartesianCoord(upPosition);
			var downTile = GetTileByCartesianCoord(downPosition);
			var leftTile = GetTileByCartesianCoord(leftPosition);
			var rightTile = GetTileByCartesianCoord(rightPosition);

			if (upTile is not null)
			{
				SelectTile(upTile);
				_selectedTilesForPlayerWarriorMove.Add(upTile);
			}

			if (downTile is not null)
			{
				SelectTile(downTile);
				_selectedTilesForPlayerWarriorMove.Add(downTile);
			}

			if (leftTile is not null)
			{
				SelectTile(leftTile);
				_selectedTilesForPlayerWarriorMove.Add(leftTile);
			}

			if (rightTile is not null)
			{
				SelectTile(rightTile);
				_selectedTilesForPlayerWarriorMove.Add(rightTile);
			}

			if (i % 2 != 0) continue;
			var upLeftPosition = tile.CartesianPosition + Vector2I.Up + Vector2I.Left;
			var upRightPosition = tile.CartesianPosition + Vector2I.Up + Vector2I.Right;
			var downLeftPosition = tile.CartesianPosition + Vector2I.Down + Vector2I.Left;
			var downRightPosition = tile.CartesianPosition + Vector2I.Down + Vector2I.Right;
				
			var upLeftTile = GetTileByCartesianCoord(upLeftPosition);
			var upRightTile = GetTileByCartesianCoord(upRightPosition);
			var downLeftTile = GetTileByCartesianCoord(downLeftPosition);
			var downRightTile = GetTileByCartesianCoord(downRightPosition);

			if (upLeftTile is not null)
			{
				SelectTile(upLeftTile);
				_selectedTilesForPlayerWarriorMove.Add(upLeftTile);
			}

			if (upRightTile is not null)
			{
				SelectTile(upRightTile);
				_selectedTilesForPlayerWarriorMove.Add(upRightTile);
			}

			if (downLeftTile is not null)
			{
				SelectTile(downLeftTile);
				_selectedTilesForPlayerWarriorMove.Add(downLeftTile);
			}

			if (downRightTile is not null)
			{
				SelectTile(downRightTile);
				_selectedTilesForPlayerWarriorMove.Add(downRightTile);
			}
		}
	}

	public bool MovePlayerEntityInSpeedZone(PlayerEntity playerEntity)
	{
		var tile = GetTileInSelectedUnderMousePosition();
		return tile is not null && SetBattleEntityOnTile(tile, playerEntity);
	}

	public void ClearAllSelectedTiles()
	{
		foreach (var selectedTile in _selectedTilesForPlayerWarriorMove)
		{
			DeselectTile(selectedTile);
		}
		_selectedTilesForPlayerWarriorMove.Clear();
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
		if (!_selectedTilesForPlayerWarriorMove.Contains(tile)) return null;
		return tile;
	}
	
}