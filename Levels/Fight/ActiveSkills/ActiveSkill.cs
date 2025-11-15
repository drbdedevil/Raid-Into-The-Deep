using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;


public abstract class ActiveSkill
{
	public ActiveSkill(EEffectType InEffectType, PlayerEntity playerEntity)
	{
		// CreateEffect(InEffectType);
		effectType = InEffectType;
		playerEntityOwner = playerEntity;
	}

	// public Effect effect { get; private set; }
	public EEffectType effectType { get; private set; }
	public bool IsHasEffect()
	{
		return effectType != EEffectType.NONE;
	}
	public ESkillType skillType = ESkillType.NONE;

	public PlayerEntity playerEntityOwner;
	
	protected AttackDirection CalculateDirection( Vector2 startPosition, Vector2 targetPosition)
	{
		var dx = targetPosition.X - startPosition.X;
		var dy = targetPosition.Y - startPosition.Y;
		
		if (Mathf.Abs(dx) > Mathf.Abs(dy))
		{
			if (dx > 0)
				return AttackDirection.Right;
			return AttackDirection.Left;
		}
		if (dy > 0)
			return AttackDirection.Down;
		return AttackDirection.Up;
	}
	public abstract List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map);

	/*public void CreateEffect(EEffectType InEffectType)
	{
		switch (InEffectType)
		{
			case EEffectType.BattleFrenzy:
				effect = new BattleFrenzyEntityEffect(3);
				return;
			case EEffectType.SevereWound:
				effect = new SevereWoundEntityEffect(3);
				return;
			case EEffectType.Defense:
				effect = new DefenseEntityEffect(3);
				return;
			case EEffectType.BloodMark:
				effect = new BloodMarkEntityEffect(3);
				return;
			case EEffectType.ReserveDamage:
				effect = new ReserveDamageEntityEffect(3);
				return;
			case EEffectType.Fire:
				effect = new FireObstacleEffect(null, 3);
				return;
			case EEffectType.Poison:
				// создать tile effect
				return;
			default:
				break;
		}
		effect = null;
	}*/
	
	public Effect GenerateEffect()
	{
		switch (effectType)
		{
			case EEffectType.BattleFrenzy:
				return new BattleFrenzyEntityEffect(3);
			case EEffectType.SevereWound:
				return new SevereWoundEntityEffect(3);
			case EEffectType.Defense:
				return new DefenseEntityEffect(3);
			case EEffectType.BloodMark:
				return new BloodMarkEntityEffect(3);
			case EEffectType.ReserveDamage:
				return new ReserveDamageEntityEffect(3);
			case EEffectType.Fire:
				return new FireObstacleEffect(null, 3);
			case EEffectType.Poison:
				return new PoisonObstacleEffect(null, 3);
			case EEffectType.ObstacleHeal:
				return new HealObstacleEffect(null, 3);
			case EEffectType.Wall:
				return new WallObstacleEffect(null, 3);
			default:
				break;
		}
		return null;
	}

	public abstract void PlaySkillSound();

	// public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}

