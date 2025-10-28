using Godot;
using System;

public enum EWeaponList
{
	None,
	Storage,
	Shackle
}

public partial class WeaponSelector : Node, IStackPage
{
	[Signal]
	public delegate void RequestBackEventHandler();
	[Export]
	public PackedScene WeaponPanelScene;

	public Node Parent = null;
	public EWeaponList weaponList = EWeaponList.None;

	public override void _Ready()
	{
		var BackButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HBoxContainer/TextureButton");
		BackButton.ButtonDown += OnBackButtonPressed;

		FillWeaponContainer();
		AssignWeaponPanelButtons();

		GD.Print("Weapon Selector Ready");
	}

	private void OnBackButtonPressed()
	{
		if (Parent is Forge forge)
		{
			var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
			navigator.SetPanelLabelName("Кузница");

			GD.Print("Back to Forge");
		}
		else if (Parent is CharacterList characterList)
		{
			GD.Print("Back to Character List");
		}

		EmitSignal(SignalName.RequestBack);
	}
	private void OnWeaponPanelButtonPressed(WeaponPanel InWeaponPanel)
	{
		if (Parent is Forge forge)
		{
			if (weaponList == EWeaponList.Storage)
			{
				forge.ChooseWeaponData(InWeaponPanel, EForgeAction.Melt);
			}
			else if (weaponList == EWeaponList.Shackle)
			{
				forge.ChooseWeaponData(InWeaponPanel, EForgeAction.Shackle);
			}
		}
		else if (Parent is CharacterList characterList)
		{
			if (weaponList == EWeaponList.Storage)
			{
				characterList.ChooseWeaponData(InWeaponPanel);
			}

		}
		OnBackButtonPressed();
	}

	private void FillWeaponContainer()
	{
		GridContainer gridContainer = GetNode<GridContainer>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/ScrollContainer/GridContainer");
		foreach (Node child in gridContainer.GetChildren())
		{
			child.QueueFree();
		}

		if (weaponList == EWeaponList.Storage)
		{
			foreach (WeaponData weaponData in GameDataManager.Instance.currentData.storageData.Weapons)
			{
				WeaponPanel weaponPanel = WeaponPanelScene.Instantiate() as WeaponPanel;
				weaponPanel.HideRangeDamage();
				weaponPanel.SetWeaponInfos(weaponData);
				weaponPanel.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;
				gridContainer.AddChild(weaponPanel);
			}
		}
		else if (weaponList == EWeaponList.Shackle)
		{
			foreach (WeaponData weaponData in GameDataManager.Instance.currentData.forgeData.WeaponsForShackle)
			{
				WeaponPanel weaponPanel = WeaponPanelScene.Instantiate() as WeaponPanel;
				weaponPanel.SetWeaponInfosShackle(weaponData);
				weaponPanel.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;
				gridContainer.AddChild(weaponPanel);
			}
		}
	}
	private void AssignWeaponPanelButtons()
	{
		GridContainer gridContainer = GetNode<GridContainer>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/ScrollContainer/GridContainer");
		foreach (Node child in gridContainer.GetChildren())
		{
			if (child is WeaponPanel weaponPanel)
			{
				if (weaponPanel != null && !child.IsQueuedForDeletion())
				{
					GD.Print(weaponPanel.weaponData.ID);
					if (Parent is Forge forge)
					{
						weaponPanel.OnWeaponPanelPressed += OnWeaponPanelButtonPressed;
					}
					else if (Parent is CharacterList characterList)
					{
						weaponPanel.OnWeaponPanelPressed += OnWeaponPanelButtonPressed;
					}
				}
			}
		}
	}

	public void OnShow()
	{
		GD.Print("WeaponSelector Popup shown");
	}
	public void OnHide()
	{
		GD.Print("WeaponSelector Popup hidden");
	}
}
