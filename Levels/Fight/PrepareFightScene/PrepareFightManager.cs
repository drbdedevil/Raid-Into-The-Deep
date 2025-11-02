#nullable enable
using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using BattleEntity = RaidIntoTheDeep.Levels.Fight.FightScene.Scripts.BattleEntity;

namespace RaidIntoTheDeep.Levels.Fight.PrepareFightScene;

public partial class PrepareFightManager : Node2D
{
    private PrepareFightMapManager _mapManager;
    private ViewPanel.PlayerWarriorsMenu _playerTeamWarriorsMenu;
    
    private List<BattleEntity> _playerBattleEntities = [];
    
    public override void _Ready()
    {
        _playerTeamWarriorsMenu = GetNode<ViewPanel.PlayerWarriorsMenu>("PlayerWarriorsMenu");
        _playerTeamWarriorsMenu.InitTeam(GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters);
        _mapManager = GetNode<PrepareFightScene.PrepareFightMapManager>("Map");
        _mapManager.OnTileRightButtonClicked += _playerTeamWarriorsMenu.SelectCharacterFromMap;
        _mapManager.OnTileLeftButtonClicked += _playerTeamWarriorsMenu.SetCharacterDataOnMap;
        _playerTeamWarriorsMenu.MapManager = _mapManager;
        var enemyScene = GD.Load<PackedScene>("res://Levels/Fight/FightScene/Enemy.tscn");
        
    }
}