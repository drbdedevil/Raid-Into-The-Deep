using System.Threading.Tasks;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public class ApplyingEffectsBattleState : BattleState
{
    private EffectManager _effectManager;
    public ApplyingEffectsBattleState(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        StateTitleText = "Ох зря я сюда полез...";
        
        _effectManager = fightSceneManager.EffectManager;

        _effectManager.ApplyEffects();

        foreach (BattleEntity battleEntity in fightSceneManager.AllEntities)
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
    }

    public override void InputUpdate(InputEvent @event)
    {
    }

    private double time = 0.0d;
    public override void ProcessUpdate(double delta)
    {
        time += delta;
        if (time >= 0.2d)
        {
            FightSceneManager.CurrentBattleState = new EnemyWarriorTurnBattleState(FightSceneManager, MapManager);
        }
    }
}