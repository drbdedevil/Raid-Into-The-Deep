using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public class WeaponData
{
    public string ID { get; set; } = "NONE";
    public string Name { get; set; } = "NONE";
    public int Damage { get; set; } = 0;
    public int AttackShapeID { get; set; } = 0;
    public int EffectID { get; set; } = 0;
    public string TextureName { get; set; } = "NONE";
}

public class CharacterData
{
    public string ID { get; set; } = "NONE";
    public string Name { get; set; } = "NONE";
    public string Portrait { get; set; } = ""; // TODO: DELETE
    public int PortraitID { get; set; } = 0;
    public int Damage { get; set; } = 0;
    public int DamageByEffect { get; set; } = 0;
    public int Health { get; set; } = 0;
    public int Heal { get; set; } = 0;
    public int Speed { get; set; } = 0;
    public bool[] Upgrades { get; set; } = new bool[0]; // TODO: DELETE
    public WeaponData Weapon { get; set; } = new();
    public int SkillPoints { get; set; } = 10;
    public int Level { get; set; } = 1;

    public Dictionary<string, int> PassiveSkillLevels { get; set; } = new();
    public HashSet<string> ActiveSkills { get; set; } = new();
    public string ChoosenSkills { get; set; } = "NONE";
}

public class StorageData
{
    public int Level { get; set; } = 1;
    public int Crystals { get; set; } = 120;
    public int ChitinFragments { get; set; } = 150;
    public List<WeaponData> Weapons { get; set; } = new();
}

public class LivingSpaceData
{
    public int Level { get; set; } = 1;
    public List<CharacterData> UsedCharacters { get; set; } = new();
    public List<CharacterData> ReservedCharacters { get; set; } = new();
}

public class ForgeData
{
    public int Level { get; set; } = 1;
    public List<WeaponData> WeaponsForShackle { get; set; } = new();
}

public class TrainingPitsData
{
    public int Level { get; set; } = 1;
    public List<CharacterData> CharactersForHiring { get; set; } = new();
}

public class CommandBlockData
{
    public int RaidCount { get; set; } = 5;
    public int EnemyDefeated { get; set; } = 5;
    public int SquadLevel { get; set; } = 11;
    public bool VegetableDefeated { get; set; } = true;
    public bool TankDefeated { get; set; } = true;
    public bool SpiderBossDefeated { get; set; } = false;
}

public class RunMapData
{
    
}

public class GameData
{
    public StorageData storageData = new();
    public LivingSpaceData livingSpaceData = new();
    public ForgeData forgeData = new();
    public TrainingPitsData trainingPitsData = new();
    public CommandBlockData commandBlockData = new();
    public RunMapData runMapData = new();
}



public enum SkillKind
{
    Passive,
    Active
}
[AttributeUsage(AttributeTargets.Field)]
public class SkillCategoryAttribute : Attribute
{
    public SkillKind Kind { get; }
    public SkillCategoryAttribute(SkillKind kind)
    {
        Kind = kind;
    }
}

public enum ESkillType
{
    // Passive skills
    [SkillCategory(SkillKind.Passive)]
    Health,
    [SkillCategory(SkillKind.Passive)]
    Speed,
    [SkillCategory(SkillKind.Passive)]
    Damage,
    [SkillCategory(SkillKind.Passive)]
    DamageByEffect,
    [SkillCategory(SkillKind.Passive)]
    Heal,

    // Active skills
    [SkillCategory(SkillKind.Active)]
    ReverseWave,
    [SkillCategory(SkillKind.Active)]
    IronWall,
    [SkillCategory(SkillKind.Active)]
    Defense,
    [SkillCategory(SkillKind.Active)]
    SevereWound,
    [SkillCategory(SkillKind.Active)]
    BattleFrenzy,
    [SkillCategory(SkillKind.Active)]
    Leap,
    [SkillCategory(SkillKind.Active)]
    PoisonCloud,
    [SkillCategory(SkillKind.Active)]
    BloodMark,
    [SkillCategory(SkillKind.Active)]
    SilentMeditations,
    [SkillCategory(SkillKind.Active)]
    RemoveEffects,
    [SkillCategory(SkillKind.Active)]
    RestorationField,
    [SkillCategory(SkillKind.Active)]
    BurningTrail
}

public static class SkillExtensions
{
    public static SkillKind GetCategory(this ESkillType skill)
    {
        var member = typeof(ESkillType).GetMember(skill.ToString()).FirstOrDefault();
        var attribute = member?.GetCustomAttribute<SkillCategoryAttribute>();
        return attribute?.Kind ?? SkillKind.Passive;
    }

    public static bool IsPassive(this ESkillType skill)
    {
        return skill.GetCategory() == SkillKind.Passive;
    }

    public static bool IsActive(this ESkillType skill)
    {
        return skill.GetCategory() == SkillKind.Active;
    }
}