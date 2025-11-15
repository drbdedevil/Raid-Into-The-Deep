using Godot;
using System;

public partial class RaceMap : Control
{
    // [Export]
    // private PackedScene HubLocationScene;
    [Export]
    public PackedScene WarriorPanelScene;
	[Export]
	private PackedScene PopupNavigatorScene;

	private bool bShouldGoUp = false;

    public override void _Ready()
    {
        PopupNavigator popupNavigator = PopupNavigatorScene.Instantiate() as PopupNavigator;
        AddChild(popupNavigator);

        TextureButton HubLocationButton = GetNode<TextureButton>("HBoxContainer/Panel/VBoxContainer/HBoxContainer/TextureButton");
		HubLocationButton.ButtonDown += OnHubLocationButtonPressed;

		TextureButton regenerateButton = GetNode<TextureButton>("HBoxContainer/ColorRect/MarginContainer2/TextureButton");
		regenerateButton.Pressed += OnGenerateNewMapButtonPressed;

        // ----------- View Realization -----------
		// ----- Binding Functions
		GameDataManager.Instance.storageDataManager.OnCrystalsUpdate += OnCrystalsUpdate;
		GameDataManager.Instance.storageDataManager.OnChitinFragmentsUpdate += OnChitinFragmentsUpdate;
		GameDataManager.Instance.livingSpaceDataManager.OnUsedCharactersListUpdate += UpdateUsedCharactersList;
		GameDataManager.Instance.runMapDataManager.OnBossWasDefeated += CheckBossWasDefeated;
        
        // ----- Set Init Value
		OnCrystalsUpdate();
		OnChitinFragmentsUpdate();
		UpdateUsedCharactersList();

		CheckBossWasDefeated();

		SoundManager.Instance.RemoveAllSounds();
		SoundManager.Instance.PlaySoundLoop("res://Sound/Music/RaceMap/KartaAllInOne.wav", 0.1f);
    }
	public override void _ExitTree()
	{
		GameDataManager.Instance.storageDataManager.OnCrystalsUpdate -= OnCrystalsUpdate;
		GameDataManager.Instance.storageDataManager.OnChitinFragmentsUpdate -= OnChitinFragmentsUpdate;
		GameDataManager.Instance.livingSpaceDataManager.OnUsedCharactersListUpdate -= UpdateUsedCharactersList;
		GameDataManager.Instance.runMapDataManager.OnBossWasDefeated -= CheckBossWasDefeated;
	}

    public override void _Process(double delta)
    {
		base._Process(delta);
		
		if (bShouldGoUp)
        {
			ScrollContainer scrollContainer = GetNode<ScrollContainer>("HBoxContainer/ColorRect/MapContainer");
			scrollContainer.ScrollVertical = (int)Mathf.Lerp(scrollContainer.ScrollVertical, 0f, (float)delta * 2f);

			if (scrollContainer.ScrollVertical == 0)
            {
				EnableInput();
            }
        }
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

	private void CheckBossWasDefeated()
	{
		if (GameDataManager.Instance.currentData.runMapData.bShouldShowRegenerateButton)
		{
			TextureButton regenerateButton = GetNode<TextureButton>("HBoxContainer/ColorRect/MarginContainer2/TextureButton");
			regenerateButton.Visible = true;
		}
	}
	private void OnGenerateNewMapButtonPressed()
	{
		TextureButton regenerateButton = GetNode<TextureButton>("HBoxContainer/ColorRect/MarginContainer2/TextureButton");
		regenerateButton.Visible = false;

		GameDataManager.Instance.currentData.runMapData.bShouldRegenerate = true;
		GameDataManager.Instance.runMapDataManager.EmitSignal(RunMapDataManager.SignalName.OnRunMapListUpdate);

		GameDataManager.Instance.currentData.runMapData.bShouldShowRegenerateButton = false;

		DisableInput();
	}

	private void DisableInput()
	{
		bShouldGoUp = true;
		GetViewport().GuiDisableInput = true;
	}
	private void EnableInput()
    {
        bShouldGoUp = false;
		GetViewport().GuiDisableInput = false;
    }
}
