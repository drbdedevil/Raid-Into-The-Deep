using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;

public class AttackByWeaponCommand : Command
{
    private BattleEntity _battleEntity { get; set; }
    private List<Tile>  _tilesForAttack { get; set; }
    
    public AttackByWeaponCommand(BattleEntity battleEntity, List<Tile> tilesForAttack)
    {
        _battleEntity = battleEntity;
        _tilesForAttack = tilesForAttack;
    }
    
    public override void Execute()
    {
        foreach (var tile in _tilesForAttack)
        {
            GD.Print($"чувачок с Id-{ _battleEntity.Id} ударил по тайлу {tile}");
        }
    }

    public override void UnExecute()
    {
        foreach (var tile in _tilesForAttack)
        {
            GD.Print($"чувачок с Id-{ _battleEntity.Id} отменил удар по тайлу {tile}");
        }
    }
}