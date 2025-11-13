using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;


public class BattleFrenzyActiveSkill : ActiveSkill
{
	public BattleFrenzyActiveSkill(EEffectType InEffectType, PlayerEntity playerEntity) : base(InEffectType, playerEntity)
	{
		// CreateEffect(InEffectType);
	}

	public override List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map)
    {
        return new List<Vector2I>();
    }

	// public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}