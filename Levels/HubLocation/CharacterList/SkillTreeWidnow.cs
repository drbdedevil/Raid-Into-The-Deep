using Godot;
using System;

public partial class SkillTreeWidnow : ColorRect, IStackPage
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

    public void OnShow()
    {
        GD.Print("SkillTreeWidnow Popup shown");
    }
    public void OnHide()
    {
        GD.Print("SkillTreeWidnow Popup hidden");
    }
}
