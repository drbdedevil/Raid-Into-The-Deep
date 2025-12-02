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

    public void GenerateWeaponsForShackle()
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
    public WeaponData GenerateRandomWeapon()
    {
        int randomIndex = GD.RandRange(0, gameDataManager.weaponDatabase.Weapons.Count - 1);
        WeaponRow currentWeaponRow = gameDataManager.weaponDatabase.Weapons[randomIndex];

        WeaponData weaponData = new WeaponData();
        weaponData.ID = Guid.NewGuid().ToString();
        weaponData.Name = currentWeaponRow.Name;
        weaponData.Damage = GD.RandRange(currentWeaponRow.DamageRange.X, currentWeaponRow.DamageRange.Y);
        weaponData.AttackShapeID = currentWeaponRow.AttackShapeID;
        weaponData.EffectID = GetWeightedRandomEffect();
        weaponData.TextureName = currentWeaponRow.WeaponTexture.ResourceName;

        return weaponData;
    }

    public int GetWeightedRandomEffect()
	{
		var effects = GameDataManager.Instance.effectDatabase.Effects;

		float totalWeight = 0f;
		foreach (var effect in effects)
		{
			totalWeight += effect.Weight;
		}

		double randomValue = GD.RandRange(0f, totalWeight);

		float currentHeight = 0f;
		for (int i = 0; i < effects.Count; ++i)
		{
			currentHeight += effects[i].Weight;
			if (randomValue <= currentHeight)
			{
				return i;
			}
		}
		
		return 0;
	}
}
