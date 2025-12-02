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
            fightSceneManager.EnemyWarriorToTurnChanged += UpdateUsedCharactersList;
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
        GameDataManager.Instance.commandBlockDataManager.ApplyPunishmentForEscape();
        GameDataManager.Instance.trainingPitsDataManager.CheckAndGenerateCharactersPresenceInGame();

        GameDataManager.Instance.Save();

        SceneTree sceneTree = Engine.GetMainLoop() as SceneTree;
        sceneTree.ChangeSceneToPacked(runMapScene);

        SoundManager.Instance.PlaySoundOnce("res://Sound/FleeFromBattle.wav", 0.7f);
    }
    private void OnCrystalsUpdate() 
	{
		Label CrystalLabel = GetNode<Label>("VBoxContainer/ResourcePanel/VBoxContainer/CrystalBoxContainer/NumberLabel");
		CrystalLabel.Text = GameDataManager.Instance.currentData.storageData.Crystals.ToString();
	}
	private void OnChitinFragmentsUpdate()
	{
		Label ChitinFragmentsLabel = GetNode<Label>("VBoxContainer/ResourcePanel/VBoxContainer/ChitinHBoxContainer/NumberLabel");
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
                chosenWarriorPanel.WarriorPanelMouseEnter += fightSceneManager.OnCharacterPanelMouseEnter;
                chosenWarriorPanel.WarriorPanelMouseExit += fightSceneManager.OnCharacterPanelMouseExit;
                usedCharactersVBoxContainer.AddChild(chosenWarriorPanel);
            }
            else
            {
                ViewWarriorPanel viewWarriorPanel = WarriorPanelScene.Instantiate() as ViewWarriorPanel;
                viewWarriorPanel.bShouldChangeCharacterList = false;
                viewWarriorPanel.bShouldShowThatCharacterHasSkillPoints = false;
                viewWarriorPanel.SetCharacterInfosToWarriorPanel(team[i], true);
                viewWarriorPanel.WarriorPanelMouseEnter += fightSceneManager.OnCharacterPanelMouseEnter;
                viewWarriorPanel.WarriorPanelMouseExit += fightSceneManager.OnCharacterPanelMouseExit;
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

    public Vector2 GetWarriorPanelPositionByID(string warriorID)
    {
        var usedCharactersVBoxContainer = GetNode<VBoxContainer>("VBoxContainer/ScrollContainer/VBoxContainer/TeamVBoxContainer");
        foreach (Node child in usedCharactersVBoxContainer.GetChildren())
        {
            if (child is ViewWarriorPanel viewWarriorPanel)
            {
                if (viewWarriorPanel.GetCharacterID() == warriorID)
                {
                    return viewWarriorPanel.GetGlobalRect().Position;
                }
            }
            else if (child is ChosenWarriorPanel chosenWarriorPanel)
            {
                if (chosenWarriorPanel.warrior.characterData.ID == warriorID)
                {
                    return chosenWarriorPanel.GetGlobalRect().Position;
                }
            }
        }

        return new Vector2();
    }
}
