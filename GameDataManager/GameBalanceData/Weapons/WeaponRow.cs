using Godot;
using System;

[GlobalClass]
public partial class WeaponRow : Resource
{
    [Export] public string Name { get; set; } = "";
    [Export] public string Description { get; set; } = "";
    [Export] public Vector2I DamageRange { get; set; } = new Vector2I(1, 2);
    [Export] public int AttackShapeID { get; set; } = 0;
    [Export] public Texture2D WeaponTexture { get; set; } = new();
    [Export] public int CrystalCost { get; set; } = new();
    [Export] public int ChitinFragmentsCost { get; set; } = new();
    [Export] public int CrystalYield { get; set; } = new();
    [Export] public int ChitinFragmentsYield { get; set; } = new();
    [Export] public string SoundPath { get; set; } = "";
    [Export] public int Damage { get; set; }
    [Export] public EWeaponType weaponType { get; set; } = EWeaponType.Sword;
}
