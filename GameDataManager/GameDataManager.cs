using Godot;
using System;

public partial class GameDataManager : Node
{
	public static GameDataManager Instance { get; private set; }

	[Export]
	public string SavePath;
	public GameData currentData { get; set; } = new();
	
	// Submanagers
	public StorageDataManager storageDataManager { get; private set; }
	public LivingSpaceDataManager livingSpaceDataManager { get; private set; }
	public TrainingPitsDataManager trainingPitsDataManager { get; private set; }

	// Databases
	[Export]
	public StorageDatabase storageDatabase { get; private set; }
	[Export]
	public LivingSpaceDatabase livingSpaceDatabase { get; private set; }
	[Export]
	public TrainingPitsDatabase trainingPitsDatabase { get; private set; }

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
		GD.Print(" -- GameDataManager init -- ");
	}
}
