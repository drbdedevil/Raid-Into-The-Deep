using Godot;
using RaidIntoTheDeep.Levels.Fight;
using System;
using System.Linq;

public partial class EntityEffect : Effect
{
	public BattleEntity entityHolder;
	public EntityEffect(EEffectType InEffectType, int InDuration = 0) : base(InEffectType, InDuration)
	{
		
	}
	public override void ApplyForHolder()
	{
		GD.Print("\n");
		// GD.Print("\nHP " + entityHolder.Id + " =" + entityHolder.Health  + " до принятия эффекта");
		switch (effectType)
		{
			case EEffectType.Poison:
				GD.Print(entityHolder.Id + " принял эффект яд");
				int randPoisonDamage = GD.RandRange(2, 5);
				entityHolder.Health = entityHolder.Health - randPoisonDamage;
				break;
			case EEffectType.Stun:
				if (!entityHolder.appliedEffects.Any(ae => ae.EffectType == EEffectType.ResistanceToStun)
				 && entityHolder.Weapon.effect.EffectType != EEffectType.ResistanceToStun)
				{
					GD.Print(entityHolder.Id + " принял эффект оглушение");
				}
				else
				{
					GD.Print(entityHolder.Id + " не принял эффект оглушение, так как имеет к нему сопротивление");
				}
				break;
			case EEffectType.Freezing:
				GD.Print(entityHolder.Id + " принял эффект мороз");

				break;
			case EEffectType.Weakening:
				GD.Print(entityHolder.Id + " принял эффект ослабление");

				break;
			case EEffectType.ResistanceToStun:
				GD.Print( entityHolder.Id + " принял эффект сопротивление к оглушению");
				break;
			// case EEffectType.Pushing:
			// GD.Print( entityHolder.Id + " принял эффект толкание");
			// break;
			case EEffectType.Sleep:
				GD.Print(entityHolder.Id + " принял эффект сон");
				break;
			case EEffectType.Fire:
				GD.Print(entityHolder.Id + " принял эффект огонь");
				int randFireDamage = GD.RandRange(1, 4);
				entityHolder.Health = entityHolder.Health - randFireDamage;
				break;
			default:
				break;
		}

		if (--duration == 0)
		{
			bIsShouldRemoveFromEffectHolder = true;
		}
		GD.Print("Осталось ходов для эффекта: " + duration);
		// GD.Print("HP " + entityHolder.Id + " =" + entityHolder.Health  + " после принятия эффекта");
	}
}
