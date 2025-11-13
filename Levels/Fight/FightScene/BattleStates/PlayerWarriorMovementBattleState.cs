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

    private Tile _tileUnderMouse;
    private List<Tile> _selectedPath = []; 
    
    private double _skippingMoveTime = 0.0d;
    private bool bShouldSkip = false;
    
    private bool _isInitialized;
    
    public PlayerWarriorMovementBattleState(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        var playerEntities = FightSceneManager.Allies.Except(fightSceneManager.PlayerWarriorsThatTurned).OrderByDescending(x => x.Speed).ToList();
        if (!playerEntities.Any())
        {
            throw new ApplicationException("Нельзя перейти в это состояние с 0 воинов готовых к передвижению");
        }
        _currentPlayerWarrior = playerEntities.First();

        if (!_currentPlayerWarrior.CanAct)
        {
            bShouldSkip = true;
        }
        else
        {
            fightSceneManager.CurrentPlayerWarriorToTurn = _currentPlayerWarrior;
            StateTitleText = "Вы перемещаетесь! Выберите клетку из предложенных!";
        }
    }

    public override void InputUpdate(InputEvent @event)
    {
        if (bShouldSkip)
        {
            return;
        }
        
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
        {
            var tile = MapManager.GetTileInSelectedUnderMousePosition();
            if (tile == _currentPlayerWarrior.Tile)
            {
                FightSceneManager.CurrentBattleState = new PlayerWarriorSelectSubjectAttack(FightSceneManager, MapManager);
            }
            if (tile is not null && tile.IsAllowedToSetBattleEntity)
            {
                var moveCommand = new MoveBattleEntityCommand(_currentPlayerWarrior, MapManager, tile);
                moveCommand.Execute();
                FightSceneManager.ExecutedCommands.Add(moveCommand);
                FightSceneManager.CurrentBattleState = new PlayerWarriorSelectSubjectAttack(FightSceneManager, MapManager);
            } 
        }
    }

    public override void ProcessUpdate(double delta)
    {
        if (_tileUnderMouse != MapManager.GetTileInSelectedUnderMousePosition())
        {
            _tileUnderMouse = MapManager.GetTileInSelectedUnderMousePosition();
            if (_tileUnderMouse is not null)
            {
                foreach (var tile in _selectedPath)
                {
                    MapManager.DeselectTile(tile);
                    MapManager.SelectTileForMovement(tile);
                }

                _selectedPath = PathFinder.CalculatePathToTarget(_currentPlayerWarrior.Tile, _tileUnderMouse, MapManager, _currentPlayerWarrior);
                
                _selectedPath.ForEach(tile => MapManager.SelectTileForCurrentPlayerEntityTurn(tile));
            }
        }
        if (bShouldSkip)
        {
            _skippingMoveTime += delta;
            if (_skippingMoveTime >= 0.25d)
            {
                FightSceneManager.PlayerWarriorsThatTurned.Add(_currentPlayerWarrior);
                if (FightSceneManager.Allies.Count == FightSceneManager.PlayerWarriorsThatTurned.Count)
				{
					FightSceneManager.EnemyWarriorsThatTurned.Clear();
					FightSceneManager.PlayerWarriorsThatTurned.Clear();
					FightSceneManager.CurrentBattleState = new EnemyWarriorTurnBattleState(FightSceneManager, MapManager);
					return;
				}
                FightSceneManager.CurrentBattleState = new PlayerWarriorMovementBattleState(FightSceneManager, MapManager);
            }

            return;
        }
        
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