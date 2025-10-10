using Godot;
using System;

public partial class Forge : Node
{
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
    }

    private void OnChooseWeaponForMeltButtonPressed()
    {
        GD.Print(" -- Choose Button for Melt --");
    }
    private void OnChooseWeaponForShackleButtonPressed()
    {
        GD.Print(" -- Choose Button for Shackle --");
    }
    private void OnMeltButtonPressed()
    {
        GD.Print(" -- Melt --");
    }
    private void OnShackleButtonPressed()
    {
        GD.Print(" -- Shackle --");
    }
}
