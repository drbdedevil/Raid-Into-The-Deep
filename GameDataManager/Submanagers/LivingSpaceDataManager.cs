using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LivingSpaceDataManager : Node
{
    private GameDataManager gameDataManager;
    [Signal]
    public delegate void OnLivingSpaceLevelUpdateEventHandler();
    [Signal]
    public delegate void OnUsedCharactersListUpdateEventHandler();
    [Signal]
    public delegate void OnReservedCharactersListUpdateEventHandler();

    public LivingSpaceDataManager(GameDataManager InGameDataManager)
    {
        gameDataManager = InGameDataManager;
    }

    public bool TryUpdateLevel()
    {
        bool IsCanUpdateUpper = gameDataManager.currentData.livingSpaceData.Level < gameDataManager.livingSpaceDatabase.Levels.Count;
        if (IsCanUpdateUpper)
        {
            gameDataManager.currentData.livingSpaceData.Level += 1;
            EmitSignal(SignalName.OnLivingSpaceLevelUpdate);

            return true;
        }

        return false;
    }

    public bool TryAddCharacterToUsed(CharacterData InCharacterData)
    {
        var charactersUsed = gameDataManager.currentData.livingSpaceData.UsedCharacters;
        if (4 <= charactersUsed.Count)
        {
            return false;
        }
    
        if (charactersUsed.Any(character => character.ID == InCharacterData.ID))
        {
            return false;
        }

        charactersUsed.Add(InCharacterData);
        EmitSignal(SignalName.OnUsedCharactersListUpdate);
        return true;
    }
    public bool TryDeleteCharacterFromUsed(string CharacterID)
    {
        var charactersUsed = gameDataManager.currentData.livingSpaceData.UsedCharacters;

        var existingCharacter = gameDataManager.currentData.livingSpaceData.UsedCharacters.FirstOrDefault(character => character.ID == CharacterID);
        if (existingCharacter != null)
        {
            charactersUsed.Remove(existingCharacter);

            EmitSignal(SignalName.OnUsedCharactersListUpdate);
            return true;
        }

        return false;
    }
    public bool TryAddCharacterToReserved(CharacterData InCharacterData, int ToDelete = 0)
    {
        var charactersReserved = gameDataManager.currentData.livingSpaceData.ReservedCharacters;
        LivingSpaceLevelData currentLivingSpaceLevelData = gameDataManager.livingSpaceDatabase.Levels[gameDataManager.currentData.livingSpaceData.Level - 1];
        if (currentLivingSpaceLevelData.Capacity <= charactersReserved.Count + gameDataManager.currentData.livingSpaceData.UsedCharacters.Count - ToDelete)
        {
            return false;
        }
    
        if (charactersReserved.Any(character => character.ID == InCharacterData.ID))
        {
            return false;
        }

        charactersReserved.Add(InCharacterData);
        EmitSignal(SignalName.OnReservedCharactersListUpdate);
        return true;
    }
    public bool TryDeleteCharacterFromReserved(string CharacterID)
    {
        var charactersReserved = gameDataManager.currentData.livingSpaceData.ReservedCharacters;

        var existingCharacter = gameDataManager.currentData.livingSpaceData.ReservedCharacters.FirstOrDefault(character => character.ID == CharacterID);
        if (existingCharacter != null)
        {
            charactersReserved.Remove(existingCharacter);

            EmitSignal(SignalName.OnReservedCharactersListUpdate);
            return true;
        }

        return false;
    }

    public bool IsCharacterInUsedList(string CharacterID)
    {
        var charactersUsed = gameDataManager.currentData.livingSpaceData.UsedCharacters;
        var existingCharacter = charactersUsed.FirstOrDefault(character => character.ID == CharacterID);
        if (existingCharacter != null)
        {
            return true;
        }
        return false;
    }
    public bool IsCharacterInReservedList(string CharacterID)
    {
        var charactersReserved = gameDataManager.currentData.livingSpaceData.ReservedCharacters;
        var existingCharacter = charactersReserved.FirstOrDefault(character => character.ID == CharacterID);
        if (existingCharacter != null)
        {
            return true;
        }
        return false;
    }

    public int GetUsedCharactersCount()
    {
        return gameDataManager.currentData.livingSpaceData.UsedCharacters.Count;
    }
    public List<CharacterData> GetListUnarmedCharactersFormUsed()
    {
        List<CharacterData> characters = new List<CharacterData>();

        foreach (CharacterData characterData in gameDataManager.currentData.livingSpaceData.UsedCharacters)
        {
            if (characterData.Weapon.Name == "NONE")
            {
                characters.Add(characterData);
            }
        }

        return characters;
    }
}
