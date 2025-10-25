using Godot;
using System;

public partial class LivingSpace : Control, IStackPage
{
    public override void _Ready()
    {
        var UpgradeButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        UpgradeButton.ButtonDown += OnLivingSpaceUpgradeButtonPressed;
    }
    public override void _ExitTree()
    {
        GameDataManager.Instance.livingSpaceDataManager.OnLivingSpaceLevelUpdate -= OnLivingSpaceLevelUpdate;
    }

    private void OnLivingSpaceUpgradeButtonPressed()
    {
        var UpgradeButton = GetNode<UpgradeButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        Int32 CrystalPrice = UpgradeButton.GetCrystalValue();
        Int32 ChitinPrice = UpgradeButton.GetChitinFragmentsValue();

        if (GameDataManager.Instance.storageDataManager.IsCrystalsEnough(CrystalPrice) && GameDataManager.Instance.storageDataManager.IsChitinFragmentsEnough(ChitinPrice))
        {
            if (GameDataManager.Instance.livingSpaceDataManager.TryUpdateLevel())
            {
                GameDataManager.Instance.storageDataManager.AdjustCrystals(-CrystalPrice);
                GameDataManager.Instance.storageDataManager.AdjustChitinFragments(-ChitinPrice);

                GD.Print(" -- Successful upgrade Living Space! -- ");
            }
            else
            {
                GD.Print(" -- Level is Max! -- ");
            }
        }
        else
        {
            GD.Print(" -- Not enough funds to upgrade Living Space! -- ");
        }
    }
    private void OnLivingSpaceLevelUpdate()
    {
        Label LevelLabel = GetNode<Label>("VBoxContainer/MarginContainer/HBoxContainer/LevelLabel");
        LevelLabel.Text = GameDataManager.Instance.currentData.livingSpaceData.Level.ToString();

        LivingSpaceLevelData currentLivingSpaceLevelData = GameDataManager.Instance.livingSpaceDatabase.Levels[GameDataManager.Instance.currentData.livingSpaceData.Level - 1];

        CapacityView capacityView = GetNode<CapacityView>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer2/CapacityView");
        capacityView.SetCurrentValue(GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.Count + GameDataManager.Instance.currentData.livingSpaceData.ReservedCharacters.Count);
        capacityView.SetMaxValue(currentLivingSpaceLevelData.Capacity);

        UpgradeButton UpgradeButton = GetNode<UpgradeButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        UpgradeButton.SetCrystalValue(currentLivingSpaceLevelData.CrystalUpgradeCost);
        UpgradeButton.SetChitinFragmentsValue(currentLivingSpaceLevelData.ChitinFragmentsUpgradeCost);

        if (GameDataManager.Instance.livingSpaceDatabase.Levels.Count == GameDataManager.Instance.currentData.livingSpaceData.Level)
        {
            UpgradeButton.Visible = false;
        }
    }

    private void UpdateUsedCharactersList()
    {
        var usedCharactersVBoxContainer = GetNode<VBoxContainer>("VBoxContainer/HBoxContainer2/ScrollContainer/VBoxContainer/TeamVBoxContainer");
        foreach (Node child in usedCharactersVBoxContainer.GetChildren())
        {
            child.QueueFree();
        }


    }
    private void UpdateReservedCharactersList()
    {
        var reservedCharactersVBoxContainer = GetNode<VBoxContainer>("VBoxContainer/HBoxContainer2/ScrollContainer/VBoxContainer/ReserveVBoxContainer");
        foreach (Node child in reservedCharactersVBoxContainer.GetChildren())
        {
            child.QueueFree();
        }


    }
    
     public void OnShow()
    {
        // ----------- View Realization -----------
        // ----- Binding Functions
        GameDataManager.Instance.livingSpaceDataManager.OnLivingSpaceLevelUpdate += OnLivingSpaceLevelUpdate;

        // ----- Set Init Value
        OnLivingSpaceLevelUpdate();
        UpdateUsedCharactersList();
        UpdateReservedCharactersList();
    }
    public void OnHide()
    {
        GameDataManager.Instance.livingSpaceDataManager.OnLivingSpaceLevelUpdate -= OnLivingSpaceLevelUpdate;
    }
}
