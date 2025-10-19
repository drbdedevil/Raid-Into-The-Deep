using Godot;
using System;

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
        return false;
    }
    public bool TryDeleteCharacterFromUsed(string CharacterID)
    {
        return false;
    }
    public bool TryAddCharacterToReserved(CharacterData InCharacterData)
    {
        return false;
    }
    public bool TryDeleteCharacterFromReserved(string CharacterID)
    {
        return false;
    }
}
