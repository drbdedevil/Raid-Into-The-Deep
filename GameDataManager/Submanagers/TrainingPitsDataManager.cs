using Godot;
using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

public partial class TrainingPitsDataManager : Node
{
    private GameDataManager gameDataManager;
    [Signal]
    public delegate void OnTrainingPitsLevelUpdateEventHandler();
    [Signal]
    public delegate void OnCharactersForHiringListUpdateEventHandler();

    public TrainingPitsDataManager(GameDataManager InGameDataManager)
    {
        gameDataManager = InGameDataManager;
    }

    public bool TryUpdateLevel()
    {
        bool IsCanUpdateUpper = gameDataManager.currentData.trainingPitsData.Level < gameDataManager.trainingPitsDatabase.Levels.Count;
        if (IsCanUpdateUpper)
        {
            gameDataManager.currentData.trainingPitsData.Level += 1;
            EmitSignal(SignalName.OnTrainingPitsLevelUpdate);

            return true;
        }

        return false;
    }

    public bool TryAddCharacterForHiring(CharacterData InCharacterData)
    {
        var charactersForHiring = gameDataManager.currentData.trainingPitsData.CharactersForHiring;
        TrainingPitsLevelData currentTrainingPitsLevelData = gameDataManager.trainingPitsDatabase.Levels[gameDataManager.currentData.trainingPitsData.Level - 1];
        if (currentTrainingPitsLevelData.Capacity <= charactersForHiring.Count)
        {
            return false;
        }
    
        if (charactersForHiring.Any(character => character.ID == InCharacterData.ID))
        {
            return false;
        }

        charactersForHiring.Add(InCharacterData);
        EmitSignal(SignalName.OnCharactersForHiringListUpdate);
        return true;
    }
    public bool TryDeleteCharacterForHiring(string CharacterID)
    {
        var charactersForHiring = gameDataManager.currentData.trainingPitsData.CharactersForHiring;

        var existingCharacter = GameDataManager.Instance.currentData.trainingPitsData.CharactersForHiring.FirstOrDefault(character => character.ID == CharacterID);
        if (existingCharacter != null)
        {
            charactersForHiring.Remove(existingCharacter);

            EmitSignal(SignalName.OnCharactersForHiringListUpdate);
            return true;
        }

        return false;
    }

