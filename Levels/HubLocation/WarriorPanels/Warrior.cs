using Godot;
using System;

public partial class Warrior : Node, IStackPage
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
		GD.Print("1");
		var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
		GD.Print("2");
		CharacterList characterList = CharacterListScene.Instantiate() as CharacterList;
		GD.Print("3");

		if (!navigator.IsSomethingOpen())
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
			GD.Print("4");
			characterList.Parent = navigator.GetCurrent();

			navigator.PushInstance(characterList);

			var hBoxContainer = navigator.GetTree().Root.FindChild("HiddenPanel", recursive: true, owned: false) as HBoxContainer;
			hBoxContainer.Modulate = new Color(1, 1, 1, 1);
			hBoxContainer.ProcessMode = ProcessModeEnum.Always;
			GD.Print("5");
		}
	}

	public void OnShow()
	{
		var ListButton = GetNode<Button>("TextureRect/HBoxContainer/MarginContainer/TextureRect/MarginContainer/Button");
		ListButton.ReleaseFocus();
		// ListButton.ButtonDown += OnListButtonPressed;
	}
	public void OnHide()
	{
		// var ListButton = GetNode<Button>("TextureRect/HBoxContainer/MarginContainer/TextureRect/MarginContainer/Button");
		// ListButton.ButtonDown -= OnListButtonPressed;
	}
}
