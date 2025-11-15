using Godot;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public partial class GameDataManager : Node
{
	public static GameDataManager Instance { get; private set; }

	[Export]
	private string saveName = "";

	public GameData currentData { get; set; } = new();

	[Export]
	public bool IsShouldGenerateWeaponOnStartToEveryCreatedCharacter = false;
	
	// Submanagers
	public StorageDataManager storageDataManager { get; private set; }
	public LivingSpaceDataManager livingSpaceDataManager { get; private set; }
	public TrainingPitsDataManager trainingPitsDataManager { get; private set; }
	public ForgeDataManager forgeDataManager { get; private set; }
	public CommandBlockDataManager commandBlockDataManager { get; private set; }
	public RunMapDataManager runMapDataManager { get; private set; }

	// Databases
	[Export]
	public StorageDatabase storageDatabase { get; private set; }
	[Export]
	public LivingSpaceDatabase livingSpaceDatabase { get; private set; }
	[Export]
	public TrainingPitsDatabase trainingPitsDatabase { get; private set; }
	[Export]
	public ForgeDatabase forgeDatabase { get; private set; }

	[Export]
	public AttackShapeDatabase attackShapeDatabase { get; private set; }
	[Export]
	public EffectDatabase effectDatabase { get; private set; }
	[Export]
	public WeaponDatabase weaponDatabase { get; private set; }

	[Export]
	public BaseStatsDatabase baseStatsDatabase { get; private set; }
	
	[Export]
	public EnemyBaseStatsDatabase EnemyBaseStatsDatabase { get; private set; }
	[Export]
	public PassiveSkillsProgressionDatabase passiveSkillsProgressionDatabase { get; private set; }
	[Export]
	public CharactersSprites charactersSpritesDatabase { get; private set; }
	[Export]
	public SkillsDatabase passiveSkillsDatabase { get; private set; }
	[Export]
	public SkillsDatabase activeSkillsDatabase { get; private set; }

	[Export]
	public RaceMapDatabase raceMapDatabase { get; private set; }
	[Export]
	public RaceMapValuesDatabase raceMapValuesDatabase  { get; private set; }
	[Export]
	public CharactersExperienceLevelsDatabase charactersExperienceLevelsDatabase  { get; private set; }

	// Scripts
	public override void _Ready()
	{
		if (Instance != null && Instance != this)
		{
			QueueFree();
			return;
		}
		Instance = this;

		storageDataManager = new StorageDataManager(this);
		livingSpaceDataManager = new LivingSpaceDataManager(this);
		trainingPitsDataManager = new TrainingPitsDataManager(this);
		forgeDataManager = new ForgeDataManager(this);
		commandBlockDataManager = new CommandBlockDataManager(this);
		runMapDataManager = new RunMapDataManager(this);

		GD.Print(" -- GameDataManager init -- ");
	}
	public void CreateNewGame()
	{
		currentData = new GameData();
		forgeDataManager.GenerateWeaponsForShackle();
		trainingPitsDataManager.GenerateCharactersForHiring(6);
	}

	public void SetSavePath(string InSaveName)
	{
		saveName = InSaveName;
	}
	public string GetSavePath()
	{
		return "user://saves/" + saveName + ".json";
	}
	public void Save()
	{
		try
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
				IncludeFields = true,
			};

			var absPath = ProjectSettings.GlobalizePath("user://saves/");
			if (!DirAccess.DirExistsAbsolute(absPath))
			{
				GD.Print("Папка сохранений не найдена, создаю новую...");
				DirAccess.MakeDirRecursiveAbsolute(absPath);
			}

			runMapDataManager.SaveNodeIds();
			string json = JsonSerializer.Serialize(currentData, options);
			File.WriteAllText(ProjectSettings.GlobalizePath(GetSavePath()), json);
			GD.Print($"Игра сохранена: {GetSavePath()}");
		}
		catch (Exception e)
		{
			GD.PrintErr($"Ошибка при сохранении: {e.Message}");
		}
	}
	public void Load()
	{
		try
		{
			if (!File.Exists(ProjectSettings.GlobalizePath(GetSavePath())))
			{
				GD.Print("Сохранение не найдено");
				currentData = new GameData();
			}

			string json = File.ReadAllText(ProjectSettings.GlobalizePath(GetSavePath()));
			var options = new JsonSerializerOptions { IncludeFields = true };

			MapNode.ResetIds();
			currentData = JsonSerializer.Deserialize<GameData>(json, options);
			runMapDataManager.LoadNodeIds();
			GD.Print("Сохранение загружено.");
		}
		catch (Exception e)
		{
			GD.PrintErr($"Ошибка при pfuheprt: {e.Message}");
		}
	}

	public void DeleteSave(string saveName)
	{
		string saveToDelete = "user://saves/" + saveName + ".json";

		if (File.Exists(ProjectSettings.GlobalizePath(saveToDelete)))
		{
			File.Delete(ProjectSettings.GlobalizePath(saveToDelete));
		}
	}
	
	public List<string> GetSaveList()
	{
		var saveNames = new List<string>();

		var absPath = ProjectSettings.GlobalizePath("user://saves/");
		if (!DirAccess.DirExistsAbsolute(absPath))
		{
			GD.Print("Папка сохранений не найдена, создаю новую...");
			DirAccess.MakeDirRecursiveAbsolute(absPath);
			return saveNames;
		}

		using var dir = DirAccess.Open("user://saves/");
		if (dir == null)
		{
			GD.PrintErr("Не удалось открыть папку сохранений!");
			return saveNames;
		}

		dir.ListDirBegin();
		string fileName = dir.GetNext();
		while (!string.IsNullOrEmpty(fileName))
		{
			if (!dir.CurrentIsDir() && fileName.EndsWith(".json"))
			{
				string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
				saveNames.Add(nameWithoutExt);
			}
			fileName = dir.GetNext();
		}
		dir.ListDirEnd();

		return saveNames;
	}

}
