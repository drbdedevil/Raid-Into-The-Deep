using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

public partial class FightSceneManager : Node2D
{
    private readonly RayCast2D _rayCast = new ();
    private MapManager _mapManager;

    private List<BattleEntity> _allEntities = [];
    private List<PlayerEntity> _allies = [];
    private List<EnemyEntity> _enemies = [];

    
    private List<PlayerEntity> _playerWarriorsTurn = [];
    
    private PlayerEntity? _currentPlayerWarriorToTurn; 

    private bool _isPlayerTurn = true;
    
    public override void _Ready()
    {
        _mapManager = GetNode<MapManager>("Map");
        _allies = BattleMapInitStateManager.Instance.PlayerEntities.ToList();
        _enemies = BattleMapInitStateManager.Instance.EnemyEntities.ToList();
        
        _allEntities.AddRange(_allies);
        _allEntities.AddRange(_enemies);
        
        _playerWarriorsTurn.AddRange(_allies.OrderByDescending(x => x.Speed).ToList());
        _currentPlayerWarriorToTurn = _playerWarriorsTurn.First();
        _mapManager.DrawPlayerEntitySpeedZone(_currentPlayerWarriorToTurn!);
    }


    public override void _Input(InputEvent @event)
    {
        if (_isPlayerTurn)
        {
            if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (_mapManager.MovePlayerEntityInSpeedZone(_currentPlayerWarriorToTurn!))
                {
                    _playerWarriorsTurn.Remove(_currentPlayerWarriorToTurn);
                    if (!_playerWarriorsTurn.Any())
                    {
                        _mapManager.ClearAllSelectedTiles();
                        _isPlayerTurn = false;
                        return;
                    }
                    _currentPlayerWarriorToTurn = _playerWarriorsTurn.First();
                    _mapManager.DrawPlayerEntitySpeedZone(_currentPlayerWarriorToTurn!);
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        
    }
}