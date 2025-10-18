using Godot;
using System;

public partial class FightSceneManager : Node2D
{
    private readonly RayCast2D _rayCast = new ();
    private MapManager _mapManager;

    public override void _Ready()
    {
        _mapManager = GetNode<MapManager>("TileMapLayer");
    }

    public override void _Process(double delta)
    {
        var tilePos = _mapManager.LocalToMap(_mapManager.GetLocalMousePosition());
        try
        {
            GD.Print(
                $"Изометрические координаты - {tilePos}. Декартовые координаты - {_mapManager.GetDecartCoordByIsometricCoord(tilePos)}");
        }
        catch (ApplicationException )
        {
        }
    }
}
