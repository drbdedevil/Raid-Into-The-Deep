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
        _effectToHang.entityHolder = _battleEntity;
    }
    
    public override void Execute()
    {
        GD.Print($"Получил эффект - {_effectToHang.EffectType}");
        _battleEntity.AddEffect(_effectToHang);
    }

    public override void UnExecute()
    {
        GD.Print($"Убрал эффект - {_effectToHang.EffectType}");
        _battleEntity.RemoveEffect(_effectToHang);
    }
}