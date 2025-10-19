using Godot;
using System;

[GlobalClass]
public partial class LivingSpaceLevelData : Resource
{
    [Export] public int Level { get; set; } = 1;
    [Export] public int CrystalUpgradeCost { get; set; } = 2;
    [Export] public int ChitinFragmentsUpgradeCost { get; set; } = 3;
    [Export] public int Capacity { get; set; } = 8;
}
