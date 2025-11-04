using Godot;
using System;
using System.Linq;
using System.Text.Json;

public enum EForgeAction
{
	Melt,
	Shackle
}


public partial class Forge : Node, IStackPage
{
	[Export]
	public PackedScene ChooseWeaponScene;

	private WeaponData chosenWeaponDataForMelt = new();
	private WeaponData chosenWeaponDataForShackle = new();
	private bool bIsWeaponChosenForMelt = false;
	private bool bIsWeaponChosenForShackle = false;

	public override void _Ready()
	{
		var ChooseMeltButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/TextureButton");
		ChooseMeltButton.ButtonDown += OnChooseWeaponForMeltButtonPressed;

		var ChooseShackleButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/TextureButton");
		ChooseShackleButton.ButtonDown += OnChooseWeaponForShackleButtonPressed;

		var MeltButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureButton");
		MeltButton.ButtonDown += OnMeltButtonPressed;
		MeltButton.Disabled = true;
		Label MeltLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureButton/Label");
		MeltLabel.AddThemeColorOverride("font_color", new Color("#707070"));

		var ShackleButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureButton");
		ShackleButton.ButtonDown += OnShackleButtonPressed;
		ShackleButton.Disabled = true;
		Label ShackleLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureButton/Label");
		ShackleLabel.AddThemeColorOverride("font_color", new Color("#707070"));

		var UpgradeButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
		UpgradeButton.ButtonDown += OnForgeUpgradeButtonPressed;

		Button clearMeltButton = GetNode<Button>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/MarginContainer/ClearMeltButton");
		clearMeltButton.ButtonDown += UnchooseWeaponForMelt;

		Button clearShackleButton = GetNode<Button>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/MarginContainer/ClearShackleButton");
		clearShackleButton.ButtonDown += UnchooseWeaponForShackle;
	}
	public override void _ExitTree()
	{
		GameDataManager.Instance.forgeDataManager.OnForgeLevelUpdate -= OnForgeLevelUpdate;
	}

