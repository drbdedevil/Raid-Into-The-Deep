using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight;

public partial class EnemyEntity : BattleEntity
{
    public EnemyEntity(Tile tile, string id, int speed, int health, int damage, GameEnemyCode enemyId) : base(tile, id, speed, health, damage)
    {
        EnemyId = enemyId;
    }

    public GameEnemyCode EnemyId { get; set; }
}