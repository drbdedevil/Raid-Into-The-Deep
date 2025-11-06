using System;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;

public class MoveBattleEntityCommand : Command
{
    private Tile _previousTile;
    private BattleEntity _battleEntityToMove;
    private MapManager _mapManager;
    private Tile _tileTarget;

    public MoveBattleEntityCommand(BattleEntity battleEntityToMove, MapManager mapManager, Tile tileTarget)
    {
        _battleEntityToMove = battleEntityToMove;
        _mapManager = mapManager;
        _tileTarget = tileTarget;
        _previousTile = battleEntityToMove.Tile;
    }
    
    public override void Execute()
    {
        if (_tileTarget is null) throw new ApplicationException("Target tile is null");
        if (_previousTile == _tileTarget) throw new ApplicationException("Нельзя так перемещаться");
        if (_tileTarget.BattleEntity is not null) throw new ApplicationException("Tile уже занят");
        
        _mapManager.SetBattleEntityOnTile(_tileTarget, _battleEntityToMove);
    }
    
    public override void UnExecute()
    {
        if (_previousTile is null) throw new ApplicationException("Previous tile is null");
        
        _mapManager.SetBattleEntityOnTile(_previousTile, _battleEntityToMove);
        
    }
}