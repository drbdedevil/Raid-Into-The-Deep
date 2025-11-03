using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

public partial class FightSceneManager : Node2D
{
    private readonly RayCast2D _rayCast = new ();
    private MapManager _mapManager;

    private readonly List<BattleEntity> _allEntities = [];
    
    private readonly List<PlayerEntity> _allies = [];
    private readonly List<EnemyEntity> _enemies = [];
    
    private Tile _selectedTile = null;
    
    
    public override void _Ready()
    {
        _mapManager = GetNode<MapManager>("Map");
        GD.Print(BattleMapInitStateManager.Instance.Tiles.Count);
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
        }
    }
}