using Godot;
using System;

public partial class SkillButton : TextureButton
{
    [Export]
    public ESkillType skillType = ESkillType.Health;
    [Signal]
    public delegate void OnSkillButtonPressedEventHandler(SkillButton skillButton);
    [Signal]
    public delegate void OnSkillButtonMouseEnteredEventHandler(SkillButton skillButton);
    [Signal]
    public delegate void OnSkillButtonMouseExitedEventHandler(SkillButton skillButton);

    public override void _Ready()
    {
        ButtonDown += SkillButtonPressed;
        MouseEntered += SkillButtonMouseEntered;
        MouseExited += SkillButtonMouseExited;
    }

    private void SkillButtonPressed()
    {
        EmitSignal(SignalName.OnSkillButtonPressed, this);
    }
    private void SkillButtonMouseEntered()
    {
        EmitSignal(SignalName.OnSkillButtonMouseEntered, this);
    }
    private void SkillButtonMouseExited()
    {
        EmitSignal(SignalName.OnSkillButtonMouseExited, this);
    }
}
