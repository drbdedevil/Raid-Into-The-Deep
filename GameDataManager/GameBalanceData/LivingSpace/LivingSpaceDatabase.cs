using Godot;
using System;

[GlobalClass]
public partial class LivingSpaceDatabase : Resource
{
    [Export] public Godot.Collections.Array<LivingSpaceLevelData> Levels { get; set; } = new();
}
