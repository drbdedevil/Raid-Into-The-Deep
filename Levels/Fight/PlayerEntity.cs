namespace RaidIntoTheDeep.Levels.Fight;

public partial class PlayerEntity : BattleEntity
{
    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is BattleEntity battleEntity)
        {
            return ID == battleEntity.ID;
        }
        return false;
    }
}