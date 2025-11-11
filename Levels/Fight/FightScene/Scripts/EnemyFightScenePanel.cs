using Godot;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EnemyFightScenePanel : TextureRect
{
    [Export]
    public PackedScene WarriorEnemyPanelScene;
    public override void _Ready()
    {
        FightSceneManager fightSceneManager = GetTree().CurrentScene as FightSceneManager;
        if (fightSceneManager != null)
        {
            fightSceneManager.FightSceneManagerInitialized += OnFightSceneManagerInitialized;
            fightSceneManager.BattleEntityMadeMove += UpdateEnemyCharactersList;
        }
    }

    private void UpdateEnemyCharactersList()
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

        List<EnemyEntity> enemyEntities = fightSceneManager.Enemies.ToList();
        foreach (EnemyEntity enemy in enemyEntities)
        {
            ViewEnemyWarriorPanel viewWarriorPanel = WarriorEnemyPanelScene.Instantiate() as ViewEnemyWarriorPanel;
            viewWarriorPanel.SetEnemyInfosToWarriorEnemyPanel(enemy);
            viewWarriorPanel.EnemyPanelMouseEnter += fightSceneManager.OnEnemyPanelMouseEnter;
            viewWarriorPanel.EnemyPanelMouseExit += fightSceneManager.OnEnemyPanelMouseExit;
            usedCharactersVBoxContainer.AddChild(viewWarriorPanel);
        }
    }

    private void OnFightSceneManagerInitialized()
    {
        UpdateEnemyCharactersList();
    }

    public Vector2 GetEnemyPanelPositionByID(string enemyID)
    {
        var usedCharactersVBoxContainer = GetNode<VBoxContainer>("VBoxContainer/ScrollContainer/VBoxContainer/TeamVBoxContainer");
        foreach (Node child in usedCharactersVBoxContainer.GetChildren())
        {
            if (child is ViewEnemyWarriorPanel viewEnemyWarriorPanel)
            {
                if (viewEnemyWarriorPanel.GetEnemyEntity().Id == enemyID)
                {
                    return viewEnemyWarriorPanel.GetGlobalRect().Position;
                }
            }
        }

        return new Vector2();
    }
}