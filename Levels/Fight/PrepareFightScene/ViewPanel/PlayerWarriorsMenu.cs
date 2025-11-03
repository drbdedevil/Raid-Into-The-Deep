#nullable enable
using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.PrepareFightScene.ViewPanel;

/// <summary>
/// Является 
/// </summary>
public partial class PlayerWarriorsMenu : HBoxContainer
{
    [Export] private VBoxContainer _teamWarriorsContainer { get; set; }
    [Export] private VBoxContainer _currentSelectedWarriorContainer { get; set; }
    [Export] private PackedScene _warriorPanel { get; set; }
    
    public PrepareFightMapManager MapManager { get; set; }
    
    
    
    private PlayerEntity? _currentSelectedPlayerEntity;
    
    private readonly Dictionary<PlayerEntity, PrepareFightWarriorPanel> _allPlayerEntity = new();
    
    private readonly Dictionary<PlayerEntity, PrepareFightWarriorPanel> _playerEntitiesInTeamContainer = new();

    public static readonly int PlayerUserWarriorsCount = GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters .Count;

    public PlayerEntity? CurrentSelectedPlayerEntity => _currentSelectedPlayerEntity;

    
    private int _placedWarriorsOnMap = 0;
    public bool IsPlayerPlacedAnyoneWarrior => _placedWarriorsOnMap != 0 ;
    
    public void InitTeam(List<CharacterData> characters)
    {
        var playerEntities = characters.Select(x => new PlayerEntity()
        {
            Tile = null,
            ID = x.ID,
        });
        
        foreach (var playerEntity in playerEntities)
        {
            var panel = _warriorPanel.Instantiate<PrepareFightWarriorPanel>();
            panel.SetPlayerEntityData(playerEntity);
            _allPlayerEntity.Add(playerEntity, panel);
            AddWarriorIconToTeamContainer(playerEntity);
        }
    }
    public PlayerEntity GetCharacterDataByPrepareFightWarriorPanel(PrepareFightWarriorPanel warriorPanel)
    {
        return _playerEntitiesInTeamContainer.FirstOrDefault(x => x.Value == warriorPanel).Key;
    }
    
    /// <summary>
    /// Добавление воина на "Команда" контейнер
    /// </summary>
    /// <param name="warrior"></param>
    /// <returns></returns>
    public void AddWarriorIconToTeamContainer(PlayerEntity warrior)
    {
        GD.Print($"Adding {warrior.ID}");
        var characterPanel = _allPlayerEntity[warrior];
        _playerEntitiesInTeamContainer.Add(warrior, characterPanel);
        _teamWarriorsContainer.AddChild(characterPanel);
        characterPanel.OnWarriorPanelLeftButtonClicked += SelectPlayerEntity;
    }

    public void SelectPlayerEntity(PrepareFightWarriorPanel warriorPanel)
    {
        var playerEntity = _playerEntitiesInTeamContainer.FirstOrDefault(x => x.Value == warriorPanel).Key;
        GD.Print($"Adding to selected {playerEntity.ID}");
        RemoveWarriorIconFromTeamContainer(playerEntity);

        if (_currentSelectedPlayerEntity is not null)
        {
            var prevSelectedWarriorPanel = _allPlayerEntity[_currentSelectedPlayerEntity];
            _currentSelectedWarriorContainer.RemoveChild(prevSelectedWarriorPanel);
            AddWarriorIconToTeamContainer(_currentSelectedPlayerEntity);
        }
        _currentSelectedPlayerEntity = playerEntity;
        _currentSelectedWarriorContainer.AddChild(warriorPanel);
        warriorPanel.OnWarriorPanelLeftButtonClicked -= SelectPlayerEntity;
    }

    /// <summary>
    /// Установка в Select персонажа
    /// </summary>
    /// <param name="playerEntity"></param>
    public void SelectPlayerEntityFromMap(PlayerEntity playerEntity)
    {
        var warriorPanel = _allPlayerEntity[playerEntity];
        
        if (_currentSelectedPlayerEntity is not null)
        {
            var prevSelectedWarriorPanel = _allPlayerEntity[_currentSelectedPlayerEntity];
            _currentSelectedWarriorContainer.RemoveChild(prevSelectedWarriorPanel);
            AddWarriorIconToTeamContainer(_currentSelectedPlayerEntity);
        }
        _currentSelectedPlayerEntity = playerEntity;
        _currentSelectedWarriorContainer.AddChild(warriorPanel);
        warriorPanel.OnWarriorPanelLeftButtonClicked -= SelectPlayerEntity;
        MapManager.RemoveEnemyOnTile(playerEntity.Tile);
        _placedWarriorsOnMap -= 1;
    }
    
    public void RemoveWarriorIconFromTeamContainer(PlayerEntity warrior)
    {
        GD.Print($"remove from Team Container {warrior.ID}");
        var characterView = _playerEntitiesInTeamContainer[warrior];
        _teamWarriorsContainer.RemoveChild(characterView);
        _playerEntitiesInTeamContainer.Remove(warrior);
    }

    public void SetPlayerEntityOnMap(Vector2I tileCartesianPosition)
    {
        if (_currentSelectedPlayerEntity is null) return;
        var tile = MapManager.GetTileByCartesianCoord(tileCartesianPosition);
        MapManager.SetBattleEntityOnTile(tile!, new PlayerEntity()
        {
            ID = _currentSelectedPlayerEntity.ID,
        });
        _currentSelectedWarriorContainer.RemoveChild(_allPlayerEntity[_currentSelectedPlayerEntity]);
        _currentSelectedPlayerEntity = null;
        if (_playerEntitiesInTeamContainer.Any()) SelectPlayerEntity(_playerEntitiesInTeamContainer.First().Value);
        _placedWarriorsOnMap += 1;
    }
}