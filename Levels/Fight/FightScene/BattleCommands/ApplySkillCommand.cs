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
        if (_battleEntity is PlayerEntity playerEntityActive)
        {
            if (playerEntityActive.activeSkill != null)
            {
                GD.Print($"чувачок с Id-{_battleEntity.Id} применил скилл");
                
                if (playerEntityActive.activeSkill.IsHasEffect())
                {
                    if (playerEntityActive.activeSkill.effect.TargetType == EEffectTarget.Self)
                    {
                        if (playerEntityActive.activeSkill.effect is EntityEffect entityEffect)
                        {
                            entityEffect.entityHolder = playerEntityActive;
                            playerEntityActive.AddEffect(playerEntityActive.activeSkill.effect);
                            return;
                        }
                    }
                }
            } 
        }

        foreach (var tile in _tilesForAttack)
        {
            if (_battleEntity is PlayerEntity playerEntity)
            {
                if (playerEntity.activeSkill == null) continue;

                if (playerEntity.activeSkill.IsHasEffect())
                {
                    if (tile.BattleEntity != null && playerEntity.activeSkill.effect.TargetType == EEffectTarget.Enemy)
                    {
                        if (playerEntity.activeSkill.effect is EntityEffect entityEffect)
                        {
                            if (playerEntity.activeSkill.effect is SevereWoundEntityEffect severeWoundEntityEffect)
                            {
                                severeWoundEntityEffect.DamageFromInstigator = playerEntity.Damage / 4;
                            }
                            else if (playerEntity.activeSkill.effect is BloodMarkEntityEffect bloodMarkEntityEffect)
                            {
                                bloodMarkEntityEffect.DamageFromInstigator = playerEntity.DamageByEffect * 2;
                            }
                            entityEffect.entityHolder = tile.BattleEntity;
                            tile.BattleEntity.AddEffect(playerEntity.activeSkill.effect);
                            GD.Print($"чувачок с Id-{_battleEntity.Id} применил скилл по тайлу {tile}");
                        }
                    }
                }
                else
                {
                    if (tile.BattleEntity == null) continue;

                    if (playerEntity.activeSkill is SilentMeditationsActiveSkill silentMeditationsActiveSkill)
                    {
                        tile.BattleEntity.ApplyHeal(playerEntity, tile.BattleEntity.Heal);
                    }
                    else if (playerEntity.activeSkill is RemoveEffectsActiveSkill removeEffectsActiveSkill)
                    {
                        List<Effect> effectsToDelete = new List<Effect>();
                        foreach (Effect effect in tile.BattleEntity.appliedEffects)
                        {
                            if (EffectExtensions.IsNegative(effect.EffectType))
                            {
                                effectsToDelete.Add(effect);
                            }
                        }
                        foreach (Effect effect in effectsToDelete)
                        {
                            tile.BattleEntity.RemoveEffect(effect);
                        }
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