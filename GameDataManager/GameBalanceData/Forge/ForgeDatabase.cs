using Godot;
using System;

[GlobalClass]
public partial class ForgeDatabase : Resource
{
    [Export] public Godot.Collections.Array<ForgeLevelData> Levels { get; set; } = new();
    [Export] public int MaxWeaponsForShackle = 1;
}
