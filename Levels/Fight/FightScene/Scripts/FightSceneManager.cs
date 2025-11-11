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
    
    private List<ObstacleEntity> _obstacles = [];
    
    public IReadOnlyCollection<ObstacleEntity> Obstacles => _obstacles.ToList();
    
    public Label TitleLabel { get; private set; }
    
    /// <summary>
    /// Воины игрока которые уже сходили
    /// </summary>
    public List<PlayerEntity> PlayerWarriorsThatTurned = [];

    public List<EnemyEntity> EnemyWarriorsThatTurned = [];

    [Signal]
    public delegate void CurrentPlayerWarriorToTurnChangedEventHandler(string playerWarriorId);
    [Signal]
    public delegate void FightSceneManagerInitializedEventHandler();
    [Signal]
    public delegate void BattleEntityMadeMoveEventHandler();
    
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

    private BattleState? _currentBattleState;
    public BattleState CurrentBattleState
    {
        get  => _currentBattleState;
        set
        {
            _currentBattleState = value;
            if (_currentBattleState != null) EmitSignal(SignalName.BattleEntityMadeMove);
        }
    }
    
    
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
        _obstacles = BattleMapInitStateManager.Instance.ObstacleEntities.ToList();
        
        _allEntities.AddRange(_allies);
        _allEntities.AddRange(_enemies);
        _allEntities.AddRange(_obstacles);
        
        TitleLabel = GetNode<Label>("StateTitleText");
        
        ConfirmTurnButton = GetNode<Button>("Control/HBoxContainer/MarginContainer2/ConfirmTurnButton");
        ConfirmTurnButton.SetDisabled(true);
        CancelTurnButton = GetNode<Button>("Control/HBoxContainer/MarginContainer/CancelTurnButton");
        CancelTurnButton.SetDisabled(true);
    
        CurrentBattleState = new PlayerWarriorMovementBattleState(this, _mapManager);

        _effectManager = new EffectManager(_mapManager, this);

        EntityEffectsPanel entityEffectsPanel = GetNode<EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
        entityEffectsPanel.Visible = false;

        EmitSignal(SignalName.FightSceneManagerInitialized);
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
        _mapManager.RemoveBattleEntityFromTile(playerWarrior.Tile, isDead: true);
        _allies.Remove(playerWarrior);
        _allEntities.Remove(playerWarrior);
    }

    public void RemoveEnemyWarrior(EnemyEntity enemyWarrior)
    {
        _mapManager.RemoveBattleEntityFromTile(enemyWarrior.Tile, isDead: true);
        _enemies.Remove(enemyWarrior);
        _allEntities.Remove(enemyWarrior);
    }

    public void OnEnemyPanelMouseEnter(EnemyEntity enemyEntity)
    {
        EnemyFightScenePanel enemyFightScenePanel = GetNode<EnemyFightScenePanel>("HBoxContainer/EnemyFightScenePanel");

        EntityEffectsPanel entityEffectsPanel = GetNode<EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
        entityEffectsPanel.Visible = true;

        entityEffectsPanel.SetEffectsInfos(enemyEntity.appliedEffects);
        entityEffectsPanel.ChangeToFitAndReplace(enemyEntity.appliedEffects.Count);
        entityEffectsPanel.SetPositionEnemyOffset(enemyFightScenePanel.GetEnemyPanelPositionByID(enemyEntity.Id));
    }

    public void OnEnemyPanelMouseExit(EnemyEntity enemyEntity)
    {
        EntityEffectsPanel entityEffectsPanel = GetNode<EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
        entityEffectsPanel.Visible = false;
    }
    
    public void OnCharacterPanelMouseEnter(string characterID)
    {
        FightScenePanel fightScenePanel = GetNode<FightScenePanel>("HBoxContainer/FightScenePanel");

        EntityEffectsPanel entityEffectsPanel = GetNode<EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
        entityEffectsPanel.Visible = true;

        PlayerEntity playerEntity = Allies.FirstOrDefault(character => character.Id == characterID);
        if (playerEntity != null)
        {
            entityEffectsPanel.SetEffectsInfos(playerEntity.appliedEffects);
            entityEffectsPanel.ChangeToFitAndReplace(playerEntity.appliedEffects.Count);
            entityEffectsPanel.SetPositionWarriorOffset(fightScenePanel.GetWarriorPanelPositionByID(characterID));
        }
    }

    public void OnCharacterPanelMouseExit(string characterID)
    {
        EntityEffectsPanel entityEffectsPanel = GetNode<EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
        entityEffectsPanel.Visible = false;
    }
}