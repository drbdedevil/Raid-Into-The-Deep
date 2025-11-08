using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;

public class HangEffectCommand : Command
{
    private BattleEntity _battleEntity { get; set; }
    private List<Tile>  _tilesForAttack { get; set; }
    
    public HangEffectCommand(BattleEntity battleEntity, List<Tile> tilesForAttack)
    {
        _battleEntity = battleEntity;
        _tilesForAttack = tilesForAttack;
    }
    
    public override void Execute()
    {
        foreach (var tile in _tilesForAttack)
        {
            GD.Print($"чувачок с Id-{ _battleEntity.Id} применил скилл по тайлу {tile}");
        }
    }

    public override void UnExecute()
    {
        foreach (var tile in _tilesForAttack)
        {
            GD.Print($"чувачок с Id-{ _battleEntity.Id} применил скилл по тайлу {tile}");
        }
    }
}