using System.Threading.Tasks;
using Godot;
using System.Linq;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public class PlayerWarriorConfirmationBattleState : BattleState
{
    public PlayerWarriorConfirmationBattleState(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        FightSceneManager.ConfirmTurnButton.ButtonUp += ConfirmTurn;
        FightSceneManager.ConfirmTurnButton.SetDisabled(false);
        FightSceneManager.CancelTurnButton.ButtonUp += CancelTurn;
        FightSceneManager.CancelTurnButton.SetDisabled(false);
        
        StateTitleText = "Уверены ли в своём выборе? Подтвердите ваш выбор!";
    }

    public override void InputUpdate(InputEvent @event)
    {
    }

    public override void ProcessUpdate(double delta)
    {
    }

    private void ConfirmTurn()
    {
    
        MapManager.ClearAllSelectedTiles();
        foreach (var notExecutedCommand in FightSceneManager.NotExecutedCommands)
        {
            notExecutedCommand.Execute();
        }
        FightSceneManager.NotExecutedCommands.Clear();
        FightSceneManager.ExecutedCommands.Clear();

        Effect battleFrenzyEffect = FightSceneManager.CurrentPlayerWarriorToTurn.appliedEffects.FirstOrDefault(effect => effect.EffectType == EEffectType.BattleFrenzy);
        if (battleFrenzyEffect is BattleFrenzyEntityEffect battleFrenzyEntityEffect)
        {
            if (battleFrenzyEntityEffect.PlayerKilledSomeone)
            {
                battleFrenzyEntityEffect.PlayerKilledSomeone = false;
                if (FightSceneManager.PlayerWarriorsThatTurned.Count == FightSceneManager.Allies.Count)
                    FightSceneManager.CurrentBattleState = new ApplyingEffectsBattleState(FightSceneManager, MapManager);
                else 
                    FightSceneManager.CurrentBattleState = new PlayerWarriorMovementBattleState(FightSceneManager, MapManager);
                DisableButtons();
                return;
            }
        }

        FightSceneManager.PlayerWarriorsThatTurned.Add(FightSceneManager.CurrentPlayerWarriorToTurn);
        
        if (FightSceneManager.PlayerWarriorsThatTurned.Count == FightSceneManager.Allies.Count)
            FightSceneManager.CurrentBattleState = new ApplyingEffectsBattleState(FightSceneManager, MapManager);
        else 
            FightSceneManager.CurrentBattleState = new PlayerWarriorMovementBattleState(FightSceneManager, MapManager);
        
        DisableButtons();
    }

    private void CancelTurn()
    {
        MapManager.ClearAllSelectedTiles();
        foreach (var executedCommand in FightSceneManager.ExecutedCommands)
        {
            executedCommand.UnExecute();
        }
        FightSceneManager.NotExecutedCommands.Clear();
        FightSceneManager.ExecutedCommands.Clear();
        FightSceneManager.CurrentBattleState = new PlayerWarriorMovementBattleState(FightSceneManager, MapManager);
        DisableButtons();
    }

    private void DisableButtons()
    {
        FightSceneManager.ConfirmTurnButton.ButtonUp -= ConfirmTurn;
        FightSceneManager.ConfirmTurnButton.SetDisabled(true);
        FightSceneManager.CancelTurnButton.ButtonUp -= CancelTurn;
        FightSceneManager.CancelTurnButton.SetDisabled(true);
    }

    private void SecondTurn()
    {
        MapManager.ClearAllSelectedTiles();
        foreach (var executedCommand in FightSceneManager.ExecutedCommands)
        {
            executedCommand.Execute();
        }
        FightSceneManager.NotExecutedCommands.Clear();
        FightSceneManager.ExecutedCommands.Clear();
        FightSceneManager.CurrentBattleState = new PlayerWarriorMovementBattleState(FightSceneManager, MapManager);
        DisableButtons();
    }
}