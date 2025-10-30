using Godot;
using System;

public enum EEffectType
{
    Poison = 0,
    Stun = 1,
    Freezing = 2,
    Weakening = 3,
    ResistanceToStun = 4,
    Pushing = 5,
    Sleep = 6,
    Fire = 7
}

[GlobalClass]
public partial class EffectInfo : Resource
{
    [Export] public string Name { get; set; } = "";
    [Export] public Texture2D texture2D { get; set; } = new();
    [Export] public EEffectType effectType { get; set; } = new();
    [Export] public float Weight { get; set; } = 1.0f;
}
