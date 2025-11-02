using Godot;
using System;

[GlobalClass]
public partial class RaceMapDatabase : Resource
{
    [Export] public Godot.Collections.Array<RaceMapEventRow> RaceMapInfos { get; set; } = new();
}
