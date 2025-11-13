using System;

namespace RaidIntoTheDeep.Levels.Fight;

public enum ObstacleCode
{
    Wall,
    Rock,
    Poison,
    Fire,
    Totem
}

public static class ObstacleCodeExtensions
{
    public static string GetMapCode(this ObstacleCode code)
    {
        switch (code)
        {
            case ObstacleCode.Wall: return "w";
            case ObstacleCode.Rock: return "r";
            case ObstacleCode.Poison: return "p";
            case ObstacleCode.Fire: return "f";
            default: return "";
        }
    }
    
    public static ObstacleCode GetByMapCode(this ObstacleCode code, string mapCode)
    {
        switch (mapCode)
        {
            case "w": return ObstacleCode.Wall;
            case "r": return ObstacleCode.Rock;
            case "p": return ObstacleCode.Poison;
            case "f": return ObstacleCode.Fire;
            default: throw new ArgumentOutOfRangeException("Не существует такого препятствия");
        }
    }
}