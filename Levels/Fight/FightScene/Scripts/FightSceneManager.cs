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
    private Tile? _currentPlayerWarriorPrevTile;
    private bool _isPlayerTurn = true;
    private bool _isPlayerAttackTurn = false;
    
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
        if (_isPlayerTurn && !_isPlayerAttackTurn)
        {
            if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (_mapManager.MovePlayerEntityInSpeedZone(_currentPlayerWarriorToTurn!))
                {
                    _mapManager.ClearAllSelectedTiles();
                    _isPlayerAttackTurn = true;
                }
            }
        }
        else if (_isPlayerAttackTurn)
        {
            var tile = _mapManager.GetTileUnderMousePosition();
            if (tile is not null)
            {
                _mapManager.ClearAllSelectedTiles();
                _mapManager.DrawPlayerEntityAttackZone(_currentPlayerWarriorToTurn, tile);
            }
            
            if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                _playerWarriorsTurn.Remove(_currentPlayerWarriorToTurn);
                _currentPlayerWarriorToTurn = _playerWarriorsTurn.First();
                _currentPlayerWarriorPrevTile = _currentPlayerWarriorToTurn!.Tile;
                _mapManager.DrawPlayerEntitySpeedZone(_currentPlayerWarriorToTurn!);
            }
            
        }
    }

    public override void _Process(double delta)
    {
        if (_isPlayerAttackTurn)
        {
            
        }
    }

    public void ResetPlayerWarriorTurn()
    {
        _isPlayerTurn = false;
        _mapManager.SetBattleEntityOnTile(_currentPlayerWarriorPrevTile, _currentPlayerWarriorToTurn);
        _currentPlayerWarriorToTurn = null;
    }
}