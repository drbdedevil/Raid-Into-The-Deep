using System;
using System.Collections.Generic;
using System.Numerics;

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
    public int SkillPoints { get; set; } = 0;
    public int Level { get; set; } = 1;

    public Dictionary<string, int> PassiveSkillLevels { get; set; } = new();
    public HashSet<string> ActiveSkills { get; set; } = new();
    public string ChoosenSkills { get; set; } = "";
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
    public int Level { get; set; } = 6;
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




public enum EPassiveSkillType
{
    Health = 0,
    Speed = 1,
    Damage = 2,
    DamageByEffect = 3,
    Heal = 4
}