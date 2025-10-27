using Godot;
using System;

[GlobalClass]
public partial class PassiveSkillProgressionRow : Resource
{
    [Export] public ESkillType skillType { get; set; } = ESkillType.Health;
    [Export] public Godot.Collections.Array<int> increments { get; set; } = new();
}
