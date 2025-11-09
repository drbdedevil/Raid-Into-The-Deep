using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public abstract class BattleState
{
    protected readonly FightSceneManager FightSceneManager;
    protected readonly MapManager MapManager;
    private string _stateTitleText = string.Empty;
    
    protected BattleState(FightSceneManager fightSceneManager, MapManager mapManager)
    {
        FightSceneManager = fightSceneManager;
        MapManager = mapManager;
    }

    protected string StateTitleText
    {
        get => _stateTitleText;
        set
        {
            _stateTitleText = value;
            FightSceneManager.TitleLabel.Text = _stateTitleText;
        }
    }

    public abstract void InputUpdate(InputEvent @event);
    
    public abstract void ProcessUpdate(double delta);
}