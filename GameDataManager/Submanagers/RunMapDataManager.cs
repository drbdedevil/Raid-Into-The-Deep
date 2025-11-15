using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RunMapDataManager : Node
{
	private GameDataManager gameDataManager;

	[Signal]
	public delegate void OnRunMapListUpdateEventHandler();
	[Signal]
	public delegate void OnBossWasDefeatedEventHandler();

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

		var tree = Engine.GetMainLoop() as SceneTree;
		tree.CreateTimer(0.7f).Timeout += () =>
		{
			StartPlayRandomBattleSoundLoop();
		};
		
		var scene = ResourceLoader.Load<PackedScene>("res://Levels/Fight/PrepareFightScene/PrepareFightScene.tscn");
		SceneTree sceneTree = Engine.GetMainLoop() as SceneTree;
		sceneTree.ChangeSceneToPacked(scene);
	}
	public void RunEliteBattle(MapNode mapNode)
	{
		pressedMapNode = mapNode;
		
		var tree = Engine.GetMainLoop() as SceneTree;
		tree.CreateTimer(0.7f).Timeout += () =>
		{
			StartPlayRandomBattleSoundLoop();
		};

		PassMapNode(); // TODO: запустить уровень с элитным боем и при победе сделать PassMapNode для pressedMapNode
	}
	public void RunBossBattle(MapNode mapNode, MapNodeType bossType)
	{
		var tree = Engine.GetMainLoop() as SceneTree;

		pressedMapNode = mapNode;
		switch (bossType)
		{
			case MapNodeType.SpiderBoss:
				tree.CreateTimer(0.7f).Timeout += () =>
				{
					StartPlaySoundLoopForSpiderBoss();
				};
				PassMapNode();
				gameDataManager.currentData.commandBlockData.SpiderBossDefeated = true;
				break;
			case MapNodeType.TankBoss:
				tree.CreateTimer(0.7f).Timeout += () =>
				{
					StartPlaySoundLoopForTankBoss();
				};
				PassMapNode();
				gameDataManager.currentData.commandBlockData.TankDefeated = true;
				break;
			case MapNodeType.VegetableBoss:
				tree.CreateTimer(0.7f).Timeout += () =>
				{
					StartPlaySoundLoopForVegetableBoss();
				};
				PassMapNode();
				gameDataManager.currentData.commandBlockData.VegetableDefeated = true;
				break;
			default:
				break;
		} // TODO: запустить уровень с нужным боссом и при победе сделать PassMapNode для pressedMapNode
		gameDataManager.currentData.runMapData.bShouldShowRegenerateButton = true;
		EmitSignal(SignalName.OnBossWasDefeated);
	}

	public void PassMapNode()
	{
		pressedMapNode.PassMapNode();
		pressedMapNode = new MapNode();

		GameDataManager.Instance.commandBlockDataManager.RestoreHealthToTheReservists();

		int currentLevel = gameDataManager.currentData.trainingPitsData.Level;
        TrainingPitsLevelData currentTrainingPitsLevelData = gameDataManager.trainingPitsDatabase.Levels[currentLevel - 1];
		GameDataManager.Instance.trainingPitsDataManager.GenerateCharactersForHiring(currentTrainingPitsLevelData.Capacity / 4);

		GameDataManager.Instance.runMapDataManager.EmitSignal(RunMapDataManager.SignalName.OnRunMapListUpdate); // TODO: По идее, это временно
	}

	public void SaveNodeIds()
	{
		foreach (var floor in gameDataManager.currentData.runMapData.runMapList)
		{
			foreach (var node in floor)
			{
				node.NextIds.Clear();
				foreach (var next in node.Next)
				{
					node.NextIds.Add(next.Id);
				}
			}
		}
	}
	public void LoadNodeIds()
	{
		List<MapNode> allNodes = new List<MapNode>();
		foreach (var floor in gameDataManager.currentData.runMapData.runMapList)
		{
			foreach (var node in floor)
			{
				allNodes.Add(node);
			}
		}

		var lookup = new Dictionary<int, MapNode>();
		foreach (var node in allNodes)
		{
			if (!lookup.ContainsKey(node.Id))
				lookup.Add(node.Id, node);
			else
				GD.PrintErr($"Дублирующийся ID найден: {node.Id}");
		}

		foreach (var node in allNodes)
		{
			node.Next.Clear();
			foreach (int nextId in node.NextIds)
			{
				if (lookup.TryGetValue(nextId, out var nextNode))
				{
					node.Next.Add(nextNode);
				}
				else
				{
					GD.PrintErr($"Не найден узел с ID {nextId} для ноды {node.Id}");
				}
			}
		}
	}

	private void StartPlayRandomBattleSoundLoop()
    {
        List<string> sounds = new List<string>() {
			"res://Sound/Music/Battle/Fight1_1.wav",
			"res://Sound/Music/Battle/Fight2_1.wav",
			"res://Sound/Music/Battle/Fight3_1.wav" };
		int randSound = GD.RandRange(0, sounds.Count - 1);
		// SoundManager.Instance.RemoveAllSounds();
		SoundManager.Instance.PlaySoundLoop(sounds[randSound], 0.2f);
    }
	private void StartPlaySoundLoopForSpiderBoss()
    {
        SoundManager.Instance.PlaySoundLoop("res://Sound/Music/BossBattle/SpiderBoss.wav", 0.2f);
    }
	private void StartPlaySoundLoopForTankBoss()
    {
        SoundManager.Instance.PlaySoundLoop("res://Sound/Music/BossBattle/Tank.wav", 0.2f);
    }
	private void StartPlaySoundLoopForVegetableBoss()
    {
        SoundManager.Instance.PlaySoundLoop("res://Sound/Music/BossBattle/Vegetable.wav", 0.2f);
    }
}
