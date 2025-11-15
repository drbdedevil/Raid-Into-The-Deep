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

    private readonly List<EntityWithEffectToDraw> _entityWithEffectToDraws = [];
    
    public IReadOnlyCollection<EntityWithEffectToDraw> EntityWithEffectToDraws => _entityWithEffectToDraws.ToList();
    public EffectManager(MapManager InMapManager, FightSceneManager InFightSceneManager)
    {
        _mapManager = InMapManager;
        _fightSceneManager = InFightSceneManager;
    }

    public void ApplyEffects()
    {
        _entityWithEffectToDraws.Clear();
        ApplyEffectsForTiles();
        ApplyEffectsForEntity();
    }

    public void GetEntitiesWithEffectsToDraw()
    {
        
    }
    
    private void ApplyEffectsForEntity()
    {
        List<BattleEntity> battleEntities = _fightSceneManager.AllEntities.ToList();
        foreach (var entity in battleEntities)
        {
            List<Effect> effectsToDraw = [];
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

                    if (effect.EffectType == EEffectType.Fire || effect.EffectType == EEffectType.Poison ||
                        effect.EffectType == EEffectType.Freezing)
                    {
                        effectsToDraw.Add(effect);
                    }
                    effect.OnApply();
                    effect.OnTurnEnd();
                    if (effect.IsExpired)
                    {
                        GD.Print($"Эффект {effect.EffectType} истёк у {entity.Id}");
                        effect.OnRemove();
                        entity.RemoveEffect(effect);
                    }
                    else
                    {
                        GD.Print($"Эффект {effect.EffectType} применён к {entity.Id}");
                    }
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
            
            _entityWithEffectToDraws.Add(new EntityWithEffectToDraw(entity, effectsToDraw));
        }
    }
    private void ApplyEffectsForTiles()
    {
        List<Tile> tiles = _mapManager.MapTiles;
        foreach (var tile in tiles)
        {
            if (tile.ObstacleEntity == null) continue;
            foreach (var effect in tile.ObstacleEntity.appliedEffects.ToList())
            {
                if (effect.IsPending)
                {
                    effect.OnApply();
                    effect.OnTurnEnd();
                    if (effect.IsExpired)
                    {
                        effect.OnRemove();
                    }
                    else
                    {
                        GD.Print($"Эффект {effect.EffectType} применён к тайлу");
                    }
                }
                else
                {
                    effect.OnTurnEnd();

                    if (effect.IsExpired)
                    {
                        effect.OnRemove();
                        tile.ObstacleEntity.RemoveEffect(effect);
                    }
                }
            }
        }
    }
}

public class EntityWithEffectToDraw
{
    public EntityWithEffectToDraw(BattleEntity battleEntity, List<Effect> effectsToDraw)
    {
        BattleEntity = battleEntity;
        EffectsToDraw = effectsToDraw;
    }

    public BattleEntity BattleEntity { get; set; }
    public List<Effect> EffectsToDraw { get; set; }
}