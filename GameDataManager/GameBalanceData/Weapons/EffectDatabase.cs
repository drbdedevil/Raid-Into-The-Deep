using Godot;
using System;

[GlobalClass]
public partial class EffectDatabase : Resource
{
    [Export] public Godot.Collections.Array<EffectInfo> Effects { get; set; } = new();
}
