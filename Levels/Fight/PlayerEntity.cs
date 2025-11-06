using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using System.Collections.Generic;

namespace RaidIntoTheDeep.Levels.Fight;

public partial class PlayerEntity : BattleEntity
{
    public int Level { get; set; } = 1;
    public int PortraitID { get; set; } = 0;
    public string CharacterName { get; set; } = "NONE";
    public Dictionary<string, int> PassiveSkillLevels { get; set; } = new();
    public PlayerEntity(Tile tile, CharacterData characterData) : base(tile, new Weapon(characterData.Weapon.AttackShapeID), characterData.ID, characterData.Speed, characterData.Health, characterData.Damage)
    {
        Level = characterData.Level;
        PortraitID = characterData.PortraitID;
        CharacterName = characterData.Name;
        PassiveSkillLevels = characterData.PassiveSkillLevels;
    }
}