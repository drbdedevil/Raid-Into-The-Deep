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

	// Databases
	[Export] 
	public StorageDatabase storageDatabase { get; private set; }

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
		GD.Print(" -- GameDataManager init -- ");
	}
}
