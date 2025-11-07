using Godot;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;

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

    [Signal]
    public delegate void WarriorPanelsUpdateEventHandler();
    private string currentCharacterID = "NONE";

    public ChosenWarriorPanel chosenWarriorPanel = new();
    
    public override void _Ready()
    {
        PopupNavigator popupNavigator = PopupNavigatorScene.Instantiate() as PopupNavigator;
        AddChild(popupNavigator);

        TextureButton escapeButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer/TextureButton");
        escapeButton.Pressed += OnEscapeButtonPressed;

        FightSceneManager fightSceneManager = GetTree().CurrentScene as FightSceneManager;
        if (fightSceneManager != null)
        {
            fightSceneManager.CurrentPlayerWarriorToTurnChanged += OnCurrentPlayerWarriorToTurnChanged;
            // WarriorPanelsUpdate += fightSceneManager.OnWarriorPanelsUpdate;
        }

        // ----------- View Realization -----------
        // ----- Binding Functions
        GameDataManager.Instance.storageDataManager.OnCrystalsUpdate += OnCrystalsUpdate;
        GameDataManager.Instance.storageDataManager.OnChitinFragmentsUpdate += OnChitinFragmentsUpdate;
        GameDataManager.Instance.livingSpaceDataManager.OnUsedCharactersListUpdate += UpdateUsedCharactersList;

        // ----- Set Init Value
		OnCrystalsUpdate();
		OnChitinFragmentsUpdate();
		// UpdateUsedCharactersList();
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
        return;

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
        FightSceneManager fightSceneManager = GetTree().CurrentScene as FightSceneManager;
        if (fightSceneManager == null)
        {
            return;
        }

        var usedCharactersVBoxContainer = GetNode<VBoxContainer>("VBoxContainer/ScrollContainer/VBoxContainer/TeamVBoxContainer");
        foreach (Node child in usedCharactersVBoxContainer.GetChildren())
        {
            child.QueueFree();
        }

        List<PlayerEntity> playerEntities = fightSceneManager.Allies.ToList();
        List<CharacterData> team = GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.Where(x => playerEntities.Select(e => e.Id).Contains(x.ID)).ToList();
        for (int i = 0; i < team.Count; ++i)
        {
            if (team[i].ID == currentCharacterID)
            {
                chosenWarriorPanel = ChosenWarriorPanelScene.Instantiate() as ChosenWarriorPanel;
                chosenWarriorPanel.SetCharacterInfos(team[i]);
                chosenWarriorPanel.DisableButtonsForSelectingSubjectAttack();
                usedCharactersVBoxContainer.AddChild(chosenWarriorPanel);
            }
            else
            {
                ViewWarriorPanel viewWarriorPanel = WarriorPanelScene.Instantiate() as ViewWarriorPanel;
                viewWarriorPanel.bShouldChangeCharacterList = false;
                viewWarriorPanel.SetCharacterInfosToWarriorPanel(team[i], true);
                usedCharactersVBoxContainer.AddChild(viewWarriorPanel);
            }
        }
        EmitSignal(SignalName.WarriorPanelsUpdate);
    }

    public void OnCurrentPlayerWarriorToTurnChanged(string playerWarriorId)
    {
        currentCharacterID = playerWarriorId;
        UpdateUsedCharactersList();
    }
}
