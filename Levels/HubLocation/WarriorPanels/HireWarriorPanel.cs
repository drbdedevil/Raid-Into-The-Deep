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
			if (GameDataManager.Instance.trainingPitsDataManager.TryDeleteCharacterForHiring(warrior.characterData.ID))
			{
				NotificationSystem.Instance.ShowMessage("Воин \'" + warrior.characterData.Name + "\' успешно нанят.");
			}
		}
		else
		{
			NotificationSystem.Instance.ShowMessage("Не получилось нанять \'" + warrior.characterData.Name + "\'. Вероятно, нет места в жилом помещении.");
		}
	}
	private void OnRejectButtonPressed()
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
		GameDataManager.Instance.trainingPitsDataManager.TryDeleteCharacterForHiring(warrior.characterData.ID);

		NotificationSystem.Instance.ShowMessage("Воину \'" + warrior.characterData.Name + "\' отказано в найме.");
	}
	
	public void SetCharacterInfosToWarriorPanel(CharacterData InCharacterData)
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
		warrior.SetCharacterInfos(InCharacterData);
		warrior.DebugCharacterInfos();
	}
}
