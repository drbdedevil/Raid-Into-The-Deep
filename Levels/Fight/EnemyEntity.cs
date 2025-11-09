using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using RaidIntoTheDeep.Levels.Fight.Weapons;

namespace RaidIntoTheDeep.Levels.Fight;

public partial class EnemyEntity : BattleEntity
{
    // надо будет написать метод для спавна рандомного оружия врагам
    public EnemyEntity(Tile tile, string id, int speed, int health, int damage, GameEnemyCode enemyId) : base(tile, WeaponFactory.CreateWeaponByAttackShapeType(AttackShapeType.ScatterShot, new WeaponData()), id, speed, health, damage)
    {
        EnemyId = enemyId;
    }

    public GameEnemyCode EnemyId { get; set; }
}