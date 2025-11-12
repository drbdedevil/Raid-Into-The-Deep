using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;

public class ApplySkillCommand : Command
{
    private BattleEntity _battleEntity { get; set; }
    private List<Tile>  _tilesForAttack { get; set; }
    
    public ApplySkillCommand(BattleEntity battleEntity, List<Tile> tilesForAttack)
    {
        _battleEntity = battleEntity;
        _tilesForAttack = tilesForAttack;
    }
    
    public override void Execute()
    {
        foreach (var tile in _tilesForAttack)
        {
            if (_battleEntity is PlayerEntity playerEntity)
            {
                if (playerEntity.activeSkill == null) continue;
                GD.Print($"чувачок с Id-{_battleEntity.Id} применил скилл по тайлу {tile}");
                
                if (playerEntity.activeSkill.IsHasEffect())
                {
                    if (playerEntity.activeSkill.effect.TargetType == EEffectTarget.Self)
                    {
                        playerEntity.AddEffect(playerEntity.activeSkill.effect);
                    }
                }
            }
        }
    }

    public override void UnExecute()
    {
        foreach (var tile in _tilesForAttack)
        {
            GD.Print($"чувачок с Id-{_battleEntity.Id} применил скилл по тайлу {tile}");
        }
    }
}