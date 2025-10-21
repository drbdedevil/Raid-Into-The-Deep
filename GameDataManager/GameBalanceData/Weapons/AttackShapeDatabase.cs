using Godot;
using System;

[GlobalClass]
public partial class AttackShapeDatabase : Resource
{
    [Export] public Godot.Collections.Array<AttackShapeInfo> AttackShapes { get; set; } = new();
}
