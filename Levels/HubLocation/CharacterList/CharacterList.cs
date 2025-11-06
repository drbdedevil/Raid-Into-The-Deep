using Godot;
using System;

public partial class CharacterList : ColorRect, IStackPage
{
	[Export]
	public PackedScene ChooseWeaponScene;
	[Export]
	public PackedScene SkillTreeScene;
	[Export]
	public PackedScene noWeaponPanelScene;
	[Export]
	public PackedScene weaponPanelScene;
	[Export]
	public PackedScene noSkillPanelScene;
	[Export]
	public PackedScene skillPanelScene;
	[Signal]
	public delegate void RequestBackEventHandler();

	public Node Parent = null;
	public Warrior warriorOwner { get; set; } = null;

	private WeaponData chosenWeaponData = new();
	private bool bIsWeaponChosen = false;
	
	public override void _Ready()
	{
		var WeaponButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect3/VBoxContainer/MarginContainer/HBoxContainer/TextureButton");
		WeaponButton.ButtonDown += OnChooseWeaponButtonPressed;

		var SkillButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect4/VBoxContainer/MarginContainer/HBoxContainer/TextureButton");
		SkillButton.ButtonDown += OnSkillTreeButtonPressed;

		var BackButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HiddenPanel/TextureButton");
		BackButton.ButtonDown += OnBackButtonPressed;

		TextureButton removeWeaponButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect3/VBoxContainer/MarginContainer/HBoxContainer/TextureButton2");
		removeWeaponButton.ButtonDown += OnRemoveWeaponButtonPressed;

		TextureButton removeSkillButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect4/VBoxContainer/MarginContainer/HBoxContainer/TextureButton2");
		removeSkillButton.ButtonDown += OnRemoveSkillButtonPressed;
	}

	private void OnChooseWeaponButtonPressed()
	{
		var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
		WeaponSelector sceneInstance = ChooseWeaponScene.Instantiate() as WeaponSelector;
		sceneInstance.Parent = this;
		sceneInstance.weaponList = EWeaponList.Storage;

		navigator.PushInstance(sceneInstance);
	}
	private void OnSkillTreeButtonPressed()
	{
		var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
		SkillTreeWidnow sceneInstance = SkillTreeScene.Instantiate() as SkillTreeWidnow;
		sceneInstance.Parent = this;
		
		navigator.PushInstance(sceneInstance);
	}
	private void OnBackButtonPressed()
	{
		EmitSignal(SignalName.RequestBack);
	}
	private void OnRemoveWeaponButtonPressed()
	{
		if (GameDataManager.Instance.storageDataManager.TryAddWeapon(warriorOwner.characterData.Weapon))
		{
			warriorOwner.characterData.Weapon = new WeaponData();
		}
		else
		{
			NotificationSystem.Instance.ShowMessage("Не получится забрать оружие.\nНет места на складе...", EMessageType.Warning);
			GD.Print("NO PLACE IN STORAGE");
		}
		CheckWeaponInfos();
	}
	private void OnRemoveSkillButtonPressed()
	{
		warriorOwner.characterData.ChoosenSkills = "NONE";
		CheckSkillInfos();
	}

