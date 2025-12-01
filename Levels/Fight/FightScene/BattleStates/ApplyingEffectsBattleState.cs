using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.EffectManagerLogic;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public class ApplyingEffectsBattleState : BattleState
{
    private EffectManager _effectManager;

    private List<Task> _allTasks = [];
    
    public ApplyingEffectsBattleState(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        StateTitleText = "Ох зря я сюда полез...";
        FightSceneManager.SkipButton.SetDisabled(true);
        
        _effectManager = fightSceneManager.EffectManager;

        _effectManager.ApplyEffects();

        foreach (var entityToDrawEffect in _effectManager.EntityWithEffectToDraws)
        {
            Task? currentEffectTask = null;
            foreach (var effect in entityToDrawEffect.EffectsToDraw)
            {
                if (currentEffectTask is null)
                {
                    currentEffectTask = DrawEffectApply(entityToDrawEffect.BattleEntity, effect, null);
                    _allTasks.Add(currentEffectTask);
                }
                else
                {
                    var newEffect = DrawEffectApply(entityToDrawEffect.BattleEntity, effect, currentEffectTask);
                    currentEffectTask = newEffect;
                    _allTasks.Add(currentEffectTask);
                }
            }
        }
    }

    public override void InputUpdate(InputEvent @event)
    {
    }

    private double time = 0.0d;
    public override void ProcessUpdate(double delta)
    {
        if (_allTasks.All(x => x.IsCompleted))
        {
            foreach (BattleEntity battleEntity in FightSceneManager.AllEntities)
            {
                if (battleEntity is PlayerEntity playerWarrior)
                {
                    if (battleEntity.Health <= 0)
                    {
                        FightSceneManager.RemovePlayerWarrior(playerWarrior);
                        SoundManager.Instance.PlaySoundOnce("res://Sound/Death.wav", 0.6f);
                    }
                }
                else if (battleEntity is EnemyEntity enemyEntity)
                {
                    if (battleEntity.Health <= 0)
                    {
                        FightSceneManager.RemoveEnemyWarrior(enemyEntity);
                        SoundManager.Instance.PlaySoundOnce("res://Sound/Death.wav", 0.6f);
                    }
                }
            }
            FightSceneManager.CurrentBattleState = new EnemyWarriorTurnBattleState(FightSceneManager, MapManager);
        }
    }

    private async Task DrawEffectApply(BattleEntity battleEntity, Effect effect, Task? task)
    {
        if (task is not null) await task;
        if (effect.EffectType == EEffectType.Poison)
        {
            MapManager.SetBattleEntityOnTile(battleEntity.Tile, battleEntity, (int)EffectDrawTypes.Poison);
        }
        else if (effect.EffectType == EEffectType.Fire)
        {
            MapManager.SetBattleEntityOnTile(battleEntity.Tile, battleEntity, (int)EffectDrawTypes.Fire);
        }
        else if (effect.EffectType == EEffectType.Freezing)
        {
            MapManager.SetBattleEntityOnTile(battleEntity.Tile, battleEntity, (int)EffectDrawTypes.Freezing);
        }

        await Task.Delay(500);
        
        MapManager.SetBattleEntityOnTile(battleEntity.Tile, battleEntity);
        
        await Task.Delay(250);
    }
}