    public void GenerateCharactersForHiring(int charactersCountToGenerate)
    {
        int currentLevel = gameDataManager.currentData.trainingPitsData.Level;
        TrainingPitsLevelData currentTrainingPitsLevelData = gameDataManager.trainingPitsDatabase.Levels[currentLevel - 1];

        int currentCharactersCount = gameDataManager.currentData.trainingPitsData.CharactersForHiring.Count;
        if (currentCharactersCount >= currentTrainingPitsLevelData.Capacity)
        {
            return;
        }

        int estimatedNumberOfCharacters = currentCharactersCount + charactersCountToGenerate;
        if (estimatedNumberOfCharacters > currentTrainingPitsLevelData.Capacity)
        {
            charactersCountToGenerate = currentTrainingPitsLevelData.Capacity - currentCharactersCount;
        }

        for (int i = 0; i < charactersCountToGenerate; ++i)
        {
            int LevelToCreate = GD.RandRange(1, currentLevel);

            CharacterData characterData = new CharacterData();
            characterData.ID = Guid.NewGuid().ToString();
            characterData.Name = GetUniqRandomName();
            characterData.PortraitID = GetRandomPortraitID();
            characterData.Damage = gameDataManager.baseStatsDatabase.Damage;
            characterData.DamageByEffect = gameDataManager.baseStatsDatabase.DamageByEffect;
            characterData.Health = gameDataManager.baseStatsDatabase.Health;// = 8;
            characterData.Heal = gameDataManager.baseStatsDatabase.Heal;
            characterData.Speed = gameDataManager.baseStatsDatabase.Speed;
            characterData.Level = LevelToCreate;

            int pointsForActive = GD.RandRange(0, LevelToCreate > 1 ? 1 : 0);
            int pointsForPassive = LevelToCreate - 1 - pointsForActive;
            // int pointsForPassive = GD.RandRange(0, LevelToCreate - 1);
            // int pointsForActive = LevelToCreate - 1 - pointsForPassive;

            characterData.PassiveSkillLevels = GetRandomPassiveSkills(pointsForPassive);
            characterData.ActiveSkills = GetRandomActiveSkills(pointsForActive);
            if (pointsForActive == 1)
            {
                characterData.ChoosenSkills = characterData.ActiveSkills.First();
            }

            if (pointsForPassive > 0)
            {
                SetProgression(characterData.PassiveSkillLevels, characterData);
            }

            if (gameDataManager.IsShouldGenerateWeaponOnStartToEveryCreatedCharacter)
            {
                characterData.Weapon = gameDataManager.forgeDataManager.GenerateRandomWeapon();
            }

            TryAddCharacterForHiring(characterData);
        }
    }
    private string GetUniqRandomName()
    {
        using FileAccess file = FileAccess.Open("res://GameDataManager/GameData/names.json", FileAccess.ModeFlags.Read);
        if (file == null)
        {
            return "No name";
        }
        string jsonText = file.GetAsText();

        using JsonDocument doc = JsonDocument.Parse(jsonText);
        JsonElement root = doc.RootElement;

        JsonElement namesArray = root.GetProperty("names");
        List<string> names = new List<string>();
        for (int i = 0; i < namesArray.GetArrayLength(); ++i)
        {
            names.Add(namesArray[i].GetString());
        }

        List<string> existingNames = GetExistingNames();

        bool nameDefined = false;
        string randomName = "No name";
        while (!nameDefined)
        {
            int randomIndex = GD.RandRange(0, namesArray.GetArrayLength() - 1);
            if (!existingNames.Contains(names[randomIndex]))
            {
                nameDefined = true;
                randomName = names[randomIndex];
            }
        }

        return randomName;
    }
    private int GetRandomPortraitID()
    {
        return GD.RandRange(0, gameDataManager.charactersSpritesDatabase.CharactersSpritesArray.Count - 1);
    }
    private Dictionary<string, int> GetRandomPassiveSkills(int Amount)
    {
        Dictionary<string, int> passiveSkills = new Dictionary<string, int>(Amount);
        foreach (SkillRow skillRow in gameDataManager.passiveSkillsDatabase.skillsRows)
        {
            passiveSkills[skillRow.skillName] = 0;
        }

        if (Amount <= 0)
        {
            return passiveSkills;
        }

        int SkillsForUpgrade = Amount;
        while (SkillsForUpgrade > 0)
        {
            int randomIndex = GD.RandRange(0, gameDataManager.passiveSkillsDatabase.skillsRows.Count - 1);
            SkillRow randomSkillRow = gameDataManager.passiveSkillsDatabase.skillsRows[randomIndex];

            PassiveSkillProgressionRow existingPassiveSkillType = gameDataManager.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(progression => progression.skillType == randomSkillRow.skillType);
            int maxToAdd = Math.Min(existingPassiveSkillType.increments.Count - passiveSkills[randomSkillRow.skillName], SkillsForUpgrade);
            if (maxToAdd > 0)
            {
                int pointsToAdd = GD.RandRange(1, maxToAdd);
                passiveSkills[randomSkillRow.skillName] += pointsToAdd;
                SkillsForUpgrade -= pointsToAdd;
            }
        }

        return passiveSkills;
    }
    private HashSet<string> GetRandomActiveSkills(int Amount)
    {
        HashSet<string> activeSkills = new HashSet<string>(Amount);
        if (Amount <= 0)
        {
            return activeSkills;
        }

        int skillsToUpgrade = Amount;
        while (skillsToUpgrade > 0)
        {
            int randomActiveSkills = GD.RandRange(0, gameDataManager.activeSkillsDatabase.skillsRows.Count - 1);
            if (!activeSkills.Contains(gameDataManager.activeSkillsDatabase.skillsRows[randomActiveSkills].skillName))
            {
                activeSkills.Add(gameDataManager.activeSkillsDatabase.skillsRows[randomActiveSkills].skillName);
                --skillsToUpgrade;
            }
        }

        return activeSkills;
    }
    public void SetProgression(Dictionary<string, int> PassiveSkills, CharacterData characterData)
    {
        foreach (KeyValuePair<string, int> skill in PassiveSkills)
        {
            string skillName = skill.Key;
            int skillLevel = skill.Value;

            SkillRow skillRow = gameDataManager.passiveSkillsDatabase.skillsRows.FirstOrDefault(skill => skill.skillName == skillName);
            PassiveSkillProgressionRow existingPassiveSkillType = gameDataManager.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(skill => skill.skillType == skillRow.skillType);

            for (int i = 0; i < skillLevel; ++i)
            {
                switch (existingPassiveSkillType.skillType)
                {
                    case ESkillType.Health:
                        characterData.Health += existingPassiveSkillType.increments[i];
                        break;
                    case ESkillType.Speed:
                        characterData.Speed += existingPassiveSkillType.increments[i];
                        break;
                    case ESkillType.Damage:
                        characterData.Damage += existingPassiveSkillType.increments[i];
                        break;
                    case ESkillType.DamageByEffect:
                        characterData.DamageByEffect += existingPassiveSkillType.increments[i];
                        break;
                    case ESkillType.Heal:
                        characterData.Heal += existingPassiveSkillType.increments[i];
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private List<string> GetExistingNames()
    {
        List<string> existingNames = new List<string>();
        for (int i = 0; i < gameDataManager.currentData.trainingPitsData.CharactersForHiring.Count; ++i)
        {
            existingNames.Add(gameDataManager.currentData.trainingPitsData.CharactersForHiring[i].Name);
        }
        for (int i = 0; i < gameDataManager.currentData.livingSpaceData.UsedCharacters.Count; ++i)
        {
            existingNames.Add(gameDataManager.currentData.livingSpaceData.UsedCharacters[i].Name);
        }
        for (int i = 0; i < gameDataManager.currentData.livingSpaceData.ReservedCharacters.Count; ++i)
        {
            existingNames.Add(gameDataManager.currentData.livingSpaceData.ReservedCharacters[i].Name);
        }

        return existingNames;
    }

    public bool IsCharacterInHiringList(string CharacterID)
    {
        var charactersForHiring = gameDataManager.currentData.trainingPitsData.CharactersForHiring;
        var existingCharacter = charactersForHiring.FirstOrDefault(character => character.ID == CharacterID);
        if (existingCharacter != null)
        {
            return true;
        }
        return false;
    }
}
