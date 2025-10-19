using Godot;
using System;

public partial class CommandBlock : Control
{
    public override void _Ready()
    {
        CheckInfo();
    }
    
    public void CheckInfo()
    {
        Label allRaids = GetNode<Label>("HBoxContainer/VBoxContainer/TextureRect/VBoxContainer/MarginContainer/MarginContainer/VBoxContainer/HBoxContainer/AllRaidsLabel");
        Label enemyDefeated = GetNode<Label>("HBoxContainer/VBoxContainer/TextureRect/VBoxContainer/MarginContainer/MarginContainer/VBoxContainer/HBoxContainer2/EnemyDefeatedLabel");
        Label squadLevel = GetNode<Label>("HBoxContainer/VBoxContainer/TextureRect/VBoxContainer/MarginContainer/MarginContainer/VBoxContainer/HBoxContainer3/LevelSquadLabel");
        Label squadCount = GetNode<Label>("HBoxContainer/VBoxContainer/TextureRect/VBoxContainer/MarginContainer/MarginContainer/VBoxContainer/HBoxContainer4/SquadCountLabel");
        Label chitins = GetNode<Label>("HBoxContainer/VBoxContainer/TextureRect/VBoxContainer/MarginContainer/MarginContainer/VBoxContainer/HBoxContainer5/ChitinLabel");
        Label crystals = GetNode<Label>("HBoxContainer/VBoxContainer/TextureRect/VBoxContainer/MarginContainer/MarginContainer/VBoxContainer/HBoxContainer6/CrystalLabel");

        allRaids.Text = GameDataManager.Instance.currentData.commandBlockData.RaidCount.ToString();
        enemyDefeated.Text = GameDataManager.Instance.currentData.commandBlockData.EnemyDefeated.ToString();
        squadLevel.Text = GameDataManager.Instance.currentData.commandBlockData.SquadLevel.ToString();
        squadCount.Text = GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.Count.ToString();
        chitins.Text = GameDataManager.Instance.currentData.storageData.ChitinFragments.ToString();
        crystals.Text = GameDataManager.Instance.currentData.storageData.Crystals.ToString();

        InfoBoss VegetableInfo = GetNode<InfoBoss>("HBoxContainer/VBoxContainer/VBoxContainer/InfoBoss");
        InfoBoss TankInfo = GetNode<InfoBoss>("HBoxContainer/VBoxContainer/VBoxContainer/InfoBoss2");
        InfoBoss SpiderBossInfo = GetNode<InfoBoss>("HBoxContainer/VBoxContainer/VBoxContainer/InfoBoss3");
        VegetableInfo.bIsDefeated = GameDataManager.Instance.currentData.commandBlockData.VegetableDefeated;
        VegetableInfo.Check();
        TankInfo.bIsDefeated = GameDataManager.Instance.currentData.commandBlockData.TankDefeated;
        TankInfo.Check();
        SpiderBossInfo.bIsDefeated = GameDataManager.Instance.currentData.commandBlockData.SpiderBossDefeated;
        SpiderBossInfo.Check();
    }
}
