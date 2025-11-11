using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using System.Collections.Generic;
using RaidIntoTheDeep.Levels.Fight.Weapons;
using System.Linq;

namespace RaidIntoTheDeep.Levels.Fight;

public partial class PlayerEntity : BattleEntity
{
    public int Level { get; set; } = 1;
    public int PortraitID { get; set; } = 0;
    public string CharacterName { get; set; } = "NONE";
    public Dictionary<string, int> PassiveSkillLevels { get; set; } = new();

    public PlayerEntity(Tile tile, CharacterData characterData) : base(tile,
        WeaponFactory.CreateWeaponByAttackShapeType((AttackShapeType)characterData.Weapon.AttackShapeID), characterData.ID, 
        characterData.Speed, characterData.Health, characterData.Damage)
    {
        Level = characterData.Level;
        PortraitID = characterData.PortraitID;
        CharacterName = characterData.Name;
        PassiveSkillLevels = characterData.PassiveSkillLevels;
    }

    public override void ApplyDamage(int damage)
    {
        CharacterData characterData = GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.FirstOrDefault(character => character.ID == Id);
        if (characterData != null)
        {
            characterData.Health -= damage;
            Health = characterData.Health;

            if (IsDead())
            {
                GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.Remove(characterData);
            }
        }
    }
}