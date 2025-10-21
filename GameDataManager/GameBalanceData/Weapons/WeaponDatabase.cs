using Godot;
using System;

[GlobalClass]
public partial class WeaponDatabase : Resource
{
    [Export] public Godot.Collections.Array<WeaponRow> Weapons { get; set; } = new();
}
