using Godot;
using System;

[GlobalClass]
public partial class RaceMapValuesDatabase : Resource
{
    [Export] public int MinCrystalsAward = 10;
    [Export] public int MaxCrystalsAward = 50;
    [Export] public int MinChitinFragmentsAward = 10;
    [Export] public int MaxChitinFragmentsAward = 50;

    [Export] public int HealPercent = 25;
}
