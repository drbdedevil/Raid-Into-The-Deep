using System;
using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.PrepareFightScene;

public static class MapParser
{
    public static (List<Tile> Tiles, Vector2I Size) LoadFromText(string mapText)
    {
        string[] lines = mapText.Split('\n');
        string[] sizeLine = lines[0].Split(" ");
        int width = int.Parse(sizeLine[0]);
        int height = int.Parse(sizeLine[1]);

        string[] mapLines = lines[1 .. (height + 1)];

        List<Tile> tiles = [];
        
        for (int y = 0; y < height; y++)
        {
            var row = mapLines[y].Split(' ');
            for (int x = 0; x < width; x++)
            {
                var mapElement = row[x];
                var cartesianCoord = new Vector2I(x, y);

                var tile = new Tile(cartesianCoord, CalculateIsometricCoord(cartesianCoord), new Vector2I(32, 16),
                    mapElement != "o");
                tiles.Add(tile);
                
                
                if (mapElement != "e" && mapElement != "o" && mapElement != "c")
                {
                    
                    bool battleEntityIsEnemy = int.TryParse(mapElement, out int entityCode);
                    if (battleEntityIsEnemy)
                    {
                        EnemyEntity battleEntity;
                        var gameEnemyCode = (GameEnemyCode)entityCode;
                        battleEntity = new EnemyEntity(tile, gameEnemyCode, GameDataManager.Instance.EnemyBaseStatsDatabase.EnemyBaseStatRows[gameEnemyCode]);
                        tile.BattleEntity = battleEntity;
                    }
                    else
                    {
                        var obstacleCode = ObstacleCode.Wall; // Знаю, что это бред
                        obstacleCode =  obstacleCode.GetByMapCode(mapElement);
                        var battleEntity = new ObstacleEntity(tile, obstacleCode);
                        tile.ObstacleEntity = battleEntity;
                    }
                }

            }
        }
        
        return (tiles, new Vector2I(width, height));
    }

    public static Vector2I CalculateIsometricCoord(Vector2 cartesianCoord)
    {
        var y = (int)cartesianCoord.Y;
        var x =  (int)cartesianCoord.X; 
        
        return y % 2 == 0 ? CalculateEvenVector(x) : CalculateOddVector(x);
        
        
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