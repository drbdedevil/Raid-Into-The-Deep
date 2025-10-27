using Godot;
using System;

[GlobalClass]
public partial class SkillRow : Resource
{
	[Export] public Texture2D skillTextureBase { get; set; } = new();
	[Export] public Texture2D skillTextureHover { get; set; } = new();
	[Export] public Texture2D skillTextureUpgraded { get; set; } = new();
	[Export] public Texture2D skillTextureActive { get; set; } = new();
	[Export] public string skillName { get; set; } = "";
	[Export] public string skillDescription { get; set; } = "";
	[Export] public EPassiveSkillType skillType { get; set; } = EPassiveSkillType.Health;
}
