using Godot;
using System;

public partial class UpgradeButton : TextureButton
{
    public override void _Ready()
    {
        var UpgradeButton = GetNode<TextureButton>(".");
        UpgradeButton.ButtonDown += OnUpgradeButtonPressed;
    }
    
    private void OnUpgradeButtonPressed()
	{
		GD.Print(" -- Upgrade --");
	}
}
