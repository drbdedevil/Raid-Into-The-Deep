using Godot;
using System;

[GlobalClass]
public partial class AttackShapeInfo : Resource
{
    [Export] public string Name { get; set; } = "";
    [Export] public Texture2D texture2D { get; set; } = new();
    [Export] public Godot.Collections.Array<Vector2I> AttackCood = new();
}
