using Godot;
using System;

public partial class FiredWarriorPanel : Control
{
    public override void _Ready()
    {
        var FiredButton = GetNode<Button>("PanelContainer/TextureRect/HBoxContainer/MarginContainer2/FiredWarriorButton");
        FiredButton.ButtonDown += OnFiredButtonPressed;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
            {
                Warrior warrior = GetNode<Warrior>("PanelContainer/TextureRect/HBoxContainer/MarginContainer/WarriorPanel");
                warrior.ReplaceCharacter();
            }
        }
    }

    private void OnFiredButtonPressed()
    {
        Warrior warrior = GetNode<Warrior>("PanelContainer/TextureRect/HBoxContainer/MarginContainer/WarriorPanel");
        GameDataManager.Instance.livingSpaceDataManager.TryDeleteCharacterFromUsed(warrior.characterData.ID);
        GameDataManager.Instance.livingSpaceDataManager.TryDeleteCharacterFromReserved(warrior.characterData.ID);
    }
    
    public void SetCharacterInfosToWarriorPanel(CharacterData InCharacterData)
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/TextureRect/HBoxContainer/MarginContainer/WarriorPanel");
		warrior.SetCharacterInfos(InCharacterData);
	}
}
