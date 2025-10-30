using Godot;
using System;

public partial class HubLocation : Node
{
	[Export]
	private PackedScene PopupScene;
	[Export]
	public PackedScene WarriorPanelScene;
	[Export]
	private PackedScene RaceMapScene;

	public override void _Ready()
	{
		var MenuButton = GetNode<Button>("MainPanel/Panel/VBoxContainer/HBoxContainer/MenuButton");
		MenuButton.ButtonDown += OnMenuButtonPressed;

		var SettingsButton = GetNode<Button>("MainPanel/Panel/VBoxContainer/HBoxContainer/SettingsButton");
		SettingsButton.ButtonDown += OnSettingsButtonPressed;

		Button RaceMapButton = GetNode<Button>("MainPanel/Control/RaceMapButton");
		RaceMapButton.ButtonDown += OnRaceMapButtonPressed;

		// ----------- Base Interset Button -----------
		var TrainingPitsButton = GetNode<Button>("MainPanel/Control/TrainingPitsButton");
		TrainingPitsButton.ButtonDown += OnTrainingPitsPressed;

		var StorageButton = GetNode<Button>("MainPanel/Control/StorageButton");
		StorageButton.ButtonDown += OnStoragePressed;

		var CommandBlockButton = GetNode<Button>("MainPanel/Control/CommandBlockButton");
		CommandBlockButton.ButtonDown += OnCommandBlockPressed;

		var LivingSpaceButton = GetNode<Button>("MainPanel/Control/LivingSpaceButton");
		LivingSpaceButton.ButtonDown += OnLivingSpacePressed;

		var ForgeButton = GetNode<Button>("MainPanel/Control/ForgeButton");
		ForgeButton.ButtonDown += OnForgePressed;

		//  -----------

		// var close_button = GetNode<Button>("PopupPanel/MarginContainer/VBoxContainer/HBoxContainer2/MarginContainer/ClosePopupButton");
		// close_button.ButtonDown += OnClosePopupPressed;
		// var popup = GetNode<PopupPanel>("PopupPanel");
		// popup.PopupHide += OnClosePopupPressed;

		// ----------- View Realization -----------
		// ----- Binding Functions
		GameDataManager.Instance.storageDataManager.OnCrystalsUpdate += OnCrystalsUpdate;
		GameDataManager.Instance.storageDataManager.OnChitinFragmentsUpdate += OnChitinFragmentsUpdate;
		GameDataManager.Instance.livingSpaceDataManager.OnUsedCharactersListUpdate += UpdateUsedCharactersList;
		GameDataManager.Instance.livingSpaceDataManager.OnReservedCharactersListUpdate += UpdateReservedCharactersList;

		// ----- Set Init Value
		OnCrystalsUpdate();
		OnChitinFragmentsUpdate();
		UpdateUsedCharactersList();
		UpdateReservedCharactersList();
	}
	public override void _ExitTree()
	{
		GameDataManager.Instance.storageDataManager.OnCrystalsUpdate -= OnCrystalsUpdate;
		GameDataManager.Instance.storageDataManager.OnChitinFragmentsUpdate -= OnChitinFragmentsUpdate;
		GameDataManager.Instance.livingSpaceDataManager.OnUsedCharactersListUpdate -= UpdateUsedCharactersList;
		GameDataManager.Instance.livingSpaceDataManager.OnReservedCharactersListUpdate -= UpdateReservedCharactersList;
	}

// ---------------------------------------- Buttons ----------------------------------------
	private void OnMenuButtonPressed()
	{
		GD.Print(" -- MenuButtonButtonPressed --");

		GetTree().ChangeSceneToFile("res://Levels/Menu/MainMenu.tscn");
	}
	private void OnSettingsButtonPressed()
	{
		GD.Print(" -- SettingsButtonPressed --");
	}
	private void OnRaceMapButtonPressed()
	{
		GetTree().ChangeSceneToPacked(RaceMapScene);
	}

