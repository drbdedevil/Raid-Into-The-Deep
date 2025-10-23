using Godot;
using System;

[GlobalClass]
public partial class WeaponRow : Resource
{
    [Export] public string Name { get; set; } = "";
    [Export] public Vector2I DamageRange { get; set; } = new Vector2I(1, 2);
    [Export] public int AttackShapeID { get; set; } = 0;
    [Export] public Texture2D WeaponTexture { get; set; } = new();
}
