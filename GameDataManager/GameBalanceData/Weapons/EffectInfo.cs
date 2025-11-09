using Godot;
using System;

[GlobalClass]
public partial class EffectInfo : Resource
{
    [Export] public string Name { get; set; } = "";
    [Export] public Texture2D texture2D { get; set; } = new();
    [Export] public EEffectType effectType { get; set; } = new();
    [Export] public float Weight { get; set; } = 1.0f;
    [Export] public int duration { get; set; } = 0;
}
