using Godot;
using System;

[GlobalClass]
public partial class CharactersExperienceLevelsDatabase : Resource
{
	[Export] public Godot.Collections.Array<CharacterExprerienceLevelRow> Levels { get; set; } = new();
}
