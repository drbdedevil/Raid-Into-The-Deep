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
    
    
    
    private CharacterData? _currentSelectedCharacterData;
    
    private readonly Dictionary<CharacterData, PrepareFightWarriorPanel> _allCharacters = new();
    
    private readonly Dictionary<CharacterData, PrepareFightWarriorPanel> _charactersInTeamContainer = new();

    public CharacterData? CurrentSelectedCharacterData => _currentSelectedCharacterData;

    public void InitTeam(List<CharacterData> characters)
    {
        foreach (var character in characters)
        {
            var panel = _warriorPanel.Instantiate<PrepareFightWarriorPanel>();
            panel.SetCharacterData(character);
            _allCharacters.Add(character, panel);
            AddWarriorIconToTeamContainer(character);
        }
    }
    public CharacterData GetCharacterDataByPrepareFightWarriorPanel(PrepareFightWarriorPanel warriorPanel)
    {
        return _charactersInTeamContainer.FirstOrDefault(x => x.Value == warriorPanel).Key;
    }
    
    /// <summary>
    /// Добавление воина на "Команда" контейнер
    /// </summary>
    /// <param name="warrior"></param>
    /// <returns></returns>
    public void AddWarriorIconToTeamContainer(CharacterData warrior)
    {
        GD.Print($"Adding {warrior.ID}");
        var characterPanel = _allCharacters[warrior];
        _charactersInTeamContainer.Add(warrior, characterPanel);
        _teamWarriorsContainer.AddChild(characterPanel);
        characterPanel.OnWarriorPanelLeftButtonClicked += SelectCharacter;
    }

    public void SelectCharacter(PrepareFightWarriorPanel warriorPanel)
    {
        var characterData = _charactersInTeamContainer.FirstOrDefault(x => x.Value == warriorPanel).Key;
        GD.Print($"Adding to selected {characterData.ID}");
        RemoveWarriorIconFromTeamContainer(characterData);

        if (_currentSelectedCharacterData is not null)
        {
            var prevSelectedWarriorPanel = _allCharacters[_currentSelectedCharacterData];
            _currentSelectedWarriorContainer.RemoveChild(prevSelectedWarriorPanel);
            AddWarriorIconToTeamContainer(_currentSelectedCharacterData);
        }
        _currentSelectedCharacterData = characterData;
        _currentSelectedWarriorContainer.AddChild(warriorPanel);
        warriorPanel.OnWarriorPanelLeftButtonClicked -= SelectCharacter;
    }

    /// <summary>
    /// Установка в Select персонажа
    /// </summary>
    /// <param name="battleEntity"></param>
    public void SelectCharacterFromMap(BattleEntity battleEntity)
    {
        var characterData = battleEntity.Character;
        var warriorPanel = _allCharacters[characterData];
        
        if (_currentSelectedCharacterData is not null)
        {
            var prevSelectedWarriorPanel = _allCharacters[_currentSelectedCharacterData];
            _currentSelectedWarriorContainer.RemoveChild(prevSelectedWarriorPanel);
            AddWarriorIconToTeamContainer(_currentSelectedCharacterData);
        }
        _currentSelectedCharacterData = characterData;
        _currentSelectedWarriorContainer.AddChild(warriorPanel);
        warriorPanel.OnWarriorPanelLeftButtonClicked -= SelectCharacter;
        MapManager.RemoveEnemyOnTile(battleEntity.Tile);
    }
    
    public void RemoveWarriorIconFromTeamContainer(CharacterData warrior)
    {
        GD.Print($"remove from Team Container {warrior.ID}");
        var characterView = _charactersInTeamContainer[warrior];
        _teamWarriorsContainer.RemoveChild(characterView);
        _charactersInTeamContainer.Remove(warrior);
    }

    public void SetCharacterDataOnMap(Vector2I tileCartesianPosition)
    {
        if (_currentSelectedCharacterData is null) return;
        var tile = MapManager.GetTileByCartesianCoord(tileCartesianPosition);
        MapManager.SetEnemyOnTile(tile!, new BattleEntity()
        {
            Character = _currentSelectedCharacterData
        });
        _currentSelectedWarriorContainer.RemoveChild(_allCharacters[_currentSelectedCharacterData]);
        _currentSelectedCharacterData = null;
        if (_charactersInTeamContainer.Any()) SelectCharacter(_charactersInTeamContainer.First().Value);
    }
}