using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;
using RaidIntoTheDeep.Levels.Fight.FightScene.BattleStates;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

public partial class FightSceneManager : Control
{
	private readonly RayCast2D _rayCast = new ();
	private MapManager _mapManager;
	public MapManager MapManager
	{
		get { return _mapManager; }
		set { _mapManager = value; }
	}
	
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
	public delegate void EnemyWarriorToTurnChangedEventHandler();
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

	private EnemyEntity? _currentEnemyWarriorToTurn;

	public EnemyEntity CurrentEnemyWarriorToTurn
	{
		get => _currentEnemyWarriorToTurn;
		set
		{
			_currentEnemyWarriorToTurn = value;
			if (_currentEnemyWarriorToTurn is not null) EmitSignalEnemyWarriorToTurnChanged();
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
	public Button SkipButton { get; set; }
	/// <summary>
	/// </summary>
	public List<Command> ExecutedCommands { get; set; } = [];

	public List<Command> NotExecutedCommands { get; set; } = [];

	private EffectManagerLogic.EffectManager _effectManager;
	public EffectManagerLogic.EffectManager EffectManager => _effectManager;
	
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
		SkipButton = GetNode<Button>("Control/HBoxContainer/MarginContainer3/SkipButton");
	
		CurrentBattleState = new PlayerWarriorMovementBattleState(this, _mapManager);

		_effectManager = new(_mapManager, this);

		EffectManagerLogic.EntityEffectsPanel entityEffectsPanel = GetNode<EffectManagerLogic.EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
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
		ApplyExperienceByEnemy(enemyWarrior);
		GameDataManager.Instance.currentData.commandBlockData.EnemyDefeated += 1;

		_mapManager.RemoveBattleEntityFromTile(enemyWarrior.Tile, isDead: true);
		_enemies.Remove(enemyWarrior);
		_allEntities.Remove(enemyWarrior);
	}

	public void OnEnemyPanelMouseEnter(EnemyEntity enemyEntity)
	{
		EnemyFightScenePanel enemyFightScenePanel = GetNode<EnemyFightScenePanel>("HBoxContainer/EnemyFightScenePanel");

		EffectManagerLogic.EntityEffectsPanel entityEffectsPanel = GetNode<EffectManagerLogic.EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
		entityEffectsPanel.Visible = true;

		entityEffectsPanel.SetEffectsInfos(enemyEntity.appliedEffects, enemyEntity.Weapon.effect);
		entityEffectsPanel.ChangeToFitAndReplace(enemyEntity.appliedEffects.Count, enemyEntity.Weapon.effect);
		entityEffectsPanel.SetPositionEnemyOffset(enemyFightScenePanel.GetEnemyPanelPositionByID(enemyEntity.Id));
		
		_mapManager.SelectTileForCurrentEnemyEntityTurn(enemyEntity.Tile);
	}

	public void OnEnemyPanelMouseExit(EnemyEntity enemyEntity)
	{
		EffectManagerLogic.EntityEffectsPanel entityEffectsPanel = GetNode<EffectManagerLogic.EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
		entityEffectsPanel.Visible = false;
		_mapManager.DeselectTile(enemyEntity.Tile);
	}
	
	public void OnCharacterPanelMouseEnter(string characterID)
	{
		FightScenePanel fightScenePanel = GetNode<FightScenePanel>("HBoxContainer/FightScenePanel");

		EffectManagerLogic.EntityEffectsPanel entityEffectsPanel = GetNode<EffectManagerLogic.EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
		entityEffectsPanel.Visible = true;

		PlayerEntity playerEntity = Allies.FirstOrDefault(character => character.Id == characterID);
		if (playerEntity != null)
		{
			entityEffectsPanel.SetEffectsInfos(playerEntity.appliedEffects, playerEntity.Weapon.effect);
			entityEffectsPanel.ChangeToFitAndReplace(playerEntity.appliedEffects.Count, playerEntity.Weapon.effect);
			entityEffectsPanel.SetPositionWarriorOffset(fightScenePanel.GetWarriorPanelPositionByID(characterID));
			_mapManager.SelectTileForCurrentPlayerEntityTurn(playerEntity.Tile);
		}
	}

	public void OnCharacterPanelMouseExit(string characterID)
	{
		EffectManagerLogic.EntityEffectsPanel entityEffectsPanel = GetNode<EffectManagerLogic.EntityEffectsPanel>("EffectPanel/EntityEffectsPanel");
		entityEffectsPanel.Visible = false;
		PlayerEntity playerEntity = Allies.FirstOrDefault(character => character.Id == characterID);
		if (playerEntity != null)
		{
			_mapManager.DeselectTile(playerEntity.Tile);
		}
	}

	private void ApplyExperienceByEnemy(EnemyEntity enemyWarrior)
	{
		if (enemyWarrior.EnemyId == GameEnemyCode.Spider)
		{
			GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle += 20;
			GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle += 2;
			GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle += 3;
		}
		else if (enemyWarrior.EnemyId == GameEnemyCode.Wasp)
		{
			GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle += 23;
			GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle += 5;
			GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle += 0;
		}
		else if (enemyWarrior.EnemyId == GameEnemyCode.Root)
		{
			GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle += 15;
			GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle += 3;
			GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle += 0;
		}
		else if (enemyWarrior.EnemyId == GameEnemyCode.Bug)
		{
			GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle += 25;
			GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle += 0;
			GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle += 5;
		}
		else if (enemyWarrior.EnemyId == GameEnemyCode.EvilEye)
		{
			GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle += 40;
			GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle += 10;
			GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle += 0;
		}
		else if (enemyWarrior.EnemyId == GameEnemyCode.Centipede)
		{
			GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle += 80;
			GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle += 10;
			GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle += 20;
		}
		else if (enemyWarrior.EnemyId == GameEnemyCode.Tank)
		{
			GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle += 500;
			GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle += 50;
			GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle += 100;
		}
		else if (enemyWarrior.EnemyId == GameEnemyCode.Vegetable)
		{
			GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle += 500;
			GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle += 150;
			GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle += 0;
		}
		else if (enemyWarrior.EnemyId == GameEnemyCode.SpiderBoss)
		{
			GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle += 500;
			GameDataManager.Instance.currentData.commandBlockData.CrystalsByOneBattle += 100;
			GameDataManager.Instance.currentData.commandBlockData.ChitinFragmentsByOneBattle += 75;
		}

		// GD.Print(GameDataManager.Instance.currentData.commandBlockData.ExperienceByOneBattle);
	}
}
