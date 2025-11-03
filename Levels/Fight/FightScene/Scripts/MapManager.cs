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
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			var tile = GetTileUnderMousePosition();
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
		
		if (@event is InputEventMouseButton mouseButton && @event.IsPressed())
		{
			if (mouseButton.Pressed)
			{ 
				var tile = GetTileUnderMousePosition();
				if (mouseButton.ButtonIndex == MouseButton.Left)
				{
					if (tile != null && tile.BattleEntity == null) EmitSignalOnTileLeftButtonClicked(tile.CartesianPosition);
				}
				else if (mouseButton.ButtonIndex == MouseButton.Right)
				{
					if (tile?.BattleEntity != null) EmitSignalOnTileRightButtonClicked(tile.BattleEntity);
				}
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
	
	public void RemoveEnemyOnTile(Tile tile)
	{
		_entityLayer.EraseCell(tile.IsometricPosition);
		tile.BattleEntity = null;
	}

	public Tile? GetTileUnderMousePosition()
	{
		var clickedCell = _floorLayer.LocalToMap(_floorLayer.GetLocalMousePosition() + new Vector2I(0, 16));
		return GetTileByIsometricCoord(clickedCell);
	}

	public void SelectTile(Tile tile)
	{
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(1, 0));
	}
	
	public void DeselectTile(Tile tile)
	{
		_floorLayer.SetCell(tile.IsometricPosition, 0, new Vector2I(0, 0));
	}


}