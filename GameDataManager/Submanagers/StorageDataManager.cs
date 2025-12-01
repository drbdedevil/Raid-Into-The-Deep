using Godot;
using System;
using System.Linq;

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
        gameDataManager.currentData.storageData.Crystals = gameDataManager.currentData.storageData.Crystals > 1000 ? 1000 : gameDataManager.currentData.storageData.Crystals;
        EmitSignal(SignalName.OnCrystalsUpdate);
    }
    public void AdjustChitinFragments(Int32 Value)
    {
        gameDataManager.currentData.storageData.ChitinFragments += Value;
        gameDataManager.currentData.storageData.ChitinFragments = gameDataManager.currentData.storageData.ChitinFragments > 1000 ? 1000 : gameDataManager.currentData.storageData.ChitinFragments;
        EmitSignal(SignalName.OnChitinFragmentsUpdate);
    }
    public bool TryAddWeapon(WeaponData InWeaponData)
    {
        var weapons = gameDataManager.currentData.storageData.Weapons;
        StorageLevelData levelData = gameDataManager.storageDatabase.Levels[gameDataManager.currentData.storageData.Level - 1];
        if (levelData.Capacity <= weapons.Count)
        {
            return false;
        }
    
        if (weapons.Any(weapon => weapon.ID == InWeaponData.ID))
        {
            return false;
        }

        weapons.Add(InWeaponData);
        return true;
    }
    public bool TryDeleteWeapon(string WeaponID)
    {
        var weapons = gameDataManager.currentData.storageData.Weapons;

        var existingWeapon = GameDataManager.Instance.currentData.storageData.Weapons.FirstOrDefault(weapon => weapon.ID == WeaponID);
        if (existingWeapon != null)
        {
            weapons.Remove(existingWeapon);

            return true;
        }

        return false;
    }
}
