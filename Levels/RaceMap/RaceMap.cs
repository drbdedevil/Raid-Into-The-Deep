using Godot;
using System;

public partial class RaceMap : Control
{
    // [Export]
    // private PackedScene HubLocationScene;
    [Export]
	public PackedScene WarriorPanelScene;

    public override void _Ready()
    {
        TextureButton HubLocationButton = GetNode<TextureButton>("HBoxContainer/Panel/VBoxContainer/HBoxContainer/TextureButton");
        HubLocationButton.ButtonDown += OnHubLocationButtonPressed;

        // ----------- View Realization -----------
		// ----- Binding Functions
		GameDataManager.Instance.storageDataManager.OnCrystalsUpdate += OnCrystalsUpdate;
		GameDataManager.Instance.storageDataManager.OnChitinFragmentsUpdate += OnChitinFragmentsUpdate;
        GameDataManager.Instance.livingSpaceDataManager.OnUsedCharactersListUpdate += UpdateUsedCharactersList;
        
        // ----- Set Init Value
		OnCrystalsUpdate();
		OnChitinFragmentsUpdate();
		UpdateUsedCharactersList();
    }
    public override void _ExitTree()
	{
		GameDataManager.Instance.storageDataManager.OnCrystalsUpdate -= OnCrystalsUpdate;
		GameDataManager.Instance.storageDataManager.OnChitinFragmentsUpdate -= OnChitinFragmentsUpdate;
		GameDataManager.Instance.livingSpaceDataManager.OnUsedCharactersListUpdate -= UpdateUsedCharactersList;
	}

    private void OnHubLocationButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Levels/HubLocation/HubLocation.tscn");
    }
    
    private void OnCrystalsUpdate() 
	{
		Label CrystalLabel = GetNode<Label>("HBoxContainer/Panel/VBoxContainer/ResourcePanel/VBoxContainer/CrystalBoxContainer/TextureRect/NumberLabel");
		CrystalLabel.Text = GameDataManager.Instance.currentData.storageData.Crystals.ToString();
	}
	private void OnChitinFragmentsUpdate()
	{
		Label ChitinFragmentsLabel = GetNode<Label>("HBoxContainer/Panel/VBoxContainer/ResourcePanel/VBoxContainer/ChitinHBoxContainer/TextureRect/NumberLabel");
		ChitinFragmentsLabel.Text = GameDataManager.Instance.currentData.storageData.ChitinFragments.ToString();
	}
	private void UpdateUsedCharactersList()
	{
		var usedCharactersVBoxContainer = GetNode<VBoxContainer>("HBoxContainer/Panel/VBoxContainer/ScrollContainer/VBoxContainer/TeamVBoxContainer");
		foreach (Node child in usedCharactersVBoxContainer.GetChildren())
		{
			child.QueueFree();
		}

		foreach (CharacterData characterData in GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters)
		{
            ViewWarriorPanel viewWarriorPanel = WarriorPanelScene.Instantiate() as ViewWarriorPanel;
            viewWarriorPanel.bShouldChangeCharacterList = false;
			viewWarriorPanel.SetCharacterInfosToWarriorPanel(characterData);
			usedCharactersVBoxContainer.AddChild(viewWarriorPanel);
		}
	}
}
