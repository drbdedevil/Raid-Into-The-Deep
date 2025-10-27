using Godot;
using System;

[GlobalClass]
public partial class PassiveSkillProgressionRow : Resource
{
    [Export] public EPassiveSkillType skillType { get; set; } = EPassiveSkillType.Health;
    [Export] public Godot.Collections.Array<int> increments { get; set; } = new();
}
