using System.Collections.Generic;
using System.Linq;
using System;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

public interface IEffectHolder
{
    // применённые эффекты
    public List<Effect> appliedEffects { get; set; }
    // ещё не обработанные эффекты - именно отсюда они начнут применяться, если игрок закончит ход, а не отменит его
    public List<Effect> rawEffects { get; set; }
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
        foreach (BattleEntity battleEntity in battleEntities)
        {
            if (battleEntity is IEffectHolder)
            {
                var seenIds = new HashSet<EEffectType>();
                for (int i = battleEntity.rawEffects.Count - 1; i >= 0; --i)
                {
                    if (!seenIds.Add(battleEntity.rawEffects[i].EffectType))
                    {
                        battleEntity.rawEffects.RemoveAt(i);
                    }
                }

                battleEntity.rawEffects = battleEntity.rawEffects.Except(battleEntity.appliedEffects).ToList();
                foreach (EntityEffect effect in battleEntity.rawEffects)
                {
                    effect.entityHolder = battleEntity;
                }
                battleEntity.appliedEffects.AddRange(battleEntity.rawEffects);
                battleEntity.rawEffects.Clear();

                foreach (Effect effect in battleEntity.appliedEffects)
                {
                    effect.ApplyForHolder();
                }
            }
        }
    }
    private void ApplyEffectsForTiles()
    {

    }
}