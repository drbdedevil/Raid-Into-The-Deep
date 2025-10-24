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
        GenerateWeaponsForShackle();
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
    private bool TryAddWeaponForShackle(WeaponData InWeaponData)
    {
        var weaponForShackle = gameDataManager.currentData.forgeData.WeaponsForShackle;
    
        /* if (weaponForShackle.Any(weapon => weapon.ID == InWeaponData.ID))
        {
            return false;
        } */

        weaponForShackle.Add(InWeaponData);
        return true;
    }
    public bool TryDeleteWeaponForShackle(string CharacterID)
    {
        return false;
    }

    private void GenerateWeaponsForShackle()
    {
        for (int i = 0; i < gameDataManager.weaponDatabase.Weapons.Count; ++i)
        {
            WeaponRow currentWeaponRow = gameDataManager.weaponDatabase.Weapons[i];

            WeaponData weaponData = new WeaponData();
            weaponData.ID = "NOT ASSIGNED"; // ПРИСВОИТСЯ ВО ВРЕМЯ КОВКИ
            weaponData.Name = currentWeaponRow.Name;
            weaponData.Damage = 0; // GD.RandRange(currentWeaponRow.DamageRange.X, currentWeaponRow.DamageRange.Y); ПРИСВОИТСЯ ВО ВРЕМЯ КОВКИ
            weaponData.AttackShapeID = currentWeaponRow.AttackShapeID;
            weaponData.EffectID = 0; // ADD EFFECT DATABASE - ВО ВРЕМЯ КОВКИ
            weaponData.TextureName = currentWeaponRow.WeaponTexture.ResourceName;

            TryAddWeaponForShackle(weaponData);
        }
    }
}
