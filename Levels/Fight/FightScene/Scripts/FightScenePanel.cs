using Godot;
using System;
using System.Collections.Generic;

public partial class FightScenePanel : TextureRect
{
    [Export]
    public PackedScene runMapScene;
    [Export]
    public PackedScene WarriorPanelScene;
    [Export]
    public PackedScene ChosenWarriorPanelScene;
    [Export]
    private PackedScene PopupNavigatorScene;
    
    public override void _Ready()
    {
        PopupNavigator popupNavigator = PopupNavigatorScene.Instantiate() as PopupNavigator;
        AddChild(popupNavigator);

        TextureButton escapeButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer/TextureButton");
        escapeButton.Pressed += OnEscapeButtonPressed;

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

    private double timer = 0d;
    public override void _Process(double delta)
    {
        base._Process(delta);

        timer += delta;
        if (timer >= 10d)
        {
            timer = 0d;
            UpdateUsedCharactersList();
        }
    }


    private void OnEscapeButtonPressed()
    {
        GameDataManager.Instance.runMapDataManager.PassMapNode();
        SceneTree sceneTree = Engine.GetMainLoop() as SceneTree;
        sceneTree.ChangeSceneToPacked(runMapScene);
    }
    private void OnCrystalsUpdate() 
	{
		Label CrystalLabel = GetNode<Label>("VBoxContainer/ResourcePanel/VBoxContainer/CrystalBoxContainer/TextureRect/NumberLabel");
		CrystalLabel.Text = GameDataManager.Instance.currentData.storageData.Crystals.ToString();
	}
	private void OnChitinFragmentsUpdate()
	{
		Label ChitinFragmentsLabel = GetNode<Label>("VBoxContainer/ResourcePanel/VBoxContainer/ChitinHBoxContainer/TextureRect/NumberLabel");
		ChitinFragmentsLabel.Text = GameDataManager.Instance.currentData.storageData.ChitinFragments.ToString();
	}
	private void UpdateUsedCharactersList()
	{
		var usedCharactersVBoxContainer = GetNode<VBoxContainer>("VBoxContainer/ScrollContainer/VBoxContainer/TeamVBoxContainer");
		foreach (Node child in usedCharactersVBoxContainer.GetChildren())
		{
			child.QueueFree();
		}

        /* foreach (CharacterData characterData in GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters)
        {
            ViewWarriorPanel viewWarriorPanel = WarriorPanelScene.Instantiate() as ViewWarriorPanel;
            viewWarriorPanel.bShouldChangeCharacterList = false;
            viewWarriorPanel.SetCharacterInfosToWarriorPanel(characterData);
            usedCharactersVBoxContainer.AddChild(viewWarriorPanel);
        }*/

        List<CharacterData> team = GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters;
        int randomValue = GD.RandRange(0, team.Count - 1);
        for (int i = 0; i < team.Count; ++i)
        {
            if (i == randomValue)
            {
                ChosenWarriorPanel chosenWarriorPanel = ChosenWarriorPanelScene.Instantiate() as ChosenWarriorPanel;
                chosenWarriorPanel.SetCharacterInfos(team[i]);
                usedCharactersVBoxContainer.AddChild(chosenWarriorPanel);
            }
            else
            {
                ViewWarriorPanel viewWarriorPanel = WarriorPanelScene.Instantiate() as ViewWarriorPanel;
                viewWarriorPanel.bShouldChangeCharacterList = false;
                viewWarriorPanel.SetCharacterInfosToWarriorPanel(team[i]);
                usedCharactersVBoxContainer.AddChild(viewWarriorPanel);
            }
        }
	}
}
