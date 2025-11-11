using System.Linq;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public static class WeaponFactory
{
    public static Weapon CreateWeaponByAttackShapeType(AttackShapeType shapeType)
    {
        switch (shapeType)
        {
            case AttackShapeType.Melee: return new MeleeWeapon((int)shapeType, new WeaponData(GameDataManager.Instance.weaponDatabase.Weapons[(int)shapeType]));
            case AttackShapeType.LongMelee: return new MeleeWeapon((int)shapeType, new WeaponData(GameDataManager.Instance.weaponDatabase.Weapons[(int)shapeType]));;
            case AttackShapeType.Sweep: return new MeleeWeapon((int)shapeType, new WeaponData(GameDataManager.Instance.weaponDatabase.Weapons[(int)shapeType]));
            case AttackShapeType.ScatterShot: return new RangeWeapon((int)shapeType, new WeaponData(GameDataManager.Instance.weaponDatabase.Weapons[(int)shapeType]));
            case AttackShapeType.Ranged: return new RangeWeapon((int)shapeType, new WeaponData(GameDataManager.Instance.weaponDatabase.Weapons[(int)shapeType]));
            case AttackShapeType.Artillery: return new ArtilleryWeapon((int)shapeType, new WeaponData(GameDataManager.Instance.weaponDatabase.Weapons[(int)shapeType]));
            default: return null;
        }
    }
}