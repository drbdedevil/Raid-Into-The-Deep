#nullable enable
using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using BattleEntity = RaidIntoTheDeep.Levels.Fight.FightScene.Scripts.BattleEntity;

namespace RaidIntoTheDeep.Levels.Fight.PrepareFightScene;

public partial class PrepareFightManager : Node2D
{
    
    
    private char[,] _enemiesOnMap = new char[,]
    {
        {'e', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'1', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'2', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'3', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'e'}
    };
    
    private PrepareFightMapManager _mapManager;
    private ViewPanel.PlayerWarriorsMenu _playerTeamWarriorsMenu;
    
    private List<BattleEntity> _playerBattleEntities = [];
    private List<CharacterData> _characterData = new()
    {
        new CharacterData()
        {
            ID = "1", Damage = 100, Heal = 100, Name = "123", Portrait = "123", Speed = 3, Upgrades = [true],
            Weapon = null
        },
        new CharacterData()
        {
            ID = "2", Damage = 100, Heal = 100, Name = "123", Portrait = "123", Speed = 3, Upgrades = [true],
            Weapon = null
        }
    };
    
    private CharacterData? _currentSelectedCharacterData;
    private Tile _selectedTile = null;
    
    
    public void Setup(List<CharacterData> characterData)
    {
        _characterData = characterData;
    }

    
    public override void _Ready()
    {
        _playerTeamWarriorsMenu = GetNode<ViewPanel.PlayerWarriorsMenu>("PlayerWarriorsMenu");
        _playerTeamWarriorsMenu.InitTeam(_characterData);
        _mapManager = GetNode<PrepareFightScene.PrepareFightMapManager>("Map");
        _mapManager.OnTileRightButtonClicked += _playerTeamWarriorsMenu.AddWarriorIconToTeamContainer;
        _mapManager.OnTileLeftButtonClicked += _playerTeamWarriorsMenu.SetCharacterDataOnMap;
        _playerTeamWarriorsMenu.MapManager = _mapManager;
        var enemyScene = GD.Load<PackedScene>("res://Levels/Fight/FightScene/Enemy.tscn");

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (_enemiesOnMap[y, x] == 'e')
                {
                    var enemy = enemyScene.Instantiate<BattleEntity>();
                    var tile = _mapManager.GetTileByCartesianCoord(new Vector2I(x, y));
                    _mapManager.SetEnemyOnTile(tile!, enemy);
                }
            }
        }
    }
}