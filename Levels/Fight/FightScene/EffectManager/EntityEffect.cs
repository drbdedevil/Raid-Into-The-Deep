using Godot;
using RaidIntoTheDeep.Levels.Fight;
using System;

public partial class EntityEffect : Effect
{
    public BattleEntity entityHolder;
    public EntityEffect(EEffectType InEffectType, int InDuration = 0) : base(InEffectType, InDuration)
    {
        
    }
    public override void ApplyForHolder()
    {
        GD.Print("\nHP " + entityHolder.Id + " =" + entityHolder.Health  + " до принятия эффекта");
        switch (effectType)
        {
            case EEffectType.Poison:
                GD.Print(entityHolder.Id + " принял эффект яд");
                entityHolder.Health = entityHolder.Health - 5;
                break;
            case EEffectType.Stun:
                GD.Print(entityHolder.Id + " принял эффект оглушение");

                break;
            case EEffectType.Freezing:
                GD.Print(entityHolder.Id + " принял эффект мороз");

                break;
            case EEffectType.Weakening:
                GD.Print(entityHolder.Id + " принял эффект ослабление");

                break;
            // case EEffectType.ResistanceToStun:
            // GD.Print( entityHolder.Id + " принял эффект сопротивление к оглушению");
            // break;
            // case EEffectType.Pushing:
            // GD.Print( entityHolder.Id + " принял эффект толкание");
            // break;
            case EEffectType.Sleep:
                GD.Print(entityHolder.Id + " принял эффект сон");

                break;
            case EEffectType.Fire:
                GD.Print(entityHolder.Id + " принял эффект огонь");
                entityHolder.Health = entityHolder.Health - 5;
                break;
            default:
                break;
        }
        
        GD.Print("HP " + entityHolder.Id + " =" + entityHolder.Health  + " после принятия эффекта");
    }
}
