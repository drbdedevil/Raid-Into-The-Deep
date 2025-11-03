#nullable enable
using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using BattleEntity = RaidIntoTheDeep.Levels.Fight.BattleEntity;

namespace RaidIntoTheDeep.Levels.Fight.PrepareFightScene;

public partial class PrepareFightManager : Node2D
{
    private static NotificationSystem _notificationSystem = NotificationSystem.Instance;
    
    private PrepareFightMapManager _mapManager;
    private ViewPanel.PlayerWarriorsMenu _playerTeamWarriorsMenu;
    
    [Export]
    private PackedScene _fightScene;
    
    private Button _goFightButton;
    
    public override void _Ready()
    {
        _playerTeamWarriorsMenu = GetNode<ViewPanel.PlayerWarriorsMenu>("PlayerWarriorsMenu");
        GameDataManager.Instance.currentData.livingSpaceData = new LivingSpaceData()
        {
            UsedCharacters =
            [
                new CharacterData()
                {
                    ID = "1",
                    Name = "Жук1",
                    Level = 1
                },
                new CharacterData()
                {
                    ID = "2",
                    Name = "Жук2",
                    Level = 2
                },
                new CharacterData()
                {
                    ID = "3",
                    Name = "Жук3",
                    Level = 3
                }
            ]
        };
        _playerTeamWarriorsMenu.InitTeam(GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters);
        _mapManager = GetNode<PrepareFightScene.PrepareFightMapManager>("Map");
        _mapManager.OnTileRightButtonClicked += _playerTeamWarriorsMenu.SelectPlayerEntityFromMap;
        _mapManager.OnTileLeftButtonClicked += _playerTeamWarriorsMenu.SetPlayerEntityOnMap;
        _playerTeamWarriorsMenu.MapManager = _mapManager;
        
        
        _goFightButton = GetNode<Button>("GoFightButton");
        _goFightButton.ButtonDown += GoToFightScene;

    }

    private void GoToFightScene()
    {
        if (!_playerTeamWarriorsMenu.IsPlayerPlacedAnyoneWarrior)
        {
            _notificationSystem.ShowMessage("А сражаться то кто будет?", EMessageType.Alert);
            return;
        }
        BattleMapInitStateManager.Instance.SetMapData(_mapManager.MapTiles);
        GetTree().ChangeSceneToPacked(_fightScene);
    }
}