using Godot;
using System;

public partial class CharacterList : ColorRect
{
    [Signal]
    public delegate void RequestBackEventHandler();

    public Node Parent = null;
    
    public override void _Ready()
    {
        var BackButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HiddenPanel/TextureButton");
        BackButton.ButtonDown += OnBackButtonPressed;
    }

    private void OnBackButtonPressed()
    {
        EmitSignal(SignalName.RequestBack);
    }
}
