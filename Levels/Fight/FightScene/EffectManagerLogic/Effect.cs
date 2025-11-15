namespace RaidIntoTheDeep.Levels.Fight.FightScene.EffectManagerLogic;

public enum EEffectTarget
{
    Self,
    Enemy,
    Obstacle
}

public abstract class Effect
{
    public EEffectType EffectType { get; protected set; }
    public EEffectTarget TargetType { get; protected set; } 

    public int Duration { get; protected set; }
    public bool IsExpired => Duration == 0 && IsTemporary;

    public bool IsTemporary { get; protected set; } 
    public bool IsPending { get; set; }

    public virtual void OnApply() {}
    public virtual void OnRemove() { }
    public virtual void OnTurnEnd() 
    {
        if (IsTemporary && Duration > 0)
        {
            Duration--;
        }
    }
}