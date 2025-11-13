#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using FileAccess = Godot.FileAccess;

namespace RaidIntoTheDeep.Levels.Fight.PrepareFightScene;


public partial class PrepareFightMapManager : Node2D
{
	private List<List<Vector2I>> _cartesianCoords = [];
	private List<List<Vector2I>> _isometricCoords = [];
	private List<Tile> _mapTiles = [];
	
	private TileMapLayer _floorLayer;
	private TileMapLayer _entityLayer;
	private TileMapLayer _obstacleLayer;
	
	/// <summary>
	/// Ассоциативный массив для получения изометрической координаты по декартовой
	/// </summary>
	private readonly Dictionary<Vector2I, Tile> _tilesByCartesian = new();
	
	/// <summary>
	/// Ассоциативный массив для получения декартовой координаты по изометрической
	/// </summary>
	private readonly Dictionary<Vector2I, Tile> _tilesByIsometric = new();

	private Tile? _selectedTile = null;

	public List<Tile> MapTiles => _mapTiles.ToList();
	
	[Export]
	public string MapName { get; set; }

	
	[Signal]
	public delegate void OnTileLeftButtonClickedEventHandler(Vector2I tile);
	
	[Signal]
	public delegate void OnTileRightButtonClickedEventHandler(PlayerEntity battleEntity);
	
	public override void _Input(InputEvent @event)
	{
		
		if (@event is InputEventMouseButton mouseButton && @event.IsPressed())
		{
			if (mouseButton.Pressed)
			{ 
				var tile = GetTileUnderMousePositionForWarriorPlacement();
				if (mouseButton.ButtonIndex == MouseButton.Left)
				{
					if (tile != null && tile.BattleEntity == null) EmitSignalOnTileLeftButtonClicked(tile.CartesianPosition);
				}
				else if (mouseButton.ButtonIndex == MouseButton.Right)
				{
					if (tile?.BattleEntity != null) EmitSignalOnTileRightButtonClicked(tile.BattleEntity as PlayerEntity);
				}
			}
			
		}
	}

	public override void _Process(double delta)
	{
		var tile = GetTileUnderMousePositionForWarriorPlacement();
		if (_selectedTile != null && tile == null || (_selectedTile != tile && tile != null && _selectedTile is not null) )
		{
			DeselectTile(_selectedTile);
			_selectedTile = null;
		}
		else if (tile != null)
		{
			_selectedTile = tile;
			SelectTile(_selectedTile);
		}
	}

	public override void _Ready()
	{
		_floorLayer = GetNode<TileMapLayer>("Floor");
		_entityLayer = GetNode<TileMapLayer>("Entities");
		_obstacleLayer = GetNode<TileMapLayer>("Obstacles");
		
		int randMap = new Random().Next(1, 10);
		var mapText = FileAccess.Open($"res://Maps/Обычные/map{randMap}.txt", FileAccess.ModeFlags.Read).GetAsText();
		(_mapTiles, var mapSize) = MapParser.LoadFromText(mapText);

		for (int y = 0; y < mapSize.Y; y++)
		{
			var cartesianCoords = new List<Vector2I>();
			var isometricCoords = new List<Vector2I>();
			for (int x = 0; x < mapSize.X; x++)
			{
				var tile = _mapTiles[x+(y * mapSize.Y)];
				cartesianCoords.Add(tile.CartesianPosition);
				isometricCoords.Add(tile.IsometricPosition);
				_tilesByCartesian.Add(tile.CartesianPosition, tile);
				_tilesByIsometric.Add(tile.IsometricPosition, tile);
				SetAtlasOriginalTextureForTile(tile);
				if (tile.BattleEntity != null) SetBattleEntityOnTile(tile, tile.BattleEntity);
				if (tile.ObstacleEntity != null) SetObstacleOnTile(tile, tile.ObstacleEntity);
			}
			_isometricCoords.Add(isometricCoords);
			_cartesianCoords.Add(cartesianCoords);
		}
		
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

	public void SetBattleEntityOnTile(Tile tile, BattleEntity entity)
	{
		if (entity.Tile is not null) _entityLayer.EraseCell(entity.Tile.IsometricPosition);
		if (entity is PlayerEntity)
		{
			_entityLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(0, 0));
		}
		else if (entity is EnemyEntity enemy)
		{
			_entityLayer.SetCell(tile.IsometricPosition, 1, new Vector2I(0, (int)enemy.EnemyId));
		}
		entity.Tile = tile;
		tile.BattleEntity = entity;
	}

	public void SetObstacleOnTile(Tile tile, ObstacleEntity obstacle)
	{
		if (tile.ObstacleEntity is not null) _obstacleLayer.EraseCell(tile.IsometricPosition);
		_obstacleLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(0, (int)obstacle.ObstacleCode));
		obstacle.Tile = tile;
		tile.ObstacleEntity = obstacle;
	}
	
	public void RemoveEnemyOnTile(Tile tile)
	{
		_entityLayer.EraseCell(tile.IsometricPosition);
		tile.BattleEntity = null;
	}
	
	public Tile? GetTileUnderMousePositionForWarriorPlacement()
	{
		var clickedCell = _floorLayer.LocalToMap(_floorLayer.GetLocalMousePosition() + new Vector2I(0, 16));
		var tile = GetTileByIsometricCoord(clickedCell); 
		if (tile == null || tile.IsClosedToSetPlayerWarrior) return null;
		return tile;
	}

	public void SelectTile(Tile tile)
	{
		if (!tile.IsClosedToSetPlayerWarrior)  _floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(1, 0));
	}
	
	public void DeselectTile(Tile tile)
	{
		SetAtlasOriginalTextureForTile(tile);
	}


	/// <summary>
	/// Функция установки тайлу его "Оригинальной" текстуры.
	/// т.е. той которая не является текстурой "Выделения" 
	/// </summary>
	/// <param name="tile"></param>
	private void SetAtlasOriginalTextureForTile(Tile tile)
	{
		const int openedTileTexture = 0;
		const int closedTileTexture = 2;
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(tile.IsClosedToSetPlayerWarrior ? closedTileTexture : openedTileTexture, 0));
	}

}
