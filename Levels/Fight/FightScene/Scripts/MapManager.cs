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
				var mapTile = new Tile(cartesianCoord, isometricCoord, new Vector2I(32, 16));
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