using Godot;
using RaidIntoTheDeep.Levels.Fight;
using System;
using System.Linq;

public partial class EntityEffect : Effect
{
	public BattleEntity entityHolder;
}

public class PoisonEntityEffect : EntityEffect
{
	public PoisonEntityEffect(int duration = 3)
	{
		EffectType = EEffectType.Poison;
		TargetType = EEffectTarget.Enemy;
		Duration = duration;
		IsTemporary = true;
		IsPending = true;
	}

	public override void OnApply()
	{
		entityHolder.Health = entityHolder.Health - 5;
		GD.Print("Применился яд для " + entityHolder.Id);
	}
}

public class FireEntityEffect : EntityEffect
{
	public FireEntityEffect(int duration = 3)
	{
		EffectType = EEffectType.Fire;
		TargetType = EEffectTarget.Enemy;
		Duration = duration;
		IsTemporary = true;
		IsPending = true;
	}
	
	public override void OnApply()
	{
		entityHolder.Health = entityHolder.Health - 8;
		GD.Print("Применился огонь для " + entityHolder.Id);
	}
}

public class StunEntityEffect : EntityEffect
{
	public StunEntityEffect(int duration = 1)
	{
		EffectType = EEffectType.Stun;
		TargetType = EEffectTarget.Enemy;
		Duration = duration;
		IsTemporary = true;
		IsPending = false;
	}

	public override void OnApply()
	{
		entityHolder.CanAct = false;
		GD.Print("Применилось оглушение для " + entityHolder.Id);
	}
	public override void OnRemove()
	{
		entityHolder.CanAct = true;
		entityHolder.AddEffect(new ResistanceToStunEntityEffect(2));
	}
}

public class ResistanceToStunEntityEffect : EntityEffect
{
	public ResistanceToStunEntityEffect(int duration = 0)
	{
		EffectType = EEffectType.ResistanceToStun;
		TargetType = EEffectTarget.Self;
		Duration = duration;
		IsTemporary = duration > 0;
		IsPending = false;
	}
	public override void OnApply()
	{
		GD.Print("Применилось сопротивление оглушению для " + entityHolder.Id);
	}
}

public class PushingEntityEffect : EntityEffect
{
	public PushingEntityEffect(int duration = 0)
	{
		EffectType = EEffectType.Pushing;
		TargetType = EEffectTarget.Self;
		Duration = duration;
		IsTemporary = duration > 0;
		IsPending = false;
	}
	public override void OnApply()
	{
		GD.Print("Применилось толкание для " + entityHolder.Id);
	}
}

public class SleepEntityEffect : EntityEffect
{
	public SleepEntityEffect(int duration = 3)
	{
		EffectType = EEffectType.Sleep;
		TargetType = EEffectTarget.Enemy;
		Duration = duration;
		IsTemporary = true;
		IsPending = false;
	}
	public override void OnApply()
	{
		GD.Print("Применилось сон для " + entityHolder.Id);
		entityHolder.CanAct = false;
	}
	public override void OnRemove()
	{
		entityHolder.CanAct = true;
	}
}

public class FreezingEntityEffect : EntityEffect
{
	public FreezingEntityEffect(int duration = 3)
	{
		EffectType = EEffectType.Freezing;
		TargetType = EEffectTarget.Enemy;
		Duration = duration;
		IsTemporary = true;
		IsPending = false;
	}
	public override void OnApply()
	{
		GD.Print("Применился мороз для " + entityHolder.Id);
	}
}

public class WeakeningEntityEffect : EntityEffect
{
	public WeakeningEntityEffect(int duration = 3)
	{
		EffectType = EEffectType.Weakening;
		TargetType = EEffectTarget.Enemy;
		Duration = duration;
		IsTemporary = true;
		IsPending = false;
	}
	public override void OnApply()
	{
		GD.Print("Применилось ослабление для " + entityHolder.Id);
	}
}