	private void OnChooseWeaponForMeltButtonPressed()
	{
		GD.Print(" -- Choose Button for Melt --");

		Label panelLabel = GetTree().Root.FindChild("PanelLabel", recursive: true, owned: false) as Label;
		panelLabel.Text = "Кузница: Переплавка";

		var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
		WeaponSelector sceneInstance = ChooseWeaponScene.Instantiate() as WeaponSelector;
		sceneInstance.Parent = this;
		sceneInstance.weaponList = EWeaponList.Storage;

		navigator.PushInstance(sceneInstance);
	}
	private void OnChooseWeaponForShackleButtonPressed()
	{
		GD.Print(" -- Choose Button for Shackle --");

		Label panelLabel = GetTree().Root.FindChild("PanelLabel", recursive: true, owned: false) as Label;
		panelLabel.Text = "Кузница: Ковка";

		var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
		WeaponSelector sceneInstance = ChooseWeaponScene.Instantiate() as WeaponSelector;
		sceneInstance.Parent = this;
		sceneInstance.weaponList = EWeaponList.Shackle;

		navigator.PushInstance(sceneInstance);
	}
	private void OnMeltButtonPressed()
	{
		if (bIsWeaponChosenForMelt)
		{
			if (GameDataManager.Instance.storageDataManager.TryDeleteWeapon(chosenWeaponDataForMelt.ID))
			{
				Label chitinLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer/TextureRect/NumberLabel");
				Label crystalLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer2/TextureRect/NumberLabel");
				Int32 CrystalPrice = crystalLabel.Text.ToInt();
				Int32 ChitinPrice = chitinLabel.Text.ToInt();

				GameDataManager.Instance.storageDataManager.AdjustCrystals(CrystalPrice);
				GameDataManager.Instance.storageDataManager.AdjustChitinFragments(ChitinPrice);

				NotificationSystem.Instance.ShowMessage("Оружие \'" + chosenWeaponDataForMelt.Name + "\' успешно переплавлено.");
			}

			UnchooseWeaponForMelt();
		}
	}
	private void OnShackleButtonPressed()
	{
		if (bIsWeaponChosenForShackle)
		{
			var existingWeapon = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == chosenWeaponDataForShackle.Name);
			if (existingWeapon != null)
			{
				Guid.NewGuid().ToString();

				chosenWeaponDataForShackle.ID = Guid.NewGuid().ToString();
				GD.Print(chosenWeaponDataForShackle.ID);
				chosenWeaponDataForShackle.Damage = GD.RandRange(existingWeapon.DamageRange.X, existingWeapon.DamageRange.Y);
				chosenWeaponDataForShackle.EffectID = GetWeightedRandomEffect(); // GD.RandRange(0, GameDataManager.Instance.effectDatabase.Effects.Count - 1);

				Label chitinLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer/TextureRect/NumberLabel");
				Label crystalLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer2/TextureRect/NumberLabel");
				Int32 CrystalPrice = crystalLabel.Text.ToInt();
				Int32 ChitinPrice = chitinLabel.Text.ToInt();

				if (GameDataManager.Instance.storageDataManager.IsCrystalsEnough(CrystalPrice) && GameDataManager.Instance.storageDataManager.IsChitinFragmentsEnough(ChitinPrice))
				{
					if (GameDataManager.Instance.storageDataManager.TryAddWeapon(chosenWeaponDataForShackle))
					{
						GameDataManager.Instance.storageDataManager.AdjustCrystals(-CrystalPrice);
						GameDataManager.Instance.storageDataManager.AdjustChitinFragments(-ChitinPrice);

						NotificationSystem.Instance.ShowMessage("Оружие \'" + existingWeapon.Name + "\' успешно сковано.");
						GD.Print(" -- Successful shackle! -- ");
					}
					else
					{
						NotificationSystem.Instance.ShowMessage("Нет места на складе для: \'" + existingWeapon.Name + "\'.", EMessageType.Warning);
						GD.Print(" -- Can't shackle weapon! -- ");
					}
				}
				else
				{
					NotificationSystem.Instance.ShowMessage("Нечем платить оружейнику - денег нет, но вы держитесь!", EMessageType.Alert);
					GD.Print(" -- Not enough funds for shackle! -- ");
				}
			}

			UnchooseWeaponForShackle();
		}
	}

	private void OnForgeUpgradeButtonPressed()
	{
		var UpgradeButton = GetNode<UpgradeButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
		Int32 CrystalPrice = UpgradeButton.GetCrystalValue();
		Int32 ChitinPrice = UpgradeButton.GetChitinFragmentsValue();

		if (GameDataManager.Instance.storageDataManager.IsCrystalsEnough(CrystalPrice) && GameDataManager.Instance.storageDataManager.IsChitinFragmentsEnough(ChitinPrice))
		{
			if (GameDataManager.Instance.forgeDataManager.TryUpdateLevel())
			{
				GameDataManager.Instance.storageDataManager.AdjustCrystals(-CrystalPrice);
				GameDataManager.Instance.storageDataManager.AdjustChitinFragments(-ChitinPrice);

				GD.Print(" -- Successful upgrade Forge! -- ");
			}
			else
			{
				GD.Print(" -- Level is Max! -- ");
			}
		}
		else
		{
			NotificationSystem.Instance.ShowMessage("Не хватает ресурсов!", EMessageType.Alert);
			GD.Print(" -- Not enough funds to upgrade Forge! -- ");
		}
	}

	private void OnForgeLevelUpdate()
	{
		Label LevelLabel = GetNode<Label>("VBoxContainer/MarginContainer/HBoxContainer/LevelLabel");
		LevelLabel.Text = GameDataManager.Instance.currentData.forgeData.Level.ToString();

		ForgeLevelData currentForgeLevelData = GameDataManager.Instance.forgeDatabase.Levels[GameDataManager.Instance.currentData.forgeData.Level - 1];

		UpgradeButton UpgradeButton = GetNode<UpgradeButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
		UpgradeButton.SetCrystalValue(currentForgeLevelData.CrystalUpgradeCost);
		UpgradeButton.SetChitinFragmentsValue(currentForgeLevelData.ChitinFragmentsUpgradeCost);
		GD.Print(currentForgeLevelData.ChitinsDiscountK);
		GD.Print(currentForgeLevelData.CrystalsDiscountK);
		if (GameDataManager.Instance.forgeDatabase.Levels.Count == GameDataManager.Instance.currentData.forgeData.Level)
		{
			UpgradeButton.Visible = false;
		}

		SetInfoForMelt();
		SetInfoForShackle();
	}

	public void ChooseWeaponData(WeaponPanel InWeaponPanel, EForgeAction forgeAction)
	{
		if (forgeAction == EForgeAction.Melt)
		{
			var json = JsonSerializer.Serialize(InWeaponPanel.weaponData);
			chosenWeaponDataForMelt = JsonSerializer.Deserialize<WeaponData>(json);

			SetInfoForMelt();

			Button clearMeltButton = GetNode<Button>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/MarginContainer/ClearMeltButton");
			clearMeltButton.Visible = true;

			var MeltButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureButton");
			MeltButton.Disabled = false;
			Label MeltLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureButton/Label");
			MeltLabel.AddThemeColorOverride("font_color", new Color("#ffffff"));
		}
		else if (forgeAction == EForgeAction.Shackle)
		{
			var json = JsonSerializer.Serialize(InWeaponPanel.weaponData);
			chosenWeaponDataForShackle = JsonSerializer.Deserialize<WeaponData>(json);

			SetInfoForShackle();

			Button clearShackleButton = GetNode<Button>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/MarginContainer/ClearShackleButton");
			clearShackleButton.Visible = true;

			var ShackleButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureButton");
			ShackleButton.Disabled = false;
			Label ShackleLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureButton/Label");
		ShackleLabel.AddThemeColorOverride("font_color", new Color("#ffffff"));
		}
	}
	private void SetInfoForMelt()
	{
		if (chosenWeaponDataForMelt.ID != "NONE")
		{
			var existingWeapon = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == chosenWeaponDataForMelt.Name);
			if (existingWeapon != null)
			{
				bIsWeaponChosenForMelt = true;
				TextureButton textureButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/TextureButton");
				textureButton.TextureNormal = existingWeapon.WeaponTexture;

				ShowMeltPrice(existingWeapon.CrystalYield, existingWeapon.ChitinFragmentsYield);
			}
		}
	}
	private void SetInfoForShackle()
	{
		if (chosenWeaponDataForShackle.ID != "NONE")
		{
			var existingWeapon = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == chosenWeaponDataForShackle.Name);
			if (existingWeapon != null)
			{
				bIsWeaponChosenForShackle = true;
				TextureButton textureButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/TextureButton");
				textureButton.TextureNormal = existingWeapon.WeaponTexture;

				ShowShacklePrice(existingWeapon.CrystalCost, existingWeapon.ChitinFragmentsCost);
			}
		}
	}

	public void UnchooseWeaponForShackle()
	{
		HideShacklePrice();

		Texture2D clickButtonTexture = GD.Load<Texture2D>("res://Textures/HubLocation/Forge/ClickButton.png");

		chosenWeaponDataForShackle = new WeaponData();

		TextureButton textureButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/TextureButton");
		textureButton.TextureNormal = clickButtonTexture;

		bIsWeaponChosenForShackle = false;

		Button clearShackleButton = GetNode<Button>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/MarginContainer/ClearShackleButton");
		clearShackleButton.Visible = false;

		var ShackleButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureButton");
		ShackleButton.Disabled = true;
		Label ShackleLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureButton/Label");
		ShackleLabel.AddThemeColorOverride("font_color", new Color("#707070"));
	}
	public void UnchooseWeaponForMelt()
	{
		HideMeltPrice();

		Texture2D clickButtonTexture = GD.Load<Texture2D>("res://Textures/HubLocation/Forge/ClickButton.png");

		chosenWeaponDataForMelt = new WeaponData();

		TextureButton textureButton2 = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/TextureButton");
		textureButton2.TextureNormal = clickButtonTexture;

		bIsWeaponChosenForMelt = false;

		Button clearMeltButton = GetNode<Button>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer3/MarginContainer/ClearMeltButton");
		clearMeltButton.Visible = false;

		var MeltButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureButton");
		MeltButton.Disabled = true;
		Label MeltLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureButton/Label");
		MeltLabel.AddThemeColorOverride("font_color", new Color("#707070"));
	}

	private void HideMeltPrice()
	{
		Label chitinLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer/TextureRect/NumberLabel");
		chitinLabel.Text = "-";

		Label crystalLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer2/TextureRect/NumberLabel");
		crystalLabel.Text = "-";
	}
	private void ShowMeltPrice(int Crystals, int ChitinFragments)
	{
		ForgeLevelData forgeLevelData = GameDataManager.Instance.forgeDatabase.Levels[GameDataManager.Instance.currentData.forgeData.Level - 1];

		Label chitinLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer/TextureRect/NumberLabel");
		int chitinResult = (int)Math.Ceiling(ChitinFragments * forgeLevelData.ChitinsIncreaseK);
		chitinLabel.Text = chitinResult.ToString();

		Label crystalLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/HBoxContainer2/TextureRect/NumberLabel");
		int crystalReslut = (int)Math.Ceiling(Crystals * forgeLevelData.CrystalsIncreaseK);
		crystalLabel.Text = crystalReslut.ToString();
	}
	private void HideShacklePrice()
	{
		Label chitinLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer/TextureRect/NumberLabel");
		chitinLabel.Text = "-";

		Label crystalLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer2/TextureRect/NumberLabel");
		crystalLabel.Text = "-";
	}
	private void ShowShacklePrice(int Crystals, int ChitinFragments)
	{
		ForgeLevelData forgeLevelData = GameDataManager.Instance.forgeDatabase.Levels[GameDataManager.Instance.currentData.forgeData.Level - 1];

		Label chitinLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer/TextureRect/NumberLabel");
		int chitinResult = (int)Math.Floor(ChitinFragments * forgeLevelData.ChitinsDiscountK);
		chitinLabel.Text = chitinResult.ToString();

		Label crystalLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/HBoxContainer2/TextureRect/NumberLabel");
		int crystalResult = (int)Math.Floor(Crystals * forgeLevelData.CrystalsDiscountK);
		crystalLabel.Text = crystalResult.ToString();
	}

	private int GetWeightedRandomEffect()
	{
		var effects = GameDataManager.Instance.effectDatabase.Effects;

		float totalWeight = 0f;
		foreach (var effect in effects)
		{
			totalWeight += effect.Weight;
		}

		double randomValue = GD.RandRange(0f, totalWeight);

		float currentHeight = 0f;
		for (int i = 0; i < effects.Count; ++i)
		{
			currentHeight += effects[i].Weight;
			if (randomValue <= currentHeight)
			{
				return i;
			}
		}
		
		return 0;
	}

	public void OnShow()
	{
		// ----------- View Realization -----------
		// ----- Binding Functions
		GameDataManager.Instance.forgeDataManager.OnForgeLevelUpdate += OnForgeLevelUpdate;

		// ----- Set Init Value
		OnForgeLevelUpdate();

		GD.Print("Forge Popup shown");
	}
	public void OnHide()
	{
		GameDataManager.Instance.forgeDataManager.OnForgeLevelUpdate -= OnForgeLevelUpdate;

		GD.Print("Forge Popup hidden");
	}
}
