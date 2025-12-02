using Godot;
using RaidIntoTheDeep.Levels.Fight.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

public partial class CommandBlockDataManager : Node
{
	private GameDataManager gameDataManager;

	public CommandBlockDataManager(GameDataManager InGameDataManager)
	{
		gameDataManager = InGameDataManager;
	}

	public void ApplyRewardForVictory()
    {
		gameDataManager.storageDataManager.AdjustCrystals(GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle);
		gameDataManager.storageDataManager.AdjustChitinFragments(GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle);

		List<CharacterData> team = GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters;
		if (team.Count == 0)
        {
            return;
        }
		int experienceForAll = GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle / team.Count;

		switch (gameDataManager.currentGameMode)
        {
            case EGameMode.Usual:
                
                break;
            case EGameMode.Simple:
                experienceForAll = (int)(experienceForAll * 1.25f);
                break;
            default:
                break;
        }

		foreach (CharacterData characterData in team)
        {
            characterData.ExperiencePoints += experienceForAll;
			if (characterData.ExperiencePoints >= 1000f)
            {
                characterData.ExperiencePoints = 1000;
            }
			TryPromotion(characterData);
        }
    }
	public bool CheckPromotion(CharacterData characterData)
    {
        int currentLevel = characterData.Level;
		int currentExperience = characterData.ExperiencePoints;
		var experienceDatas = GameDataManager.Instance.charactersExperienceLevelsDatabase.Levels;
		if (experienceDatas.Count > currentLevel 
			&& currentExperience >= experienceDatas[currentLevel].NeedableExperinceForNextLevel
			&& currentLevel + characterData.SkillPoints < experienceDatas.Count)
        {
			// characterData.ExperiencePoints -= experienceDatas[currentLevel].NeedableExperinceForNextLevel;
			characterData.SkillPoints += 1;
            return true;
        }

		return false;
    }
	public bool TryPromotion(CharacterData characterData)
    {
        int currentLevel = characterData.Level;
		int currentExperience = characterData.ExperiencePoints;
		var experienceDatas = GameDataManager.Instance.charactersExperienceLevelsDatabase.Levels;
		if (experienceDatas.Count > currentLevel && currentExperience >= experienceDatas[currentLevel].NeedableExperinceForNextLevel)
        {
			return CheckPromotion(characterData);
        }
		return false;
    }
	public void Promotion(CharacterData characterData)
    {
        int currentLevel = characterData.Level;
		int currentExperience = characterData.ExperiencePoints;
		var experienceDatas = GameDataManager.Instance.charactersExperienceLevelsDatabase.Levels;
		if (experienceDatas.Count > currentLevel && currentExperience >= experienceDatas[currentLevel].NeedableExperinceForNextLevel)
        {
			characterData.ExperiencePoints -= experienceDatas[currentLevel].NeedableExperinceForNextLevel;
        }
    }

	public void RestoreHealthToTheReservists()
    {
		PassiveSkillProgressionRow existingPassiveSkillType = GameDataManager.Instance.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(progression => progression.skillType == ESkillType.Health);
        foreach (CharacterData characterData in gameDataManager.currentData.livingSpaceData.ReservedCharacters.ToList())
        {
            int currentHealth = characterData.Health;

			Dictionary<string, int> passiveSkillLevels = characterData.PassiveSkillLevels;
			int healthSkillLevel = passiveSkillLevels["Здоровье"];
			int maxHealthToSet = GameDataManager.Instance.baseStatsDatabase.Health;

			for (int i = 0; i < healthSkillLevel; ++i)
			{
				maxHealthToSet += existingPassiveSkillType.increments[i];
			}
			int heal = maxHealthToSet / 4;

			currentHealth += heal;
			if (currentHealth > maxHealthToSet)
            {
                currentHealth = maxHealthToSet;
            }
			characterData.Health = currentHealth;
        }
    }
	public void ApplyPunishmentForEscape()
    {
        // int CrystalsForSeizure = gameDataManager.currentData.storageData.Crystals / 4;
		// int ChitinsForSeizure = gameDataManager.currentData.storageData.ChitinFragments / 4;
		// gameDataManager.storageDataManager.AdjustCrystals(-CrystalsForSeizure);
		// gameDataManager.storageDataManager.AdjustChitinFragments(-ChitinsForSeizure);

		NotificationSystem.Instance.ShowMessage("Итоги твоего трусливого, жалкого, никчёмного побега, сударь:", EMessageType.Alert);
		foreach (CharacterData characterData in gameDataManager.currentData.livingSpaceData.UsedCharacters.ToList())
        {
            int rand = GD.RandRange(0, 1);

			if (rand == 0)
            {
                characterData.Weapon = new WeaponData();
				NotificationSystem.Instance.ShowMessage("Персонаж " + characterData.Name + " потерял своё оружие.", EMessageType.Alert);
            }
			else if (rand == 1)
            {
				PassiveSkillProgressionRow existingPassiveSkillType = GameDataManager.Instance.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(progression => progression.skillType == ESkillType.Health);
				int currentHealth = characterData.Health;

				Dictionary<string, int> passiveSkillLevels = characterData.PassiveSkillLevels;
				int healthSkillLevel = passiveSkillLevels["Здоровье"];
				int maxHealthToSet = GameDataManager.Instance.baseStatsDatabase.Health;

				for (int i = 0; i < healthSkillLevel; ++i)
				{
					maxHealthToSet += existingPassiveSkillType.increments[i];
				}
				int damage = maxHealthToSet / 4;

				characterData.Health -= damage;
				if (characterData.Health <= 0)
                {
                    gameDataManager.livingSpaceDataManager.TryDeleteCharacterFromUsed(characterData.ID);
                }
				string resultMessage = characterData.Health <= 0 ? ", отчего был убит." : ".";
                NotificationSystem.Instance.ShowMessage("Персонаж " + characterData.Name + " потерял свои очки здоровья" + resultMessage, EMessageType.Alert);
            }
        }
    }
}
