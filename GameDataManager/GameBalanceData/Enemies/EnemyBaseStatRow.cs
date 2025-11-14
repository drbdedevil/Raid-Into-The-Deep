using Godot;

[GlobalClass]
public partial class EnemyBaseStatRow : Resource
{
	[Export] public int Damage { get; set; } = 1;
	[Export] public int DamageByEffect { get; set; } = 1;
	[Export] public int Health { get; set; } = 20;
	[Export] public int Heal { get; set; } = 5;
	[Export] public int Speed { get; set; } = 3;
	[Export] public AttackShapeType AttackShapeType { get; set; }
}
