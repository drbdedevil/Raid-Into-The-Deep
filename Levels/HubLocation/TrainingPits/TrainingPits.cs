using Godot;
using System;

public partial class TrainingPits : Control, IStackPage
{
    public override void _Ready()
    {
        var UpgradeButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        UpgradeButton.ButtonDown += OnTrainingPitsUpgradeButtonPressed;
    }
    public override void _ExitTree()
    {
        GameDataManager.Instance.trainingPitsDataManager.OnTrainingPitsLevelUpdate -= OnTrainingPitsLevelUpdate;
    }

    private void OnTrainingPitsUpgradeButtonPressed()
    {
        var UpgradeButton = GetNode<UpgradeButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        Int32 CrystalPrice = UpgradeButton.GetCrystalValue();
        Int32 ChitinPrice = UpgradeButton.GetChitinFragmentsValue();

        if (GameDataManager.Instance.storageDataManager.IsCrystalsEnough(CrystalPrice) && GameDataManager.Instance.storageDataManager.IsChitinFragmentsEnough(ChitinPrice))
        {
            if (GameDataManager.Instance.trainingPitsDataManager.TryUpdateLevel())
            {
                GameDataManager.Instance.storageDataManager.AdjustCrystals(-CrystalPrice);
                GameDataManager.Instance.storageDataManager.AdjustChitinFragments(-ChitinPrice);

                GD.Print(" -- Successful upgrade Training Pits! -- ");
            }
            else
            {
                GD.Print(" -- Level is Max! -- ");
            }
        }
        else
        {
            GD.Print(" -- Not enough funds to upgrade Training Pits! -- ");
        }
    }
    private void OnTrainingPitsLevelUpdate()
    {
        Label LevelLabel = GetNode<Label>("VBoxContainer/MarginContainer/HBoxContainer/LevelLabel");
        LevelLabel.Text = GameDataManager.Instance.currentData.trainingPitsData.Level.ToString();

        TrainingPitsLevelData currentTrainingPitsLevelData = GameDataManager.Instance.trainingPitsDatabase.Levels[GameDataManager.Instance.currentData.trainingPitsData.Level - 1];

        CapacityView capacityView = GetNode<CapacityView>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer2/CapacityView");
        capacityView.SetCurrentValue(GameDataManager.Instance.currentData.trainingPitsData.CharactersForHiring.Count);
        capacityView.SetMaxValue(currentTrainingPitsLevelData.Capacity);

        UpgradeButton UpgradeButton = GetNode<UpgradeButton>("VBoxContainer/MarginContainer/HBoxContainer/MarginContainer/UpgradeButton");
        UpgradeButton.SetCrystalValue(currentTrainingPitsLevelData.CrystalUpgradeCost);
        UpgradeButton.SetChitinFragmentsValue(currentTrainingPitsLevelData.ChitinFragmentsUpgradeCost);

        if (GameDataManager.Instance.trainingPitsDatabase.Levels.Count == GameDataManager.Instance.currentData.trainingPitsData.Level)
        {
            UpgradeButton.Visible = false;
        }
    }

    private void UpdateCharactersForHiringList()
    {
        var charactersForHiringVBoxContainer = GetNode<VBoxContainer>("VBoxContainer/HBoxContainer2/ScrollContainer/VBoxContainer/VBoxContainer");
        foreach (Node child in charactersForHiringVBoxContainer.GetChildren())
        {
            child.QueueFree();
        }


    }

    public void OnShow()
    {
        // ----------- View Realization -----------
        // ----- Binding Functions
        GameDataManager.Instance.trainingPitsDataManager.OnTrainingPitsLevelUpdate += OnTrainingPitsLevelUpdate;

        // ----- Set Init Value
        OnTrainingPitsLevelUpdate();
        UpdateCharactersForHiringList();
    }
    public void OnHide()
	{
        GameDataManager.Instance.trainingPitsDataManager.OnTrainingPitsLevelUpdate -= OnTrainingPitsLevelUpdate;
    }
}
