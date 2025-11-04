using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight;

public partial class PlayerEntity : BattleEntity
{
    public PlayerEntity(Tile tile, CharacterData characterData) : base(tile, new Weapon(characterData.Weapon.AttackShapeID), characterData.ID, characterData.Speed, characterData.Health, characterData.Damage)
    {
    }
}