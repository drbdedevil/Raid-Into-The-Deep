using System;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using RaidIntoTheDeep.Levels.Fight.Weapons;

namespace RaidIntoTheDeep.Levels.Fight;


/// <summary>
/// Препятствие
/// </summary>
public partial class ObstacleEntity : BattleEntity
{
    public ObstacleEntity(Tile tile, ObstacleCode obstacleCode) : base(tile, null, Guid.NewGuid().ToString(), 0, 0, Int32.MaxValue, 0, 0)
    {
        ObstacleCode = obstacleCode;
        switch (ObstacleCode)
        {
            case ObstacleCode.Wall:
                IsImpassable = true;
                break;
            case ObstacleCode.Rock:
                IsImpassable = true;
                break;
            case ObstacleCode.Totem:
                IsImpassable = true;
                HasLifetime = false;
                break;
            case ObstacleCode.Poison:
                IsImpassable = false;
                ImposedEffect = new PoisonEntityEffect();
                HasLifetime = true;
                TurnCount = 3;
                break;
            case ObstacleCode.Fire: 
                IsImpassable = false;
                ImposedEffect = new FireEntityEffect();
                HasLifetime = true;
                TurnCount = 3;
                break;
            default: throw new NotImplementedException($"Не существует препятствия с кодом {obstacleCode}");
        }
    }
    
    public ObstacleCode ObstacleCode { get; set; }
    
    /// <summary>
    /// Непроходимое ли препятствие? 
    /// </summary>
    public bool IsImpassable { get; set; }
    
    public bool HasLifetime { get; set; }
    /// <summary>
    /// Параметр определяющий сколько ещё в ходах будет жить препятствие
    /// </summary>
    public int TurnCount { get; set; }
    
    public Effect ImposedEffect { get; set; }
    
}