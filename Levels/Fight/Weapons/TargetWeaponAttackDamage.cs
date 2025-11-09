namespace RaidIntoTheDeep.Levels.Fight.Weapons;

/// <summary>
/// Содержит в себе информацию о том, сколько урона нужно нанести по врагу
/// </summary>
public class TargetWeaponAttackDamage
{
    public TargetWeaponAttackDamage(BattleEntity entityToAttack, int damage)
    {
        EntityToAttack = entityToAttack;
        Damage = damage;
    }

    public BattleEntity EntityToAttack { get; set; }
    public int Damage { get; set; }
}