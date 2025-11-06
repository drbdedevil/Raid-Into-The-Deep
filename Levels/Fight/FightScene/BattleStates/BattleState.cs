using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public abstract class BattleState
{
    protected readonly FightSceneManager FightSceneManager;
    protected readonly MapManager MapManager;

    protected BattleState(FightSceneManager fightSceneManager, MapManager mapManager)
    {
        FightSceneManager = fightSceneManager;
        MapManager = mapManager;
    }

    public abstract void InputUpdate(InputEvent @event);
    
    public abstract void ProcessUpdate(double delta);
}