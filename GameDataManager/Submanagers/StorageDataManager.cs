using Godot;
using System;

public partial class StorageDataManager : Node
{
    private GameDataManager gameDataManager;
    [Signal]
    public delegate void OnStorageLevelUpdateEventHandler();
    [Signal]
    public delegate void OnCrystalsUpdateEventHandler();
    [Signal]
    public delegate void OnChitinFragmentsUpdateEventHandler();
    [Signal]
    public delegate void OnWeaponListUpdateEventHandler();

    public StorageDataManager(GameDataManager InGameDataManager)
    {
        gameDataManager = InGameDataManager;
    }

    public bool TryUpdateLevel()
    {
        bool IsCanUpdateUpper = gameDataManager.currentData.storageData.Level < gameDataManager.storageDatabase.Levels.Count;
        if (IsCanUpdateUpper)
        {
            gameDataManager.currentData.storageData.Level += 1;
            EmitSignal(SignalName.OnStorageLevelUpdate);

            return true;
        }

        return false;
    }
    public bool IsCrystalsEnough(Int32 Value)
    {
        if (Value > gameDataManager.currentData.storageData.Crystals)
        {
            return false;
        }
        return true;
    }
    public bool IsChitinFragmentsEnough(Int32 Value)
    {
        if (Value > gameDataManager.currentData.storageData.ChitinFragments)
        {
            return false;
        }
        return true;
    }
    public void AdjustCrystals(Int32 Value)
    {
        gameDataManager.currentData.storageData.Crystals += Value;
        EmitSignal(SignalName.OnCrystalsUpdate);
    }
    public void AdjustChitinFragments(Int32 Value)
    {
        gameDataManager.currentData.storageData.ChitinFragments += Value;
        EmitSignal(SignalName.OnChitinFragmentsUpdate);
    }
    public bool TryAddWeapon(WeaponData InWeaponData)
    {
        return false;
    }
    public bool TryDeleteWeapon(string WeaponID)
    {
        return false;
    }
}
