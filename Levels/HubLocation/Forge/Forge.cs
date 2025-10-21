using Godot;
using System;

public partial class Forge : Node, IStackPage
{
    [Export]
	public PackedScene ChooseWeaponScene;

    public override void _Ready()
    {
        var ChooseMeltButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureRect/VBoxContainer/TextureButton");
        ChooseMeltButton.ButtonDown += OnChooseWeaponForMeltButtonPressed;

        var ChooseShackleButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureRect/VBoxContainer/TextureButton");
        ChooseShackleButton.ButtonDown += OnChooseWeaponForShackleButtonPressed;


        var MeltButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer/TextureRect/MeltVBoxContainer/TextureButton");
        MeltButton.ButtonDown += OnMeltButtonPressed;

        var ShackleButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/VBoxContainer/MarginContainer2/TextureRect/ShackleVBoxContainer/TextureButton");
        ShackleButton.ButtonDown += OnShackleButtonPressed;

        var UpgradeButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        UpgradeButton.ButtonDown += OnForgeUpgradeButtonPressed;

        // ----------- View Realization -----------
        // ----- Binding Functions
        GameDataManager.Instance.forgeDataManager.OnForgeLevelUpdate += OnForgeLevelUpdate;

        // ----- Set Init Value
        OnForgeLevelUpdate();
    }
    public override void _ExitTree()
    {
        GameDataManager.Instance.forgeDataManager.OnForgeLevelUpdate -= OnForgeLevelUpdate;
    }
    
    private void OnChooseWeaponForMeltButtonPressed()
    {
        GD.Print(" -- Choose Button for Melt --");

        var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
        WeaponSelector sceneInstance = ChooseWeaponScene.Instantiate() as WeaponSelector;
        sceneInstance.Parent = this;
        sceneInstance.weaponList = EWeaponList.Storage;
        
        navigator.PushInstance(sceneInstance);
    }
    private void OnChooseWeaponForShackleButtonPressed()
    {
        GD.Print(" -- Choose Button for Shackle --");

        var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
        WeaponSelector sceneInstance = ChooseWeaponScene.Instantiate() as WeaponSelector;
        sceneInstance.Parent = this;
        sceneInstance.weaponList = EWeaponList.Shackle;
        
        navigator.PushInstance(sceneInstance);
    }
    private void OnMeltButtonPressed()
    {
        GD.Print(" -- Melt --");
    }
    private void OnShackleButtonPressed()
    {
        GD.Print(" -- Shackle --");
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
    }

    public void OnShow()
    {
        GD.Print("Forge Popup shown");
    }
    public void OnHide()
    {
        GD.Print("Forge Popup hidden");
    }
}
