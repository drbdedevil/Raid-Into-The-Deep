using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public class PlayerWarriorAttackChoosingBattleState : BattleState
{
    
    private PlayerEntity _currentPlayerWarrior;
    
    
    public PlayerWarriorAttackChoosingBattleState(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        _currentPlayerWarrior = fightSceneManager.CurrentPlayerWarriorToTurn;
        StateTitleText = "Вы атакуете оружием! Выбирайте кого атаковать! Выберите клетку из предложенных!";

        FightSceneManager.SkipButton.ButtonUp += SkipButtonPressed;
    }

    public override void InputUpdate(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left && MapManager.GetTileUnderMousePosition() != null)
        {
            var tilesForAttack = MapManager.SelectedTilesForPlayerAction;
            var attackCommand =
                new AttackByWeaponCommand(FightSceneManager.CurrentPlayerWarriorToTurn, tilesForAttack.ToList(), FightSceneManager);
            FightSceneManager.NotExecutedCommands.Add(attackCommand);
            FightSceneManager.SkipButton.ButtonUp -= SkipButtonPressed;
            FightSceneManager.CurrentBattleState = new PlayerWarriorConfirmationBattleState(FightSceneManager, MapManager);
        }
    }

    public override void ProcessUpdate(double delta)
    {
        var tile = MapManager.GetTileUnderMousePosition();
        if (tile is not null)
        {
            MapManager.ClearAllSelectedTiles();
            MapManager.CalculateAndDrawPlayerEntityAttackZone(_currentPlayerWarrior, tile);
        }
    }

    private void SkipButtonPressed()
    {
        MapManager.ClearAllSelectedTiles();
        foreach (var executedCommand in FightSceneManager.ExecutedCommands)
        {
            executedCommand.Execute();
        }
        FightSceneManager.NotExecutedCommands.Clear();
        FightSceneManager.ExecutedCommands.Clear();
        FightSceneManager.SkipButton.ButtonUp -= SkipButtonPressed;
        FightSceneManager.PlayerWarriorsThatTurned.Add(_currentPlayerWarrior);
        if (FightSceneManager.Allies.Count == FightSceneManager.PlayerWarriorsThatTurned.Count)
		{
			FightSceneManager.EnemyWarriorsThatTurned.Clear();
			FightSceneManager.PlayerWarriorsThatTurned.Clear();
			FightSceneManager.CurrentBattleState = new ApplyingEffectsBattleState(FightSceneManager, MapManager);
			return;
		}
        FightSceneManager.CurrentBattleState = new PlayerWarriorMovementBattleState(FightSceneManager, MapManager);
    }
}