	private void OnTrainingPitsPressed()
	{
		var TrainingPits = GetNode<BaseInterest>("MainPanel/Control/TrainingPitsButton");
		Node sceneInstance = TrainingPits.SceneToOpen.Instantiate();

		var PanelLabel = GetNode<Label>("PopupPanel/MarginContainer/VBoxContainer/HBoxContainer2/PanelLabel");
		PanelLabel.Text = TrainingPits.Text.StripEdges();
		// ClearAndAddNewSceneToPopup(sceneInstance);

		var navigator = GetNode<PopupNavigator>("PopupPanel");
		navigator.PushInstance(sceneInstance);

		var popup = GetNode<PopupPanel>("PopupPanel");
		popup.Popup();
	}
	private void OnStoragePressed()
	{
		var Storage = GetNode<BaseInterest>("MainPanel/Control/StorageButton");
		Node sceneInstance = Storage.SceneToOpen.Instantiate();

		var PanelLabel = GetNode<Label>("PopupPanel/MarginContainer/VBoxContainer/HBoxContainer2/PanelLabel");
		PanelLabel.Text = Storage.Text.StripEdges();
		// ClearAndAddNewSceneToPopup(sceneInstance);

		var navigator = GetNode<PopupNavigator>("PopupPanel");
		navigator.PushInstance(sceneInstance);

		var popup = GetNode<PopupPanel>("PopupPanel");
		popup.Popup();
	}
	private void OnCommandBlockPressed()
	{
		var CommandBlock = GetNode<BaseInterest>("MainPanel/Control/CommandBlockButton");
		Node sceneInstance = CommandBlock.SceneToOpen.Instantiate();

		var PanelLabel = GetNode<Label>("PopupPanel/MarginContainer/VBoxContainer/HBoxContainer2/PanelLabel");
		PanelLabel.Text = CommandBlock.Text.StripEdges();
		// ClearAndAddNewSceneToPopup(sceneInstance);

		var navigator = GetNode<PopupNavigator>("PopupPanel");
		navigator.PushInstance(sceneInstance);

		var popup = GetNode<PopupPanel>("PopupPanel");
		popup.Popup();
	}
	private void OnLivingSpacePressed()
	{
		var LivingSpace = GetNode<BaseInterest>("MainPanel/Control/LivingSpaceButton");
		Node sceneInstance = LivingSpace.SceneToOpen.Instantiate();

		var PanelLabel = GetNode<Label>("PopupPanel/MarginContainer/VBoxContainer/HBoxContainer2/PanelLabel");
		PanelLabel.Text = LivingSpace.Text.StripEdges();
		// ClearAndAddNewSceneToPopup(sceneInstance);

		var navigator = GetNode<PopupNavigator>("PopupPanel");
		navigator.PushInstance(sceneInstance);

		var popup = GetNode<PopupPanel>("PopupPanel");
		popup.Popup();
	}
	private void OnForgePressed()
	{
		var Forge = GetNode<BaseInterest>("MainPanel/Control/ForgeButton");
		Node sceneInstance = Forge.SceneToOpen.Instantiate();

		var PanelLabel = GetNode<Label>("PopupPanel/MarginContainer/VBoxContainer/HBoxContainer2/PanelLabel");
		PanelLabel.Text = Forge.Text.StripEdges();
		// ClearAndAddNewSceneToPopup(sceneInstance);

		var navigator = GetNode<PopupNavigator>("PopupPanel");
		navigator.PushInstance(sceneInstance);

		var popup = GetNode<PopupPanel>("PopupPanel");
		popup.Popup();
	}

	private void OnClosePopupPressed()
	{
		var navigator = GetNode<PopupNavigator>("PopupPanel");
		navigator.ClearHistory(true);

		var popup = GetNode<PopupPanel>("PopupPanel");
		popup.Hide();

		ClearSceneContainer();
	}

	private void ClearAndAddNewSceneToPopup(Node sceneInstance)
	{
		ClearSceneContainer();

		var container = GetNode<Control>("PopupPanel/MarginContainer/VBoxContainer/SceneContainer");
		container.AddChild(sceneInstance);
	}
	private void ClearSceneContainer()
	{
		var container = GetNode<Control>("PopupPanel/MarginContainer/VBoxContainer/SceneContainer");
		foreach (Node child in container.GetChildren())
		{
			child.QueueFree();
		}
	}

	// ---------------------------------------- Reactions On Update GameData ----------------------------------------
	private void OnCrystalsUpdate() 
	{
		Label CrystalLabel = GetNode<Label>("MainPanel/Panel/VBoxContainer/ResourcePanel/VBoxContainer/CrystalBoxContainer/TextureRect/NumberLabel");
		CrystalLabel.Text = GameDataManager.Instance.currentData.storageData.Crystals.ToString();
	}
	private void OnChitinFragmentsUpdate()
	{
		Label ChitinFragmentsLabel = GetNode<Label>("MainPanel/Panel/VBoxContainer/ResourcePanel/VBoxContainer/ChitinHBoxContainer/TextureRect/NumberLabel");
		ChitinFragmentsLabel.Text = GameDataManager.Instance.currentData.storageData.ChitinFragments.ToString();
	}
	private void UpdateUsedCharactersList()
	{
		var usedCharactersVBoxContainer = GetNode<VBoxContainer>("MainPanel/Panel/VBoxContainer/ScrollContainer/VBoxContainer/TeamVBoxContainer");
		foreach (Node child in usedCharactersVBoxContainer.GetChildren())
		{
			child.QueueFree();
		}

		foreach (CharacterData characterData in GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters)
		{
			ViewWarriorPanel viewWarriorPanel = WarriorPanelScene.Instantiate() as ViewWarriorPanel;
			viewWarriorPanel.SetCharacterInfosToWarriorPanel(characterData);
			usedCharactersVBoxContainer.AddChild(viewWarriorPanel);
		}
	}
	private void UpdateReservedCharactersList()
	{
		var reservedCharactersVBoxContainer = GetNode<VBoxContainer>("MainPanel/Panel/VBoxContainer/ScrollContainer/VBoxContainer/ReserveVBoxContainer");
		foreach (Node child in reservedCharactersVBoxContainer.GetChildren())
		{
			child.QueueFree();
		}

		foreach (CharacterData characterData in GameDataManager.Instance.currentData.livingSpaceData.ReservedCharacters)
		{
			ViewWarriorPanel viewWarriorPanel = WarriorPanelScene.Instantiate() as ViewWarriorPanel;
			viewWarriorPanel.SetCharacterInfosToWarriorPanel(characterData);
			reservedCharactersVBoxContainer.AddChild(viewWarriorPanel);
		}
	}
}
