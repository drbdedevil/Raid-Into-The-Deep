#nullable enable
using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using BattleEntity = RaidIntoTheDeep.Levels.Fight.BattleEntity;

namespace RaidIntoTheDeep.Levels.Fight.PrepareFightScene;

public partial class PrepareFightManager : Control
{
	private static NotificationSystem _notificationSystem = NotificationSystem.Instance;
	
	private PrepareFightMapManager _mapManager;
	private ViewPanel.PlayerWarriorsMenu _playerTeamWarriorsMenu;
	
	[Export]
	private PackedScene _fightScene;
	
	private Button _goFightButton;
	
	public override void _Ready()
	{
		_playerTeamWarriorsMenu = GetNode<ViewPanel.PlayerWarriorsMenu>("PlayerWarriorsMenu");
		
		_playerTeamWarriorsMenu.InitTeam(GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters);
		_mapManager = GetNode<PrepareFightScene.PrepareFightMapManager>("Map");
		_mapManager.OnTileRightButtonClicked += _playerTeamWarriorsMenu.SelectPlayerEntityFromMap;
		_mapManager.OnTileLeftButtonClicked += _playerTeamWarriorsMenu.SetPlayerEntityOnMap;
		_playerTeamWarriorsMenu.MapManager = _mapManager;
		
		
		_goFightButton = GetNode<Button>("GoFightButton");
		_goFightButton.ButtonDown += GoToFightScene;

	}

	private void GoToFightScene()
	{
		if (!_playerTeamWarriorsMenu.IsPlayerPlacedAnyoneWarrior)
		{
			_notificationSystem.ShowMessage("А сражаться то кто будет?", EMessageType.Alert);
			return;
		}
		BattleMapInitStateManager.Instance.SetMapData(_mapManager.MapTiles);
		GetTree().ChangeSceneToPacked(_fightScene);
	}
}
