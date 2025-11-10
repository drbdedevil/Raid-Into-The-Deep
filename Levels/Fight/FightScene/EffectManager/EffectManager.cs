using System.Collections.Generic;
using System.Linq;
using System;
using Godot;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

public interface IEffectHolder
{
    // применённые эффекты
    public List<Effect> appliedEffects { get; set; }
    // ещё не обработанные эффекты - именно отсюда они начнут применяться, если игрок закончит ход, а не отменит его
    // public List<Effect> rawEffects { get; set; }
}

public class EffectManager
{
    
    private MapManager _mapManager;
    private FightSceneManager _fightSceneManager;

    public EffectManager(MapManager InMapManager, FightSceneManager InFightSceneManager)
    {
        _mapManager = InMapManager;
        _fightSceneManager = InFightSceneManager;
    }

    public void ApplyEffects()
    {
        ApplyEffectsForEntity();
        ApplyEffectsForTiles();
    }

    private void ApplyEffectsForEntity()
    {
        List<BattleEntity> battleEntities = _fightSceneManager.AllEntities.ToList();
        foreach (var entity in battleEntities)
        {
            foreach (var effect in entity.appliedEffects.ToList())
            {
                if (effect.IsPending)
                {
                    if (effect.EffectType == EEffectType.Stun &&
                        entity.HasEffect(EEffectType.ResistanceToStun))
                    {
                        GD.Print($"Эффект {effect.EffectType} не применён к {entity.Id} — сопротивление!");
                        continue;
                    }

                    effect.OnApply();
                    effect.IsPending = false;
                    GD.Print($"Эффект {effect.EffectType} применён к {entity.Id}");
                }
                else
                {
                    effect.OnTurnEnd();

                    if (effect.IsExpired)
                    {
                        GD.Print($"Эффект {effect.EffectType} истёк у {entity.Id}");
                        effect.OnRemove();
                        entity.RemoveEffect(effect);
                    }
                }
            }
        }
    }
    private void ApplyEffectsForTiles()
    {

    }
}