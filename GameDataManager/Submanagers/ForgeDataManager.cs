using Godot;
using System;
using System.Linq;
using System.Numerics;

public partial class ForgeDataManager : Node
{
    private GameDataManager gameDataManager;
    [Signal]
    public delegate void OnForgeLevelUpdateEventHandler();
    [Signal]
    public delegate void OnWeaponForShackleListUpdateEventHandler();

    public ForgeDataManager(GameDataManager InGameDataManager)
    {
        gameDataManager = InGameDataManager;
        GenerateWeaponsForShackleToMaxValue();
    }

    public bool TryUpdateLevel()
    {
        bool IsCanUpdateUpper = gameDataManager.currentData.forgeData.Level < gameDataManager.forgeDatabase.Levels.Count;
        if (IsCanUpdateUpper)
        {
            gameDataManager.currentData.forgeData.Level += 1;
            EmitSignal(SignalName.OnForgeLevelUpdate);

            return true;
        }

        return false;
    }
    public void GenerateWeaponsForShackleToMaxValue()
    {
        for (int i = 0; i < gameDataManager.forgeDatabase.MaxWeaponsForShackle; ++i)
        {
            TryAddWeaponForShackle(GenerateRandomWeapon());
        }
    }
    public bool TryAddWeaponForShackle(WeaponData InWeaponData)
    {
        var weaponForShackle = gameDataManager.currentData.forgeData.WeaponsForShackle;
    
        if (weaponForShackle.Any(weapon => weapon.ID == InWeaponData.ID))
        {
            return false;
        }

        weaponForShackle.Add(InWeaponData);
        return true;
    }
    public bool TryDeleteWeaponForShackle(string CharacterID)
    {
        return false;
    }

    private WeaponData GenerateRandomWeapon()
    {
        WeaponData weaponData = new WeaponData();
        weaponData.ID = Guid.NewGuid().ToString();

        int randomValue = GD.RandRange(0, gameDataManager.weaponDatabase.Weapons.Count - 1);
        if (randomValue < gameDataManager.weaponDatabase.Weapons.Count)
        {
            WeaponRow randomWeaponRow = gameDataManager.weaponDatabase.Weapons[randomValue];

            weaponData.Name = randomWeaponRow.Name;
            weaponData.DamageRange = new System.Numerics.Vector2(randomWeaponRow.DamageRange.X, randomWeaponRow.DamageRange.Y);
            weaponData.AttackShapeID = randomWeaponRow.AttackShapeID;
            weaponData.EffectID = 0; // ADD EFFECT DATABASE
            weaponData.TextureName = randomWeaponRow.WeaponTexture.ResourceName;

            return weaponData;
        }

        return new WeaponData();
    }
}
