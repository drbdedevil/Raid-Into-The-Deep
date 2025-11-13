using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                
                if (upTile is not null && upTile.IsAllowedToSetBattleEntity) queue.Enqueue(new TileCostPair(upTile, currentTileCost.Cost + 1));
                if (downTile is not null && downTile.IsAllowedToSetBattleEntity) queue.Enqueue(new TileCostPair(downTile, currentTileCost.Cost + 1));
                if (leftTile is not null && leftTile.IsAllowedToSetBattleEntity) queue.Enqueue(new TileCostPair(leftTile, currentTileCost.Cost + 1));
                if (rightTile is not null && rightTile.IsAllowedToSetBattleEntity) queue.Enqueue(new TileCostPair(rightTile, currentTileCost.Cost + 1));
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
                
                if (upLeftTile is not null && upLeftTile.IsAllowedToSetBattleEntity) queue.Enqueue(new TileCostPair(upLeftTile, currentTileCost.Cost + 2));
                if (upRightTile is not null && upRightTile.IsAllowedToSetBattleEntity) queue.Enqueue(new TileCostPair(upRightTile, currentTileCost.Cost + 2));
                if (downLeftTile is not null && downLeftTile.IsAllowedToSetBattleEntity) queue.Enqueue(new TileCostPair(downLeftTile, currentTileCost.Cost + 2));
                if (downRightTile is not null && downRightTile.IsAllowedToSetBattleEntity) queue.Enqueue(new TileCostPair(downRightTile, currentTileCost.Cost + 2));
            }
            result.Add(tile);
        }
        
        return result;
    }

    public static List<Tile> FindTilesToAttack(BattleEntity attacker, Tile targetTile, MapManager mapManager, Tile startTile = null)
    {
        var result = new List<Tile>();
        
        if (startTile is null) startTile = attacker.Tile;
        var shapeTargets = attacker.Weapon.CalculateShapeAttackPositions(startTile.CartesianPosition, targetTile.CartesianPosition, mapManager);
        
        foreach (var shapeTarget in shapeTargets)
        {
            var attackTile = mapManager.GetTileByCartesianCoord(shapeTarget);
            if (attackTile is not null) result.Add(attackTile);
        }
        return result;
    }

    public static List<Tile> CalculatePathToTarget(Tile startTile, Tile targetTile, MapManager mapManager, BattleEntity battleEntity)
    {
        var result = new List<Tile>();
        var speed = battleEntity.Speed;

        List<TilePath> results = [];
        
        Stopwatch stopwatch = new Stopwatch();
        CollectPathsRecursive(startTile, targetTile, [], 0, results, []);

        var pathsResult = results.OrderByDescending(x => x.CalculateScore())
            .GroupBy(x => x.CalculateScore()).ToDictionary(x => x.Key, x => x.ToList());

        
        return pathsResult.FirstOrDefault().Value.OrderBy(x => x.Count).Select(x => x.Path).FirstOrDefault();
        
        void CollectPathsRecursive(
            Tile current,
            Tile target,
            List<Tile> currentPath,
            int currentSpeed,
            List<TilePath> results,
            HashSet<Tile> visited)
        {
            
            if (visited.Contains(current))
                return;
            
            visited.Add(current);
            currentPath.Add(current);

            // Если достигли старта — сохраняем полный путь
            if (current == target)
            {
                results.Add(new TilePath { Path = currentPath.ToList()});
            }
            else if (currentSpeed < speed)
            {
                Tile tile = current;

                if (currentSpeed + 1 <= speed)
                {
                    var upPosition = tile.CartesianPosition + Vector2I.Up;
                    var downPosition = tile.CartesianPosition + Vector2I.Down;
                    var leftPosition = tile.CartesianPosition + Vector2I.Left;
                    var rightPosition = tile.CartesianPosition + Vector2I.Right;

                    var upTile = mapManager.GetTileByCartesianCoord(upPosition);
                    var downTile = mapManager.GetTileByCartesianCoord(downPosition);
                    var leftTile = mapManager.GetTileByCartesianCoord(leftPosition);
                    var rightTile = mapManager.GetTileByCartesianCoord(rightPosition);

                    if (upTile is not null && upTile.IsAllowedToSetBattleEntity) CollectPathsRecursive(upTile, target, currentPath.ToList(), currentSpeed + 1, results, visited.ToHashSet());
                    if (downTile is not null && downTile.IsAllowedToSetBattleEntity) CollectPathsRecursive(downTile, target, currentPath.ToList(), currentSpeed + 1, results, visited.ToHashSet());
                    if (leftTile is not null && leftTile.IsAllowedToSetBattleEntity) CollectPathsRecursive(leftTile, target, currentPath.ToList(), currentSpeed + 1, results, visited.ToHashSet());
                    if (rightTile is not null && rightTile.IsAllowedToSetBattleEntity) CollectPathsRecursive(rightTile, target, currentPath.ToList(), currentSpeed + 1, results, visited.ToHashSet());
                }
                if (currentSpeed + 2 <= speed)
                {
                    var upLeftPosition = tile.CartesianPosition + Vector2I.Up + Vector2I.Left;
                    var upRightPosition = tile.CartesianPosition + Vector2I.Up + Vector2I.Right;
                    var downLeftPosition = tile.CartesianPosition + Vector2I.Down + Vector2I.Left;
                    var downRightPosition = tile.CartesianPosition + Vector2I.Down + Vector2I.Right;
				    
                    var upLeftTile = mapManager.GetTileByCartesianCoord(upLeftPosition);
                    var upRightTile = mapManager.GetTileByCartesianCoord(upRightPosition);
                    var downLeftTile = mapManager.GetTileByCartesianCoord(downLeftPosition);
                    var downRightTile = mapManager.GetTileByCartesianCoord(downRightPosition);
                    
                    if (upLeftTile is not null && upLeftTile.IsAllowedToSetBattleEntity) CollectPathsRecursive(upLeftTile, target, currentPath.ToList(), currentSpeed + 2, results, visited.ToHashSet());
                    if (upRightTile is not null && upRightTile.IsAllowedToSetBattleEntity) CollectPathsRecursive(upRightTile, target, currentPath.ToList(), currentSpeed + 2, results, visited.ToHashSet());
                    if (downLeftTile is not null && downLeftTile.IsAllowedToSetBattleEntity) CollectPathsRecursive(downRightTile, target, currentPath.ToList(), currentSpeed + 2, results, visited.ToHashSet());
                    if (downRightTile is not null && downRightTile.IsAllowedToSetBattleEntity) CollectPathsRecursive(downRightTile, target, currentPath.ToList(), currentSpeed + 2, results, visited.ToHashSet());
                }
            }

            currentPath.RemoveAt(currentPath.Count - 1);
            visited.Remove(current);
        }
        
        return result;
    }

    public static int CalculateDistanceToTile(Tile startTile, Tile targetTile)
    {
        return (int)startTile.CartesianPosition.DistanceTo(targetTile.CartesianPosition);
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

    public override bool Equals(object obj)
    {
        if (obj is TileCostPair other) return other.Target.Equals(Target) && other.Cost == Cost;
        return false;
    }

    public override int GetHashCode()
    {
        return Target.GetHashCode() ^ Cost;
    }
}

class TilePath
{
    public List<Tile> Path { get; set; } = [];

    public int CalculateScore()
    {
        int score = 0;
        foreach (var tile in Path)
        {
            if (tile.ObstacleEntity is not null && tile.ObstacleEntity.ImposedEffect is not null)
            {
                if (tile.ObstacleEntity.ImposedEffect.EffectType.GetCategory() == EffectKind.Positive) score++;
                else score--;
            }
        }
        
        return score;
    }
    public int Count => Path.Count;
    
    public override bool Equals(object obj)
    {
        if (obj is TilePath other) return other.Path.SequenceEqual(Path);
        return false;
    }
    public override int GetHashCode()
    {
        if (Path == null || !Path.Any()) return 0;
        var code = Path.FirstOrDefault().GetHashCode();
        for (int i = 1; i < Path.Count - 1; i++) code ^= Path[i].GetHashCode();
        return code;
    }
    
    
}