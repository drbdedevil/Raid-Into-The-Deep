using Godot;
using System;

[GlobalClass]
public partial class StorageDatabase : Resource
{
    [Export] public Godot.Collections.Array<StorageLevelData> Levels { get; set; } = new();
}