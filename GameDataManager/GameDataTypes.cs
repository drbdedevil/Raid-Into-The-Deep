using System;
using System.Collections.Generic;

public class WeaponData
{
    public string ID { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; }
    public int AttackShapeID { get; set; }
    public int EffectID { get; set; }
    public string TextureName { get; set; }
}

public class CharacterData
{
    public string Name { get; set; }
    public string Portrait { get; set; }
    public int Damage { get; set; }
    public int Heal { get; set; }
    public int Speed { get; set; }
    public bool[] Upgrades { get; set; }
    public WeaponData Weapon { get; set; }
}

public class StorageData
{
    public int Level { get; set; } = 1;
    public int Crystals { get; set; } = 120;
    public int ChitinFragments { get; set; } = 90;
    public List<WeaponData> Weapons { get; set; } = new();
}

public class LivingSpaceData
{
    public int Level { get; set; }
    public List<CharacterData> UsedCharacters { get; set; }
    public List<CharacterData> ReserveCharacters { get; set; }
}

public class ForgeData
{
    public int Level { get; set; }
}

public class TrainingPitsData
{
    public int Level { get; set; }
    public List<CharacterData> CharactersForHiring { get; set; }
}

public class CommandBlockData
{
    public int RaidCount { get; set; }
    public int EnemyDefeated { get; set; }
    public int SquadLevel { get; set; }
    public bool VegetableDefeated { get; set; }
    public bool TankDefeated { get; set; }
    public bool SpiderBossDefeated { get; set; }
}

public class RunMapData
{
    
}

public class GameData
{
    public StorageData storageData = new();
    public LivingSpaceData livingSpaceData = new();
    public ForgeData forgeData = new();
    public TrainingPits trainingPits = new();
    public CommandBlockData commandBlockData = new();
    public RunMapData runMapData = new();
}