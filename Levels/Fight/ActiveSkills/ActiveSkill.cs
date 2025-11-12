using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;


public abstract class ActiveSkill
{
	public ActiveSkill(EEffectType InEffectType)
	{
		CreateEffect(InEffectType);
	}

	public Effect effect { get; private set; }
	public bool IsHasEffect()
    {
		return effect != null;
    }
	
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

	public void CreateEffect(EEffectType InEffectType)
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
				// создать tile effect
				return;
			case EEffectType.Poison:
				// создать tile effect
				return;
			default:
				break;
        }
		effect = null;
    }

	// public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}

