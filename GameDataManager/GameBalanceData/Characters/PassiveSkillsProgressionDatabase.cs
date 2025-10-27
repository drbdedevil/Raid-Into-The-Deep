using Godot;
using System;

[GlobalClass]
public partial class PassiveSkillsProgressionDatabase : Resource
{
    [Export] public Godot.Collections.Array<PassiveSkillProgressionRow> Progressions { get; set; } = new();
}
