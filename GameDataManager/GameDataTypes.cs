using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.Json.Serialization;

[Serializable]
public class WeaponData
{
    public string ID { get; set; } = "NONE";
    public string Name { get; set; } = "NONE";
    public int Damage { get; set; } = 0;
    public int AttackShapeID { get; set; } = 0;
    public int EffectID { get; set; } = 0;
    public string TextureName { get; set; } = "NONE";
}

[Serializable]
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

[Serializable]
public class StorageData
{
    public int Level { get; set; } = 1;
    public int Crystals { get; set; } = 120;
    public int ChitinFragments { get; set; } = 150;
    public List<WeaponData> Weapons { get; set; } = new();
}

[Serializable]
public class LivingSpaceData
{
    public int Level { get; set; } = 1;
    public List<CharacterData> UsedCharacters { get; set; } = new();
    public List<CharacterData> ReservedCharacters { get; set; } = new();
}

[Serializable]
public class ForgeData
{
    public int Level { get; set; } = 1;
    public List<WeaponData> WeaponsForShackle { get; set; } = new();
}

[Serializable]
public class TrainingPitsData
{
    public int Level { get; set; } = 1;
    public List<CharacterData> CharactersForHiring { get; set; } = new();
}

[Serializable]
public class CommandBlockData
{
    public int RaidCount { get; set; } = 0;
    public int EnemyDefeated { get; set; } = 0;
    public int SquadLevel { get; set; } = 11;
    public bool VegetableDefeated { get; set; } = false;
    public bool TankDefeated { get; set; } = false;
    public bool SpiderBossDefeated { get; set; } = false;
}

[Serializable]
public class RunMapData
{
    public bool bShouldShowRegenerateButton { get; set; } = false;
    public bool bShouldRegenerate { get; set; } = true;
    public List<List<MapNode>> runMapList { get; set; } = new();
}

[Serializable]
public class GameData
{
    public StorageData storageData = new();
    public LivingSpaceData livingSpaceData = new();
    public ForgeData forgeData = new();
    public TrainingPitsData trainingPitsData = new();
    public CommandBlockData commandBlockData = new();
    public RunMapData runMapData = new();
}


public enum AttackShapeType
{
    Melee = 0,
    LongMelee = 1,
    Sweep = 2,
    ScatterShot = 3,
    Ranged = 4,
    Artillery = 5
}

// --------------------------------------- Skills ---------------------------------------
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


// --------------------------------------- Map ---------------------------------------
[Serializable]
public enum MapNodeType
{
    Start = 0,
    Battle = 1,
    Rest = 2,
    EliteBattle = 3,
    Treasure = 4,
    SpiderBoss = 5,
    TankBoss = 6,
    VegetableBoss = 7,
    RandomEvent = 8
}

[Serializable]
public class MapNode
{
    private static int _nextId = 0;
     public static void ResetIds() => _nextId = 0;
    public MapNode() { Id = _nextId++; }
    public int Row = -1;
    public int Col = -1;
    public MapNodeType Type = MapNodeType.Start;

    [JsonIgnore]
    public List<MapNode> Next = new List<MapNode>();
    public bool IsActive = true;
    public bool IsPassed = false;
    public float randomOffsetX { get; set; } = 0f;
    public float randomOffsetY { get; set; } = 0f;

    public int Id { get; set; }
    public List<int> NextIds = new List<int>();

    public void PassMapNode()
    {
        List<MapNode> neighbors = GameDataManager.Instance.runMapDataManager.GetNeighborsFloor(this);
        foreach (MapNode neigbor in neighbors)
        {
            if (neigbor != this)
            {
                neigbor.IsActive = false;
            }
        }

        List<MapNode> nexts = Next;
        foreach (MapNode next in nexts)
        {
            next.IsActive = true;
        }
        IsActive = false;
        IsPassed = true;
    }
}