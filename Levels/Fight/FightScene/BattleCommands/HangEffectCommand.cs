using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;

public class HangEffectCommand : Command
{
    private BattleEntity _battleEntity { get; set; }
    private EntityEffect _effectToHang { get; set; }
    
    public HangEffectCommand(BattleEntity battleEntity, EntityEffect effectToHang)
    {
        _battleEntity = battleEntity;
        _effectToHang = effectToHang;
    }
    
    public override void Execute()
    {
        _battleEntity.AddEffect(new EntityEffect());
    }

    public override void UnExecute()
    {
        _battleEntity.RemoveEffect(new EntityEffect());
    }
}