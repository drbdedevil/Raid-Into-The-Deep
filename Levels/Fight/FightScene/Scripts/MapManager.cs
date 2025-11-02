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
	public delegate void OnTileRightButtonClickedEventHandler(BattleEntity battleEntity);
	
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
					if (tile != null && tile.BattleEntity?.Character == null) EmitSignalOnTileLeftButtonClicked(tile.CartesianPosition);
				}
				else if (mouseButton.ButtonIndex == MouseButton.Right)
				{
					if (tile?.BattleEntity?.Character != null) EmitSignalOnTileRightButtonClicked(tile.BattleEntity);
				}
			}
			
		}
	}

	public override void _Ready()
	{
		_floorLayer = GetNode<TileMapLayer>("Floor");
		_entityLayer = GetNode<TileMapLayer>("Entities");
		
		var ySize = 8;
		var xSize = 8;
		for (int y = 0; y < ySize; y++)
		{
			var cartesianRow = new List<Vector2I>();
			var isometricRow = new List<Vector2I>();
			for (int x = 0; x < xSize; x++)
			{
				var cartesianCoord = new Vector2I(x, y); 
				var isometricCoord = y % 2 == 0 ? CalculateEvenVector(x) : CalculateOddVector(x);
				
				isometricRow.Add(isometricCoord);
				cartesianRow.Add(cartesianCoord);
				_floorLayer.SetCell(isometricCoord, 0, new Vector2I(0, 0));
				var mapTile = new Tile(cartesianCoord, isometricCoord, new Vector2I(32, 16), false);
				_tilesByIsometric.Add(isometricCoord, mapTile);
				_tilesByCartesian.Add(cartesianCoord, mapTile);
				_mapTiles.Add(mapTile);
			}
			_cartesianCoords.Add(cartesianRow);
			_isometricCoords.Add(isometricRow);

			Vector2I CalculateEvenVector(int x) => new (
				y + (int)Math.Round((x) / 2f, MidpointRounding.ToNegativeInfinity) -
				3 * (int)Math.Round((decimal)(y) / 2, MidpointRounding.ToNegativeInfinity),
				x + 2 * (int)Math.Round((decimal)(y) / 2, MidpointRounding.ToNegativeInfinity));

			Vector2I CalculateOddVector(int x) => new (
				(int)Math.Round((-y) / 2f, MidpointRounding.ToNegativeInfinity) +
				(int)Math.Round((decimal)(x + 1) / 2, MidpointRounding.ToNegativeInfinity),
				(int)Math.Round((decimal)(y) / 2, MidpointRounding.ToNegativeInfinity) * 2 + x + 1);
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

	public void SetEnemyOnTile(Tile tile, BattleEntity enemy)
	{
		if (enemy.Tile is not null) _entityLayer.EraseCell(enemy.Tile.IsometricPosition);
		_entityLayer.SetCell(tile.IsometricPosition, 1, new Vector2I(0, 0));
		enemy.Tile = tile;
		tile.BattleEntity = enemy;
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