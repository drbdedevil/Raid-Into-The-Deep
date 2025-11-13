using System.Linq;
using Godot;
using System.Collections.Generic;
using RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

public class PlayerWarriorSkillChoosingBattleState : BattleState
{

    private PlayerEntity _currentPlayerWarrior;
    private double timer = 0f;
    private bool bCheckFinished = false;
    
    public PlayerWarriorSkillChoosingBattleState(FightSceneManager fightSceneManager, MapManager mapManager) : base(fightSceneManager, mapManager)
    {
        _currentPlayerWarrior = fightSceneManager.CurrentPlayerWarriorToTurn;
        StateTitleText = "Вы атакуете навыком! Выберите клетки из предложенных или примените навык!";
    }

    public override void InputUpdate(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
        {
            var tilesForAttack = MapManager.SelectedTilesForPlayerAction;
            var attackCommand =
                new ApplySkillCommand(FightSceneManager.CurrentPlayerWarriorToTurn, tilesForAttack.ToList(), MapManager);
            FightSceneManager.NotExecutedCommands.Add(attackCommand);
            FightSceneManager.CurrentBattleState = new PlayerWarriorConfirmationBattleState(FightSceneManager, MapManager);
        }
        
        
        
    }

    public override void ProcessUpdate(double delta)
    {
        timer += delta;
        if (!bCheckFinished && timer > 0.2)
        {
            Effect generatedEffect = FightSceneManager.CurrentPlayerWarriorToTurn.activeSkill.GenerateEffect();
            if (generatedEffect != null && generatedEffect.TargetType == EEffectTarget.Self)
            {
                var attackCommand =
                    new ApplySkillCommand(FightSceneManager.CurrentPlayerWarriorToTurn, new List<Tile>(), MapManager);
                FightSceneManager.NotExecutedCommands.Add(attackCommand);
                FightSceneManager.CurrentBattleState = new PlayerWarriorConfirmationBattleState(FightSceneManager, MapManager);
            }
            bCheckFinished = true;
        }

        var tile = MapManager.GetTileUnderMousePosition();
        if (tile is not null)
        {
            MapManager.ClearAllSelectedTiles();
            MapManager.CalculateAndDrawPlayerEntitySkillZone(_currentPlayerWarrior, tile);
            // MapManager.CalculateAndDrawPlayerEntitySkillArea ? 
        }
    }
}