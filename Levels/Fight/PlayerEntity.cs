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

    public ActiveSkill activeSkill { get; private set; }

    public PlayerEntity(Tile tile, CharacterData characterData) : base(tile,
        WeaponFactory.CreateWeaponForPlayerEntity((AttackShapeType)characterData.Weapon.AttackShapeID, characterData.Weapon), characterData.ID,
        characterData.Speed, characterData.Health, characterData.Heal, characterData.Damage, characterData.DamageByEffect)
    {
        Level = characterData.Level;
        PortraitID = characterData.PortraitID;
        CharacterName = characterData.Name;
        PassiveSkillLevels = characterData.PassiveSkillLevels;

        PassiveSkillProgressionRow existingPassiveSkillType = GameDataManager.Instance.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(progression => progression.skillType == ESkillType.Health);
		if (existingPassiveSkillType != null)
		{
			Dictionary<string, int> passiveSkillLevels = characterData.PassiveSkillLevels;
			int healthSkillLevel = passiveSkillLevels["Здоровье"];
			int maxHealthToSet = GameDataManager.Instance.baseStatsDatabase.Health;

			for (int i = 0; i < healthSkillLevel; ++i)
			{
				maxHealthToSet += existingPassiveSkillType.increments[i];
			}

			MaxHealth = maxHealthToSet;
		}

        activeSkill = ActiveSkillFactory.CreateActiveSkillByName(characterData.ChoosenSkills);
    }

    public override void ApplyDamage(BattleEntity instigator, int damage)
    {
        base.ApplyDamage(instigator, damage);

        CharacterData characterData = GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.FirstOrDefault(character => character.ID == Id);
        if (characterData != null)
        {
            characterData.Health -= damage;

            if (IsDead())
            {
                GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.Remove(characterData);
            }
        }
    }
    public override void ApplyHeal(BattleEntity instigator, int heal)
    {
        base.ApplyHeal(instigator, heal);

        CharacterData characterData = GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.FirstOrDefault(character => character.ID == Id);
        if (characterData != null)
        {
            int result = characterData.Health + heal;
            if (result > MaxHealth)
            {
                characterData.Health = MaxHealth;
                return;
            }
            characterData.Health = result;
        }
    }
}