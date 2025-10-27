using Godot;
using System;

public partial class ViewWarriorPanel : Control
{
    public override void _Ready()
    {
        
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
            {
                Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
                warrior.ReplaceCharacter();
            }
        }
    }

    public void SetCharacterInfosToWarriorPanel(CharacterData InCharacterData)
    {
        Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
        warrior.SetCharacterInfos(InCharacterData);
    }
}
