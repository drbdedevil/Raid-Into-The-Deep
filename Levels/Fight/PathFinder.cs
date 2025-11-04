using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight;

public static class PathFinder
{
    public static List<Tile> FindTilesToMove(Tile startTile, MapManager mapManager, int speed)
    {
        var result = new List<Tile>();
        Queue<TileCostPair> queue = new();
        queue.Enqueue(new TileCostPair(startTile, 0));

        while (queue.Count > 0)
        {
            TileCostPair currentTileCost = queue.Dequeue();
            Tile tile = currentTileCost.Target;

            if (currentTileCost.Cost + 1 <= speed)
            {
                var upPosition = tile.CartesianPosition + Vector2I.Up;
                var downPosition = tile.CartesianPosition + Vector2I.Down;
                var leftPosition = tile.CartesianPosition + Vector2I.Left;
                var rightPosition = tile.CartesianPosition + Vector2I.Right;

                var upTile = mapManager.GetTileByCartesianCoord(upPosition);
                var downTile = mapManager.GetTileByCartesianCoord(downPosition);
                var leftTile = mapManager.GetTileByCartesianCoord(leftPosition);
                var rightTile = mapManager.GetTileByCartesianCoord(rightPosition);
                
                if (upTile is not null) queue.Enqueue(new TileCostPair(upTile, currentTileCost.Cost + 1));
                if (downTile is not null) queue.Enqueue(new TileCostPair(downTile, currentTileCost.Cost + 1));
                if (leftTile is not null) queue.Enqueue(new TileCostPair(leftTile, currentTileCost.Cost + 1));
                if (rightTile is not null) queue.Enqueue(new TileCostPair(rightTile, currentTileCost.Cost + 1));
            }
            if (currentTileCost.Cost + 2 <= speed)
            {
                var upLeftPosition = tile.CartesianPosition + Vector2I.Up + Vector2I.Left;
                var upRightPosition = tile.CartesianPosition + Vector2I.Up + Vector2I.Right;
                var downLeftPosition = tile.CartesianPosition + Vector2I.Down + Vector2I.Left;
                var downRightPosition = tile.CartesianPosition + Vector2I.Down + Vector2I.Right;
				
                var upLeftTile = mapManager.GetTileByCartesianCoord(upLeftPosition);
                var upRightTile = mapManager.GetTileByCartesianCoord(upRightPosition);
                var downLeftTile = mapManager.GetTileByCartesianCoord(downLeftPosition);
                var downRightTile = mapManager.GetTileByCartesianCoord(downRightPosition);
                
                if (upLeftTile is not null) queue.Enqueue(new TileCostPair(upLeftTile, currentTileCost.Cost + 2));
                if (upRightTile is not null) queue.Enqueue(new TileCostPair(upRightTile, currentTileCost.Cost + 2));
                if (downLeftTile is not null) queue.Enqueue(new TileCostPair(downLeftTile, currentTileCost.Cost + 2));
                if (downRightTile is not null) queue.Enqueue(new TileCostPair(downRightTile, currentTileCost.Cost + 2));
            }
            result.Add(tile);
        }
        
        result.Remove(startTile);
        
        return result;
    }

    public static List<Tile> FindTilesToAttack(BattleEntity attacker, Tile targetTile, MapManager mapManager)
    {
        var result = new List<Tile>();

        var shapeTargets = attacker.Weapon.CalculateShapeAttackPositions(attacker.Tile.CartesianPosition, targetTile.CartesianPosition);
        
        foreach (var shapeTarget in shapeTargets)
        {
            var attackTile = mapManager.GetTileByCartesianCoord(shapeTarget);
            if (attackTile is not null) result.Add(attackTile);
        }
        return result;
    }
}

class TileCostPair
{
    public TileCostPair(Tile target, int cost)
    {
        Target = target;
        Cost = cost;
    }

    public Tile Target { get; init; }
    public int Cost { get; init; }
}