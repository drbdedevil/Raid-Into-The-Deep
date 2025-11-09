using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public class EnemyWarriorTurnBattleState : BattleState
{
    private EnemyEntity _currentEnemyWarrior;
    private List<Tile> _tilesToMove;
    private List<Tile> _tilesToAttack;

    private Task _drawingTilesToMoveTask;
    private Task _drawingTilesToAttackTask;

    private TargetResult _targetResult = null;
    
    public EnemyWarriorTurnBattleState(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        StateTitleText = "Теперь ходит враг. Какая досада!";
        var enemyEntities = FightSceneManager.Enemies.Except(fightSceneManager.EnemyWarriorsThatTurned).OrderByDescending(x => x.Speed).ToList();
        if (!enemyEntities.Any())
        {
            throw new ApplicationException("Нельзя перейти в это состояние с 0 воинов готовых к передвижению");
        }
        _currentEnemyWarrior = enemyEntities.First();
        _tilesToMove = PathFinder.FindTilesToMove(_currentEnemyWarrior.Tile, mapManager, _currentEnemyWarrior.Speed);
        MapManager.DrawEnemyEntityTilesToMove(_tilesToMove);
        _drawingTilesToMoveTask = Task.Delay(2000);
        
    }

    public override void InputUpdate(InputEvent @event)
    {
    }

    public override void ProcessUpdate(double delta)
    {
        if (!_drawingTilesToMoveTask.IsCompleted) return;
        
        _targetResult ??= FindTargetToAttack();
        
        if (_drawingTilesToAttackTask is not null && !_drawingTilesToAttackTask.IsCompleted) return; 
        
        if (_targetResult.PlayerEntityToAttack is not null)
        {
            MapManager.ClearDrawEnemyEntityTilesToAttack(_tilesToAttack);
            FightSceneManager.EnemyWarriorsThatTurned.Add(_currentEnemyWarrior);
            if (FightSceneManager.Enemies.Count == FightSceneManager.EnemyWarriorsThatTurned.Count)
            {
                FightSceneManager.EnemyWarriorsThatTurned.Clear();
                FightSceneManager.PlayerWarriorsThatTurned.Clear();
                FightSceneManager.CurrentBattleState = new ApplyingEffectsBattleState(FightSceneManager, MapManager);
                return;
            }
            FightSceneManager.CurrentBattleState = new EnemyWarriorTurnBattleState(FightSceneManager, MapManager);
        }
        else
        {
            MapManager.ClearDrawEnemyEntityTilesToMove(_tilesToMove);
            FightSceneManager.EnemyWarriorsThatTurned.Add(_currentEnemyWarrior);
            if (FightSceneManager.Enemies.Count == FightSceneManager.EnemyWarriorsThatTurned.Count)
            {
                FightSceneManager.EnemyWarriorsThatTurned.Clear();
                FightSceneManager.PlayerWarriorsThatTurned.Clear();
                FightSceneManager.CurrentBattleState = new ApplyingEffectsBattleState(FightSceneManager, MapManager);
                return;
            }
            FightSceneManager.CurrentBattleState = new EnemyWarriorTurnBattleState(FightSceneManager, MapManager);
        }
    }

    private TargetResult FindTargetToAttack()
    {
        PlayerEntity targetToAttack = null;

        List<TileDistanceToAnyPlayerWarriorPair> tileDistances = [];
        foreach (var playerEntity in FightSceneManager.Allies)
        {
            foreach (var tileToMove in _tilesToMove)
            {
                tileDistances.Add(new TileDistanceToAnyPlayerWarriorPair(tileToMove, PathFinder.CalculateDistanceToTile(tileToMove, playerEntity.Tile)));
                var tilesToAttack = PathFinder.FindTilesToAttack(_currentEnemyWarrior, playerEntity.Tile, MapManager, tileToMove);
                if (tilesToAttack.Select(x => x.BattleEntity).Contains(playerEntity))
                {
                    _tilesToAttack = tilesToAttack;
                    MapManager.ClearDrawEnemyEntityTilesToMove(_tilesToMove);
                    MapManager.DrawEnemyEntityTilesToAttack(_tilesToAttack);
                    var command = new MoveBattleEntityCommand(_currentEnemyWarrior, MapManager, tileToMove);
                    command.Execute();
                    _drawingTilesToAttackTask = Task.Delay(2000);
                    return new (playerEntity, tileToMove);
                }
            }
        }

        var nearestTile = tileDistances.OrderBy(x => x.DistanceToPlayerEntity).FirstOrDefault()?.Tile;
        MapManager.ClearDrawEnemyEntityTilesToMove(_tilesToMove);
        var moveBattleEntityCommand = new MoveBattleEntityCommand(_currentEnemyWarrior, MapManager, nearestTile);
        moveBattleEntityCommand.Execute();
        return new TargetResult(null, null);
    }
}

class TargetResult
{
    public PlayerEntity PlayerEntityToAttack;
    public Tile TileToMove;

    public TargetResult(PlayerEntity playerEntityToAttack, Tile tileToMove)
    {
        PlayerEntityToAttack = playerEntityToAttack;
        TileToMove = tileToMove;
    }
}

class TileDistanceToAnyPlayerWarriorPair
{
    public Tile Tile;
    public int DistanceToPlayerEntity;

    public TileDistanceToAnyPlayerWarriorPair(Tile tile, int distanceToPlayerEntity)
    {
        Tile = tile;
        DistanceToPlayerEntity = distanceToPlayerEntity;
    }
}