using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using RaidIntoTheDeep.Levels.FightScene.Scripts;

public partial class FightSceneManager : Node2D
{
    private readonly RayCast2D _rayCast = new ();
    private MapManager _mapManager;

    private readonly List<GameEntity> _allies = [];
    private readonly List<GameEntity> _enemies = [];

    private GameEntity enemyTest;
    
    private Tile _selectedTile = null;
    
    public override void _Ready()
    {
        _mapManager = GetNode<MapManager>("Map");
        var enemyScene = GD.Load<PackedScene>("res://Levels/FightScene/Enemy.tscn");
        var tile = _mapManager.GetTileByCartesianCoord(new Vector2I(0, 0));
        var enemy = enemyScene.Instantiate<GameEntity>();
        enemyTest = enemy;
        _enemies.Add(enemy);
        _mapManager.SetEnemyOnTile(tile, enemy);
        
    }

    public override void _Process(double delta)
    {
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

        if (_selectedTile is not null && Input.IsMouseButtonPressed(MouseButton.Left))
        {
            _mapManager.SetEnemyOnTile(_selectedTile, _enemies.First());
        }
    }
}
