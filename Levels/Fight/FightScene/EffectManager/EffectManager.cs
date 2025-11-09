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

                battleEntity.appliedEffects.RemoveAll(ae => battleEntity.rawEffects.Any(re => re.EffectType == ae.EffectType));
                if (battleEntity.appliedEffects.Any(effect => effect.EffectType == EEffectType.ResistanceToStun))
                {
                    battleEntity.rawEffects.RemoveAll(ae => ae.EffectType == EEffectType.Stun);
                }
                foreach (EntityEffect effect in battleEntity.rawEffects)
                {
                    effect.entityHolder = battleEntity;
                }
                battleEntity.appliedEffects.AddRange(battleEntity.rawEffects);
                battleEntity.rawEffects.Clear();

                List<Effect> effectsToDelete = new();
                foreach (Effect effect in battleEntity.appliedEffects)
                {
                    effect.ApplyForHolder();
                    if (effect.bIsShouldRemoveFromEffectHolder)
                    {
                        effectsToDelete.Add(effect);
                    }
                }
                foreach (Effect effect in effectsToDelete)
                {
                    if (effect.EffectType == EEffectType.Stun)
                    {
                        EffectInfo effectInfo = GameDataManager.Instance.effectDatabase.Effects.FirstOrDefault(effect => effect.effectType == EEffectType.ResistanceToStun);
                        EntityEffect resitanceStunEffect = new EntityEffect(effectInfo.effectType, effectInfo.duration);
                        resitanceStunEffect.entityHolder = battleEntity;
                        battleEntity.appliedEffects.Add(resitanceStunEffect);
                    }
                    battleEntity.appliedEffects.Remove(effect);
                }
            }
        }
    }
    private void ApplyEffectsForTiles()
    {

    }
}