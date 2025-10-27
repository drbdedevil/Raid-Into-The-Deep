using Godot;
using System;

[GlobalClass]
public partial class SkillsDatabase : Resource
{
    [Export] public Godot.Collections.Array<SkillRow> skillsRows { get; set; } = new();
}
