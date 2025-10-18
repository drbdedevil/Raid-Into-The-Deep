using Godot;
using System;

public partial class UpgradeButton : TextureButton
{
	public override void _Ready()
	{
		// var UpgradeButton = GetNode<TextureButton>(".");
		// UpgradeButton.ButtonDown += OnUpgradeButtonPressed;
	}

	public Int32 GetCrystalValue()
	{
		Label CrystalLabel = GetNode<Label>("HBoxContainer/CrystalLabel");
		return CrystalLabel.Text.ToInt();
	}
	public Int32 GetChitinFragmentsValue()
	{
		Label ChitinFragmentsLabel = GetNode<Label>("HBoxContainer/ChitinLabel");
		return ChitinFragmentsLabel.Text.ToInt();
	}
	public void SetCrystalValue(Int32 Value)
	{
		Label CrystalLabel = GetNode<Label>("HBoxContainer/CrystalLabel");
		CrystalLabel.Text = Value.ToString();
	}
	public void SetChitinFragmentsValue(Int32 Value)
	{
		Label ChitinFragmentsLabel = GetNode<Label>("HBoxContainer/ChitinLabel");
		ChitinFragmentsLabel.Text = Value.ToString();
	}
	
	private void OnUpgradeButtonPressed()
	{
		// GD.Print(" -- Upgrade --");
	}
}
