using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight;

public partial class PlayerEntity : BattleEntity
{
    public PlayerEntity(Tile tile, CharacterData characterData) : base(tile, characterData.ID, characterData.Speed, characterData.Health, characterData.Damage)
    {
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is BattleEntity battleEntity)
        {
            return Id == battleEntity.Id;
        }
        return false;
    }
}