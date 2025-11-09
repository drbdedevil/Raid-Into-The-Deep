using System;
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
    
    public Label TitleLabel { get; private set; }
    
    /// <summary>
    /// Воины игрока которые уже сходили
    /// </summary>
    public List<PlayerEntity> PlayerWarriorsThatTurned = [];

    public List<EnemyEntity> EnemyWarriorsThatTurned = [];

    [Signal]
    public delegate void CurrentPlayerWarriorToTurnChangedEventHandler(string playerWarriorId);
    
    private PlayerEntity? _currentPlayerWarriorToTurn;
    public PlayerEntity? CurrentPlayerWarriorToTurn
    {
        get => _currentPlayerWarriorToTurn;
        set
        {
            _currentPlayerWarriorToTurn = value;
            if (_currentPlayerWarriorToTurn != null) EmitSignalCurrentPlayerWarriorToTurnChanged(_currentPlayerWarriorToTurn.Id);
        }
    }

    public BattleState  CurrentBattleState { get; set; }
    
    
    public Button ConfirmTurnButton { get; set; }
    public Button CancelTurnButton { get; set; }
    /// <summary>
    /// </summary>
    public List<Command> ExecutedCommands { get; set; } = [];

    public List<Command> NotExecutedCommands { get; set; } = [];

    private EffectManager _effectManager;
    public EffectManager EffectManager => _effectManager;
    
    public override void _Ready()
    {
        _mapManager = GetNode<MapManager>("Map");
        _allies = BattleMapInitStateManager.Instance.PlayerEntities.ToList();
        _enemies = BattleMapInitStateManager.Instance.EnemyEntities.ToList();
        
        _allEntities.AddRange(_allies);
        _allEntities.AddRange(_enemies);
        
        TitleLabel = GetNode<Label>("StateTitleText");
        ConfirmTurnButton = GetNode<Button>("ConfirmTurnButton");
        ConfirmTurnButton.SetDisabled(true);
        CancelTurnButton = GetNode<Button>("CancelTurnButton");
        CancelTurnButton.SetDisabled(true);
    
        CurrentBattleState = new PlayerWarriorMovementBattleState(this, _mapManager);

        _effectManager = new EffectManager(_mapManager, this);
    }
    
    public override void _Input(InputEvent @event)
    {
        CurrentBattleState.InputUpdate(@event);
    }

    public override void _Process(double delta)
    {
        CurrentBattleState.ProcessUpdate(delta);
    }

    public void RemovePlayerWarrior(PlayerEntity playerWarrior)
    {
        _mapManager.RemoveBattleEntityFromTile(playerWarrior.Tile);
        _allies.Remove(playerWarrior);
        _allEntities.Remove(playerWarrior);
    }

    public void RemoveEnemyWarrior(EnemyEntity enemyWarrior)
    {
        _mapManager.RemoveBattleEntityFromTile(enemyWarrior.Tile);
        _enemies.Remove(enemyWarrior);
        _allEntities.Remove(enemyWarrior);
    }
}