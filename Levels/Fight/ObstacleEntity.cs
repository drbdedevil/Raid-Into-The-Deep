using System;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using RaidIntoTheDeep.Levels.Fight.Weapons;

namespace RaidIntoTheDeep.Levels.Fight;


/// <summary>
/// Препятствие
/// </summary>
public partial class ObstacleEntity : BattleEntity
{
    public ObstacleEntity(Tile tile, ObstacleCode obstacleCode) : base(tile, null, Guid.NewGuid().ToString(), 0, Int32.MaxValue, 0)
    {
        ObstacleCode = obstacleCode;
        switch (ObstacleCode)
        {
            case ObstacleCode.Wall:
            case ObstacleCode.Rock:
                IsImpassable = true;
                break;
            default:
                IsImpassable = false;
                break;
        }
    }
    
    public ObstacleCode ObstacleCode { get; set; }
    
    /// <summary>
    /// Непроходимое ли препятствие? 
    /// </summary>
    public bool IsImpassable { get; set; }
}