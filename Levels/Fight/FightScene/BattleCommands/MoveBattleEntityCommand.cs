using System;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;

public class MoveBattleEntityCommand : Command
{
    private Tile _previousTile;
    private BattleEntity _battleEntityToMove;
    private MapManager _mapManager;
    private FightSceneManager _fightSceneManager;
    private Tile _tileTarget;

    public MoveBattleEntityCommand(BattleEntity battleEntityToMove, MapManager mapManager, FightSceneManager fightSceneManager, Tile tileTarget)
    {
        _battleEntityToMove = battleEntityToMove;
        _mapManager = mapManager;
        _fightSceneManager = fightSceneManager;
        _tileTarget = tileTarget;
        _previousTile = battleEntityToMove.Tile;
    }
    
    public override void Execute()
    {
        if (_tileTarget is null) throw new ApplicationException("Target tile is null");
        if (_tileTarget.BattleEntity is not null && !Equals(_tileTarget.BattleEntity, _battleEntityToMove)) throw new ApplicationException("Tile уже занят");
        
        var path = PathFinder.CalculatePathToTarget(_previousTile, _tileTarget, _mapManager, _battleEntityToMove);
        foreach (var tile in path)
        {
            if (tile.ObstacleEntity is not null && tile.ObstacleEntity.ImposedEffect is not null)
            {
                var hangEffectCommand = new HangEffectCommand(_battleEntityToMove, tile.ObstacleEntity.ImposedEffect);
                hangEffectCommand.Execute();
                _fightSceneManager.ExecutedCommands.Add(hangEffectCommand);
            }
        }
        _mapManager.SetBattleEntityOnTile(_tileTarget, _battleEntityToMove);
    }
    
    public override void UnExecute()
    {
        if (_previousTile is null) throw new ApplicationException("Previous tile is null");
        
        _mapManager.SetBattleEntityOnTile(_previousTile, _battleEntityToMove);
        
    }
}