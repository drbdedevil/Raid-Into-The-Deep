public abstract class Effect
{
    public Effect(EEffectType InEffectType, int InDuration = 0)
    {
        effectType = InEffectType;
        duration = InDuration;
    }
    protected EEffectType effectType = EEffectType.Poison;
    public EEffectType EffectType => effectType;
    protected int duration = 0;
    public abstract void ApplyForHolder();
    public bool bIsShouldRemoveFromEffectHolder = false;
}