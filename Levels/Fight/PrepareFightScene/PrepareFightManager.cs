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
    
    private FightScene.Scripts.MapManager _mapManager;
    
    private PlayerWarriorsMenu _playerWarriorsMenu;
    
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

    public override void _PhysicsProcess(double delta)
    {
        var selectedTile = _mapManager.GetTileUnderMousePosition();

        if (selectedTile is null)
        {
            if (_selectedTile != null)
            {
                _mapManager.DeselectTile(_selectedTile);
                _selectedTile = null;
            }   
        }
        else if (_selectedTile == null || selectedTile.IsometricPosition != _selectedTile.IsometricPosition)
        {
            if (_selectedTile != null)
            {
                _mapManager.DeselectTile(_selectedTile);
            }   
            _selectedTile = selectedTile;
            if (_selectedTile is not null) _mapManager.SelectTile(_selectedTile);
        }

        if (_selectedTile is not null && _currentSelectedCharacterData is not null && Input.IsMouseButtonPressed(MouseButton.Left))
        {
            _mapManager.SetEnemyOnTile(_selectedTile, new BattleEntity());
            _playerWarriorsMenu.RemoveWarriorIconFromContainer(_currentSelectedCharacterData);
            _currentSelectedCharacterData = null;
        }
    }

    public override void _Ready()
    {
        _playerWarriorsMenu = GetNode<PlayerWarriorsMenu>("PlayerWarriorsMenu");
        _playerWarriorsMenu.OnCharacterDataClicked += OnCharacterDataSelected;
        _mapManager = GetNode<FightScene.Scripts.MapManager>("Map");
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
                    GD.Print("Here");
                }
            }
        }
        
        foreach (var characterData in _characterData)
        {
            _playerWarriorsMenu.AddWarriorIconToContainer(characterData);
        }
        
    }

    private void OnCharacterDataSelected(PrepareFightWarriorPanel warriorPanel)
    {
        GD.Print("OnCharacterDataSelected");
        _currentSelectedCharacterData = _playerWarriorsMenu.GetCharacterDataByPrepareFightWarriorPanel(warriorPanel);
    }
    
    
}