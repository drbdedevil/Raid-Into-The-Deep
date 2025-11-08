using System.Threading.Tasks;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public class ApplyingEffectsBattleState : BattleState
{
    private EffectManager _effectManager;
    public ApplyingEffectsBattleState(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        _effectManager = fightSceneManager.EffectManager;

        _effectManager.ApplyEffects();
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
            FightSceneManager.CurrentBattleState = new PlayerWarriorMovementBattleState(FightSceneManager, MapManager);
        }
    }
}