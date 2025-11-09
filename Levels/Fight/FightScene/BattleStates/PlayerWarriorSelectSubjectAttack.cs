using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public class PlayerWarriorSelectSubjectAttack : BattleState
{
    private PlayerEntity _currentPlayerWarrior;
    public PlayerWarriorSelectSubjectAttack(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        MapManager.ClearAllSelectedTiles();

        _currentPlayerWarrior = fightSceneManager.CurrentPlayerWarriorToTurn;

        FightScenePanel fightScenePanel = fightSceneManager.GetNode<FightScenePanel>("HBoxContainer/FightScenePanel");
        fightScenePanel.chosenWarriorPanel.PlayerChoseWeapon += OnPlayerChoseWeapon;
        fightScenePanel.chosenWarriorPanel.PlayerChoseSkill += OnPlayerChoseSkill;
        fightScenePanel.chosenWarriorPanel.EnableButtonsForSelectingSubjectAttack();
        fightScenePanel.chosenWarriorPanel.bShouldShowPlayerNeedSelect = true;
        
        StateTitleText = "Выберете чем будете атаковать! Оружием или навыком?";
    }

    public override void InputUpdate(InputEvent @event)
    {
    }

    public override void ProcessUpdate(double delta)
    {
        
    }

    private void OnPlayerChoseWeapon(string characterID)
    {
        if (characterID == _currentPlayerWarrior.Id)
        {
            GD.Print(_currentPlayerWarrior.CharacterName + " ходит оружием");

            FightSceneManager.CurrentBattleState = new PlayerWarriorAttackChoosingBattleState(FightSceneManager, MapManager);

            FightScenePanel fightScenePanel = FightSceneManager.GetNode<FightScenePanel>("HBoxContainer/FightScenePanel");
            fightScenePanel.chosenWarriorPanel.PlayerChoseWeapon -= OnPlayerChoseWeapon;
            fightScenePanel.chosenWarriorPanel.PlayerChoseSkill -= OnPlayerChoseSkill;
            fightScenePanel.chosenWarriorPanel.DisableButtonsForSelectingSubjectAttack();
            fightScenePanel.chosenWarriorPanel.bShouldShowPlayerNeedSelect = false;
        }
    }
    private void OnPlayerChoseSkill(string characterID)
    {
        if (characterID == _currentPlayerWarrior.Id)
        {
            GD.Print(_currentPlayerWarrior.CharacterName + " ходит навыком");

            FightSceneManager.CurrentBattleState = new PlayerWarriorSkillChoosingBattleState(FightSceneManager, MapManager);

            FightScenePanel fightScenePanel = FightSceneManager.GetNode<FightScenePanel>("HBoxContainer/FightScenePanel");
            fightScenePanel.chosenWarriorPanel.PlayerChoseWeapon -= OnPlayerChoseWeapon;
            fightScenePanel.chosenWarriorPanel.PlayerChoseSkill -= OnPlayerChoseSkill;
            fightScenePanel.chosenWarriorPanel.DisableButtonsForSelectingSubjectAttack();
            fightScenePanel.chosenWarriorPanel.bShouldShowPlayerNeedSelect = false;
        }
    }
}
