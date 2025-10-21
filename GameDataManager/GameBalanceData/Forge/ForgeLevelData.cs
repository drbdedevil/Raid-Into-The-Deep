using Godot;
using System;

[GlobalClass]
public partial class ForgeLevelData : Resource
{
    [Export] public int Level { get; set; } = 1;
    [Export] public int CrystalUpgradeCost { get; set; } = 2;
    [Export] public int ChitinFragmentsUpgradeCost { get; set; } = 3;
    [Export] public float CrystalsDiscountK { get; set; } = 1.0f;
    [Export] public float ChitinsDiscountK { get; set; } = 1.0f;
    [Export] public float CrystalsIncreaseK { get; set; } = 1.0f;
    [Export] public float ChitinsIncreaseK { get; set; } = 1.0f;
}
