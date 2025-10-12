using Godot;
using System;

public partial class WeaponSelector : Node
{
    [Signal]
    public delegate void RequestBackEventHandler();

    public Node Parent = null;

    public override void _Ready()
    {
        var BackButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HBoxContainer/TextureButton");
        BackButton.ButtonDown += OnBackButtonPressed;

        GD.Print("Weapon Selector Ready");
    }

    private void OnBackButtonPressed()
    {
        EmitSignal(SignalName.RequestBack);
    }
}
