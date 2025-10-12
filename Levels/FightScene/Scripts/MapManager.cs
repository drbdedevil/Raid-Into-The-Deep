using Godot;
using System;
using System.Collections.Generic;

public partial class MapManager : TileMapLayer
{
    private List<List<Vector2I>> _normalCoords = [];
    private List<List<Vector2I>> _isometricCoords = []; 
    
    public override void _Ready()
    {
        var ySize = 6;
        var xSize = 5;
        for (int y = 0; y < ySize; y++)
        {
            var normalRow = new List<Vector2I>();
            var isometricRow = new List<Vector2I>();
            for (int x = 0; x < xSize; x++)
            {
                normalRow.Add(new Vector2I(x, y));
                var isometricCoord = y % 2 == 0 ? CalculateEvenVector(x) : CalculateOddVector(x);
                isometricRow.Add(isometricCoord);
                SetCell(isometricCoord, 0, new Vector2I(0, 0));
            }
            _normalCoords.Add(normalRow);
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
}
