using Godot;
using System;

public partial class Storage : Control, IStackPage
{
    [Export]
	public PackedScene WeaponPanelScene;

    public override void _Ready()
    {
        var UpgradeButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        UpgradeButton.ButtonDown += OnStorageUpgradeButtonPressed;
    }
    public override void _ExitTree()
    {
        GameDataManager.Instance.storageDataManager.OnStorageLevelUpdate -= OnStorageLevelUpdate;
    }

    private void OnStorageUpgradeButtonPressed()
    {
        var UpgradeButton = GetNode<UpgradeButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        Int32 CrystalPrice = UpgradeButton.GetCrystalValue();
        Int32 ChitinPrice = UpgradeButton.GetChitinFragmentsValue();

        if (GameDataManager.Instance.storageDataManager.IsCrystalsEnough(CrystalPrice) && GameDataManager.Instance.storageDataManager.IsChitinFragmentsEnough(ChitinPrice))
        {
            if (GameDataManager.Instance.storageDataManager.TryUpdateLevel())
            {
                GameDataManager.Instance.storageDataManager.AdjustCrystals(-CrystalPrice);
                GameDataManager.Instance.storageDataManager.AdjustChitinFragments(-ChitinPrice);
            
                GD.Print(" -- Successful upgrade storage! -- ");
            }
            else
            {
                GD.Print(" -- Level is Max! -- ");
            }
        }
        else
        {
            GD.Print(" -- Not enough funds to upgrade storage! -- ");
        }
    }
    private void OnStorageLevelUpdate()
    {
        Label LevelLabel = GetNode<Label>("VBoxContainer/MarginContainer/HBoxContainer/LevelLabel");
        LevelLabel.Text = GameDataManager.Instance.currentData.storageData.Level.ToString();

        StorageLevelData currentStorageLevelData = GameDataManager.Instance.storageDatabase.Levels[GameDataManager.Instance.currentData.storageData.Level - 1];

        CapacityView capacityView = GetNode<CapacityView>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer2/CapacityView");
        capacityView.SetCurrentValue(GameDataManager.Instance.currentData.storageData.Weapons.Count);
        capacityView.SetMaxValue(currentStorageLevelData.Capacity);

        UpgradeButton UpgradeButton = GetNode<UpgradeButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        UpgradeButton.SetCrystalValue(currentStorageLevelData.CrystalUpgradeCost);
        UpgradeButton.SetChitinFragmentsValue(currentStorageLevelData.ChitinFragmentsUpgradeCost);

        if (GameDataManager.Instance.storageDatabase.Levels.Count == GameDataManager.Instance.currentData.storageData.Level)
        {
            UpgradeButton.Visible = false;
        }
    }
    private void UpdateWeaponList()
    {
        var weaponVBoxContainer = GetNode<VBoxContainer>("VBoxContainer/HBoxContainer2/ScrollContainer/VBoxContainer/WeaponVBoxContainer");
        foreach (Node child in weaponVBoxContainer.GetChildren())
        {
            child.QueueFree();
        }

        foreach (WeaponData weaponData in GameDataManager.Instance.currentData.storageData.Weapons)
        {
            WeaponPanel weaponPanel = WeaponPanelScene.Instantiate() as WeaponPanel;
            weaponPanel.HideRangeDamage();
            weaponPanel.SetWeaponInfos(weaponData);
            weaponVBoxContainer.AddChild(weaponPanel);
        }
    }
    
     public void OnShow()
    {
        // ----------- View Realization -----------
        // ----- Binding Functions
        GameDataManager.Instance.storageDataManager.OnStorageLevelUpdate += OnStorageLevelUpdate;

        // ----- Set Init Value
        OnStorageLevelUpdate();
        UpdateWeaponList();
    }
    public void OnHide()
    {
        GameDataManager.Instance.storageDataManager.OnStorageLevelUpdate -= OnStorageLevelUpdate;
    }
}
