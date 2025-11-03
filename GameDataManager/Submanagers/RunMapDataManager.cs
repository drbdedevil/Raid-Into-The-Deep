using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RunMapDataManager : Node
{
    private GameDataManager gameDataManager;

    [Signal]
    public delegate void OnRunMapListUpdateEventHandler();

    public RunMapDataManager(GameDataManager InGameDataManager)
    {
        gameDataManager = InGameDataManager;
    }

    public MapNode pressedMapNode = new();

    public List<MapNode> GetNeighborsFloor(MapNode mapNode)
    {
        List<MapNode> neighbors = new List<MapNode>();

        List<List<MapNode>> map = gameDataManager.currentData.runMapData.runMapList;
        foreach (List<MapNode> floor in map)
        {
            if (floor.Contains(mapNode))
            {
                neighbors = floor;
                break;
            }
        }

        return neighbors;
    }

    public void GiveAwardToPlayer()
    {
        var valuesDatabase = gameDataManager.raceMapValuesDatabase;
        int crystals = GD.RandRange(valuesDatabase.MinCrystalsAward, valuesDatabase.MaxCrystalsAward);
        int chitins = GD.RandRange(valuesDatabase.MinChitinFragmentsAward, valuesDatabase.MaxChitinFragmentsAward);

        gameDataManager.storageDataManager.AdjustCrystals(crystals);
        gameDataManager.storageDataManager.AdjustChitinFragments(chitins);
    }
    public void HealTeam()
    {
        var valuesDatabase = gameDataManager.raceMapValuesDatabase;
        var levelsDatabase = gameDataManager.passiveSkillsProgressionDatabase;
        var team = gameDataManager.currentData.livingSpaceData.UsedCharacters;

        foreach (CharacterData characterData in team)
        {
            PassiveSkillProgressionRow progressionRow = levelsDatabase.Progressions.FirstOrDefault(progression => progression.skillType == ESkillType.Health);
            if (progressionRow != null)
            {
                KeyValuePair<string, int> healLevel = characterData.PassiveSkillLevels.FirstOrDefault(skill => skill.Key == "Здоровье");

                int MaxHealth = gameDataManager.baseStatsDatabase.Health;
                for (int i = 0; i < healLevel.Value; ++i)
                {
                    MaxHealth += progressionRow.increments[healLevel.Value - 1];
                }
                int CurrentHealth = characterData.Health;

                float heal = MaxHealth * ((float)valuesDatabase.HealPercent / 100f);
                int result = CurrentHealth + (int)heal;
                if (result > MaxHealth)
                {
                    result = MaxHealth;
                }

                characterData.Health = result;
            }
        }

        GameDataManager.Instance.livingSpaceDataManager.EmitSignal(LivingSpaceDataManager.SignalName.OnUsedCharactersListUpdate);
    }

    public void RunBattle(MapNode mapNode)
    {
        pressedMapNode = mapNode;
        PassMapNode(); // TODO: запустить уровень с боем и при победе сделать PassMapNode для pressedMapNode
    }
    public void RunEliteBattle(MapNode mapNode)
    {
        pressedMapNode = mapNode;
        PassMapNode(); // TODO: запустить уровень с элитным боем и при победе сделать PassMapNode для pressedMapNode
    }
    public void RunBossBattle(MapNode mapNode)
    {
        pressedMapNode = mapNode;
        PassMapNode(); // TODO: запустить уровень с боссом и при победе сделать PassMapNode для pressedMapNode
    }

    public void PassMapNode()
    {
        pressedMapNode.PassMapNode();
        pressedMapNode = new MapNode();

        GameDataManager.Instance.runMapDataManager.EmitSignal(RunMapDataManager.SignalName.OnRunMapListUpdate); // TODO: По идее, это временно
    }
}
