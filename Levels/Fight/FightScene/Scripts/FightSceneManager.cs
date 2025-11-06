using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;
using RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

public partial class FightSceneManager : Node2D
{
    private readonly RayCast2D _rayCast = new ();
    private MapManager _mapManager;
    
    private List<BattleEntity> _allEntities = [];
    public IReadOnlyCollection<BattleEntity> AllEntities => _allEntities.ToList();
    
    private List<PlayerEntity> _allies = [];
    public IReadOnlyCollection<PlayerEntity> Allies => _allies.ToList();
    
    private List<EnemyEntity> _enemies = [];
    public IReadOnlyCollection<EnemyEntity> Enemies => _enemies.ToList();
    
    /// <summary>
    /// Воины игрока которые уже сходили
    /// </summary>
    public List<PlayerEntity> PlayerWarriorsThatTurned { get; set; } = [];
    public PlayerEntity? CurrentPlayerWarriorToTurn { get; set; }
    public BattleState  CurrentBattleState { get; set; }
    
    
    public Button ConfirmTurnButton { get; set; }
    public Button CancelTurnButton { get; set; }
    /// <summary>
    /// </summary>
    public List<Command> ExecutedCommands { get; set; } = [];

    public List<Command> NotExecutedCommands { get; set; } = [];
    
    public override void _Ready()
    {
        _mapManager = GetNode<MapManager>("Map");
        _allies = BattleMapInitStateManager.Instance.PlayerEntities.ToList();
        _enemies = BattleMapInitStateManager.Instance.EnemyEntities.ToList();
        
        _allEntities.AddRange(_allies);
        _allEntities.AddRange(_enemies);
        
        ConfirmTurnButton = GetNode<Button>("ConfirmTurnButton");
        ConfirmTurnButton.SetDisabled(true);
        CancelTurnButton = GetNode<Button>("CancelTurnButton");
        CancelTurnButton.SetDisabled(true);
        
        CurrentBattleState = new PlayerWarriorMovementBattleState(this, _mapManager);
    }
    
    public override void _Input(InputEvent @event)
    {
        CurrentBattleState.InputUpdate(@event);
        /*
         * 
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
                if (!_playerWarriorsTurn.Any())
                {
                    _playerWarriorsTurn = _allies.OrderByDescending(x => x.Speed).ToList();
                }
                _currentPlayerWarriorToTurn = _playerWarriorsTurn.First();
                _currentPlayerWarriorPrevTile = _currentPlayerWarriorToTurn!.Tile;
                _mapManager.ClearAllSelectedTiles();
                _mapManager.CalculateAndDrawPlayerEntitySpeedZone(_currentPlayerWarriorToTurn!);
                _isPlayerAttackTurn = false;
            }
            
        }
         */
    }

    public override void _Process(double delta)
    {
        CurrentBattleState.ProcessUpdate(delta);
    }
}