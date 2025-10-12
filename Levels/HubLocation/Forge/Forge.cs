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
    }

    private void OnChooseWeaponForMeltButtonPressed()
    {
        GD.Print(" -- Choose Button for Melt --");

        var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
        WeaponSelector sceneInstance = ChooseWeaponScene.Instantiate() as WeaponSelector;
        sceneInstance.Parent = this;
        
        navigator.PushInstance(sceneInstance);
    }
    private void OnChooseWeaponForShackleButtonPressed()
    {
        GD.Print(" -- Choose Button for Shackle --");

        var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
        WeaponSelector sceneInstance = ChooseWeaponScene.Instantiate() as WeaponSelector;
        sceneInstance.Parent = this;
        
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

    public void OnShow()
    {
        GD.Print("Forge Popup shown");
    }
    public void OnHide()
    {
        GD.Print("Forge Popup hidden");
    }
}
