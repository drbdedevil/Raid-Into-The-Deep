using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;


public class BattleFrenzyActiveSkill : ActiveSkill
{
	public BattleFrenzyActiveSkill(EEffectType InEffectType, PlayerEntity playerEntity, ESkillType SkillType) : base(InEffectType, playerEntity)
	{
		// CreateEffect(InEffectType);
		skillType = SkillType;
	}

	public override List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map)
    {
        return new List<Vector2I>();
    }

	public override void PlaySkillSound()
    {
        SoundManager.Instance.PlaySoundOnce("res://Sound/Skills/DSGNImpt_EXPLOSION-Forced Interruption_HY_PC-001.wav", 0.2f);
    }

	// public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}