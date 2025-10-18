using Godot;
using System;
using System.Collections.Generic;

public partial class MapManager : TileMapLayer
{
	private List<List<Vector2I>> _decartCoords = [];
	private List<List<Vector2I>> _isometricCoords = [];

	
	/// <summary>
	/// Ассоциативный массив для получения изометрической координаты по декартовой
	/// </summary>
	private readonly Dictionary<Vector2I, Vector2I> _isometricByDecart = new();
	
	/// <summary>
	/// Ассоциативный массив для получения декартовой координаты по изометрической
	/// </summary>
	private readonly Dictionary<Vector2I, Vector2I> _decartByIsometric = new();
	
	public override void _Ready()
	{
		var ySize = 8;
		var xSize = 8;
		for (int y = 0; y < ySize; y++)
		{
			var normalRow = new List<Vector2I>();
			var isometricRow = new List<Vector2I>();
			for (int x = 0; x < xSize; x++)
			{
				var decartCoord = new Vector2I(x, y); 
				var isometricCoord = y % 2 == 0 ? CalculateEvenVector(x) : CalculateOddVector(x);
				
				isometricRow.Add(isometricCoord);
				normalRow.Add(decartCoord);
				SetCell(isometricCoord, 0, new Vector2I(0, 0));
				_decartByIsometric.Add(isometricCoord, decartCoord);
				_isometricByDecart.Add(decartCoord, isometricCoord);
			}
			_decartCoords.Add(normalRow);
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

	public Vector2I GetIsometricCoordByDecartCoord(Vector2I decartCoord)
	{
		if (!_isometricByDecart.TryGetValue(decartCoord, out Vector2I isometricCoord))
			throw new ApplicationException($"Не нашёлся Tile с такими декартовыми координатами-{decartCoord}");
		return isometricCoord;
	}

	public Vector2I GetDecartCoordByIsometricCoord(Vector2I isometricCoord)
	{
		if (!_decartByIsometric.TryGetValue(isometricCoord, out Vector2I decartCoord))
			throw new ApplicationException($"Не нашёлся Tile с такими изометрическими координатами-{isometricCoord}");
		return decartCoord;
	}
}
