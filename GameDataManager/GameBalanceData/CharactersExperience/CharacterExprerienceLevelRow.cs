using Godot;
using System;

[GlobalClass]
public partial class CharacterExprerienceLevelRow : Resource
{
    [Export] public int Level { get; set; } = 1;
	[Export] public int NeedableExperinceForNextLevel { get; set; } = 1;
}