	public void ShowCharacterInfos()
	{
		if (warriorOwner != null)
		{
			Label LevelLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer/TextureRect/NumberLabel");
			Label HealthLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer2/TextureRect/NumberLabel");
			Label SpeedLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer3/TextureRect/NumberLabel");
			Label DamageLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer4/TextureRect/NumberLabel");
			Label DamageByEffectLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer5/TextureRect/NumberLabel");
			Label HealLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer6/TextureRect/NumberLabel");

			LevelLabel.Text = warriorOwner.characterData.Level.ToString();
			HealthLabel.Text = warriorOwner.characterData.Health.ToString();
			SpeedLabel.Text = warriorOwner.characterData.Speed.ToString();
			DamageLabel.Text = warriorOwner.characterData.Damage.ToString();
			DamageByEffectLabel.Text = warriorOwner.characterData.DamageByEffect.ToString();
			HealLabel.Text = warriorOwner.characterData.Heal.ToString();

			TextureRect characterTexture = GetNode<TextureRect>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect/MarginContainer/ColorRect/VBoxContainer/MarginContainer/TextureRect");
			characterTexture.Texture = GameDataManager.Instance.charactersSpritesDatabase.CharactersSpritesArray[warriorOwner.characterData.PortraitID];
			Label NameLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect/MarginContainer/ColorRect/VBoxContainer/NameLabel");
			NameLabel.Text = warriorOwner.characterData.Name;

			CheckWeaponInfos();
			CheckSkillInfos();
		}
	}
	private void CheckWeaponInfos()
	{
		MarginContainer marginContainer = GetNode<MarginContainer>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect3/VBoxContainer/MarginContainer2/ColorRect/MarginContainer");
		foreach (Node child in marginContainer.GetChildren())
		{
			child.QueueFree();
		}

		TextureButton removeWeaponButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect3/VBoxContainer/MarginContainer/HBoxContainer/TextureButton2");
		TextureButton chooseWeaponButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect3/VBoxContainer/MarginContainer/HBoxContainer/TextureButton");
		if (warriorOwner.characterData.Weapon.ID == "NONE")
		{
			bIsWeaponChosen = false;
			Control noWeaponPanel = noWeaponPanelScene.Instantiate() as Control;
			marginContainer.AddChild(noWeaponPanel);

			removeWeaponButton.Visible = false;

			if (GameDataManager.Instance.trainingPitsDataManager.IsCharacterInHiringList(warriorOwner.characterData.ID))
			{
				chooseWeaponButton.Disabled = true;
			}
		}
		else
		{
			bIsWeaponChosen = true;
			BigWeaponPanel bigWeaponPanel = weaponPanelScene.Instantiate() as BigWeaponPanel;
			bigWeaponPanel.SetWeaponInfos(warriorOwner.characterData.Weapon);
			marginContainer.AddChild(bigWeaponPanel);

			if (GameDataManager.Instance.trainingPitsDataManager.IsCharacterInHiringList(warriorOwner.characterData.ID))
			{
				chooseWeaponButton.Disabled = true;
				removeWeaponButton.Visible = false;
			}
			else
			{
				chooseWeaponButton.Disabled = false;
				removeWeaponButton.Visible = true;
			}
		}
	}
	private void CheckSkillInfos()
	{
		MarginContainer marginContainer = GetNode<MarginContainer>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect4/VBoxContainer/MarginContainer2/ColorRect/MarginContainer");
		foreach (Node child in marginContainer.GetChildren())
		{
			child.QueueFree();
		}

		TextureButton removeSkillButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect4/VBoxContainer/MarginContainer/HBoxContainer/TextureButton2");
		TextureButton chooseSkillButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect4/VBoxContainer/MarginContainer/HBoxContainer/TextureButton");
		if (warriorOwner.characterData.ChoosenSkills == "NONE")
		{
			Control noSkillPanel = noSkillPanelScene.Instantiate() as Control;
			marginContainer.AddChild(noSkillPanel);

			removeSkillButton.Visible = false;

			if (GameDataManager.Instance.trainingPitsDataManager.IsCharacterInHiringList(warriorOwner.characterData.ID))
			{
				chooseSkillButton.Disabled = true;
			}
		}
		else
		{
			BigSkillPanel bigSkillPanel = skillPanelScene.Instantiate() as BigSkillPanel;
			bigSkillPanel.SetSkillInfos(warriorOwner.characterData.ChoosenSkills);
			marginContainer.AddChild(bigSkillPanel);

			if (GameDataManager.Instance.trainingPitsDataManager.IsCharacterInHiringList(warriorOwner.characterData.ID))
			{
				chooseSkillButton.Disabled = true;
				removeSkillButton.Visible = false;
			}
			else
			{
				chooseSkillButton.Disabled = false;
				removeSkillButton.Visible = true;
			}
		}
	}
	
	public void ChooseWeaponData(WeaponPanel InWeaponPanel)
	{
		if (GameDataManager.Instance.storageDataManager.TryDeleteWeapon(InWeaponPanel.weaponData.ID))
		{
			if (warriorOwner.characterData.Weapon.ID != "NONE")
			{
				GameDataManager.Instance.storageDataManager.TryAddWeapon(warriorOwner.characterData.Weapon);
			}
			warriorOwner.characterData.Weapon = InWeaponPanel.weaponData;
		}
		CheckWeaponInfos();
	}

	public void OnShow()
	{
		ShowCharacterInfos();
		GD.Print("CharacterList Popup shown");
	}
	public void OnHide()
	{
		GD.Print("CharacterList Popup hidden");
	}
}
