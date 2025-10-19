using Godot;
using System;

[GlobalClass]
public partial class TrainingPitsDatabase : Resource
{
    [Export] public Godot.Collections.Array<TrainingPitsLevelData> Levels { get; set; } = new();
}
