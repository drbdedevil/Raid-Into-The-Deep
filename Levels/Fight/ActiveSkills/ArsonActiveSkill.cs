using System.Collections.Generic;
using Godot;
using System;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;


public class ArsonActiveSkill : ActiveSkill
{
	public ArsonActiveSkill(EEffectType InEffectType, PlayerEntity playerEntity, ESkillType SkillType) : base(InEffectType, playerEntity)
	{
		// CreateEffect(InEffectType);
		skillType = SkillType;
	}

	public override List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map)
	{
		var direction = CalculateDirection(startPosition, playerTargetPosition);

		switch (direction)
		{
			case AttackDirection.Down: return [startPosition + Vector2I.Down + Vector2I.Left, startPosition + Vector2I.Down, startPosition + Vector2I.Down + Vector2I.Right];
			case AttackDirection.Up: return [startPosition + Vector2I.Up + Vector2I.Left, startPosition + Vector2I.Up, startPosition + Vector2I.Up + Vector2I.Right];
			case AttackDirection.Left: return [startPosition + Vector2I.Left + Vector2I.Up,startPosition +   Vector2I.Left,startPosition +  Vector2I.Left + Vector2I.Down];
			case AttackDirection.Right: return [startPosition + Vector2I.Right + Vector2I.Up,  startPosition + Vector2I.Right, startPosition + Vector2I.Right + Vector2I.Down];
			default: throw new ArgumentOutOfRangeException();
		}
	}

	public override void PlaySkillSound()
	{
		SoundManager.Instance.PlaySoundOnce("res://Sound/Skills/DSGNImpt_EXPLOSION-Fire Hit_HY_PC-001.wav", 0.2f);
	}

	// public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}
