using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public class PlayerWarriorMovementBattleState : BattleState
{
    private PlayerEntity _currentPlayerWarrior;
    
    private bool _isInitialized;
    
    public PlayerWarriorMovementBattleState(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        var playerEntities = FightSceneManager.Allies.Except(fightSceneManager.PlayerWarriorsThatTurned).OrderByDescending(x => x.Speed).ToList();
        if (!playerEntities.Any())
        {
            throw new ApplicationException("Нельзя перейти в это состояние с 0 воинов готовых к передвижению");
        }
        _currentPlayerWarrior = playerEntities.First();
        fightSceneManager.CurrentPlayerWarriorToTurn = _currentPlayerWarrior;
    }

    public override void InputUpdate(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
        {
            var tile = MapManager.GetTileInSelectedUnderMousePosition();
            if (tile is not null && tile.IsAllowedToSetBattleEntity)
            {
                var moveCommand = new MoveBattleEntityCommand(_currentPlayerWarrior, MapManager, tile);
                moveCommand.Execute();
                FightSceneManager.ExecutedCommands.Add(moveCommand);
                //MapManager.ClearAllSelectedTiles();
                FightSceneManager.CurrentBattleState = new PlayerWarriorSelectSubjectAttack(FightSceneManager, MapManager);
            } 
        }
    }

    public override void ProcessUpdate(double delta)
    {
        InitSpeedZone();
    }

    private void InitSpeedZone()
    {
        if (!_isInitialized)
        {
            MapManager.CalculateAndDrawPlayerEntitySpeedZone(_currentPlayerWarrior);
            _isInitialized = true;
        }
    }
}