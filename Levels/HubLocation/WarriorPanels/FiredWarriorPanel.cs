using Godot;
using System;

public partial class FiredWarriorPanel : Control
{
    public override void _Ready()
    {
        var FiredButton = GetNode<Button>("PanelContainer/TextureRect/HBoxContainer/MarginContainer2/FiredWarriorButton");
        FiredButton.ButtonDown += OnFiredButtonPressed;

        Warrior warrior = GetNode<Warrior>("PanelContainer/TextureRect/HBoxContainer/MarginContainer/WarriorPanel");
		SkillStar skillStar = GetNode<SkillStar>("PanelContainer/TextureRect/HBoxContainer/MarginContainer/MarginContainer2/SkillStar");
		if (warrior.characterData.SkillPoints > 0)
		{
			skillStar.Start();
		}
		else
		{
			skillStar.Stop();
		}
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

        NotificationSystem.Instance.ShowMessage("Воин \'" + warrior.characterData.Name + "\' уволен с позором.");

        SoundManager.Instance.PlaySoundOnce("res://Sound/Interface/FireAnWarrior.wav");
    }
    
    public void SetCharacterInfosToWarriorPanel(CharacterData InCharacterData)
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/TextureRect/HBoxContainer/MarginContainer/WarriorPanel");
		warrior.SetCharacterInfos(InCharacterData);
	}
}
