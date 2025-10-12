using Godot;
using System;

public partial class Warrior : Node
{
	[Export]
	public PackedScene CharacterListScene;
	
	public override void _Ready()
	{
		var ListButton = GetNode<Button>("TextureRect/HBoxContainer/MarginContainer/TextureRect/MarginContainer/Button");
		ListButton.ButtonDown += OnListButtonPressed;
	}

	private void OnListButtonPressed()
	{
		var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
		CharacterList characterList = CharacterListScene.Instantiate() as CharacterList;

		if (navigator.IsHistoryEmpty())
		{
			navigator.PushInstance(characterList);
			navigator.Popup();

			var PanelLabel = GetTree().Root.FindChild("PanelLabel", recursive: true, owned: false) as Label;
			PanelLabel.Text = "Лист персонажа".StripEdges();

			var hBoxContainer = GetTree().Root.FindChild("HiddenPanel", recursive: true, owned: false) as HBoxContainer;
			hBoxContainer.Modulate = new Color(1, 1, 1, 0);
			hBoxContainer.ProcessMode = ProcessModeEnum.Disabled;

			characterList.Parent = characterList;
		}
		else
		{
			characterList.Parent = navigator.GetCurrent();

			navigator.PushInstance(characterList);

			var hBoxContainer = GetTree().Root.FindChild("HiddenPanel", recursive: true, owned: false) as HBoxContainer;
			hBoxContainer.Modulate = new Color(1, 1, 1, 1);
			hBoxContainer.ProcessMode = ProcessModeEnum.Always;
		}
	}
}
