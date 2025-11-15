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
    }

    public override void InputUpdate(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left && MapManager.GetTileUnderMousePosition() != null)
        {
            var tilesForAttack = MapManager.SelectedTilesForPlayerAction;
            var attackCommand =
                new AttackByWeaponCommand(FightSceneManager.CurrentPlayerWarriorToTurn, tilesForAttack.ToList(), FightSceneManager);
            FightSceneManager.NotExecutedCommands.Add(attackCommand);
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
}