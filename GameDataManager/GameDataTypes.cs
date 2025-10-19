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
    public string ID { get; set; }
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
    public int Level { get; set; }
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