using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerWarriorsMenu : HBoxContainer
{
    [Export] private VBoxContainer _teamWarriorsContainer { get; set; }
    [Export] private PackedScene _warriorPanel { get; set; }
    
    private readonly Dictionary<CharacterData, PrepareFightWarriorPanel> _characters = new();
    
    [Signal]
    public delegate void OnCharacterDataClickedEventHandler(PrepareFightWarriorPanel warriorPanel);


    public CharacterData GetCharacterDataByPrepareFightWarriorPanel(PrepareFightWarriorPanel warriorPanel)
    {
        return _characters.FirstOrDefault(x => x.Value == warriorPanel).Key;
    }
    
    public void AddWarriorIconToContainer(CharacterData warrior)
    {
        var characterView = _warriorPanel.Instantiate<PrepareFightWarriorPanel>();
        _characters.Add(warrior, characterView);
        _teamWarriorsContainer.AddChild(characterView);
        characterView.OnWarriorPanelClicked += EmitSignalOnCharacterDataClicked;
    }

    public void RemoveWarriorIconFromContainer(CharacterData warrior)
    {
        var characterView = _characters[warrior];
        _teamWarriorsContainer.RemoveChild(characterView);
        _characters.Remove(warrior);
        characterView.Dispose();
        
    }
}
