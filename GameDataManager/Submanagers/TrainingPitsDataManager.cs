using Godot;
using System;

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
        return false;
    }
    public bool TryDeleteCharacterForHiring(string CharacterID)
    {
        return false;
    }
}
