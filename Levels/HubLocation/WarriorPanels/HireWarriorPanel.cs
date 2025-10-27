using Godot;
using System;

public partial class HireWarriorPanel : Node
{
	public override void _Ready()
	{
		var HireButton = GetNode<TextureButton>("PanelContainer/MarginContainer2/HBoxContainer/HireButton");
		HireButton.ButtonDown += OnHireButtonPressed;

		var RejectButton = GetNode<TextureButton>("PanelContainer/MarginContainer2/HBoxContainer/RejectButton");
		RejectButton.ButtonDown += OnRejectButtonPressed;
	}

	private void OnHireButtonPressed()
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
		if (GameDataManager.Instance.livingSpaceDataManager.TryAddCharacterToReserved(warrior.characterData))
		{
			GameDataManager.Instance.trainingPitsDataManager.TryDeleteCharacterForHiring(warrior.characterData.ID);
		}
	}
	private void OnRejectButtonPressed()
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
		GameDataManager.Instance.trainingPitsDataManager.TryDeleteCharacterForHiring(warrior.characterData.ID);
	}
	
	public void SetCharacterInfosToWarriorPanel(CharacterData InCharacterData)
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
		warrior.SetCharacterInfos(InCharacterData);
		warrior.DebugCharacterInfos();
	}
}
