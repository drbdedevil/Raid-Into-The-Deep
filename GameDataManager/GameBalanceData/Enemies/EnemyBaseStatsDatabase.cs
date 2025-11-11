using Godot;

[GlobalClass]
public partial class EnemyBaseStatsDatabase : Resource
{ 
    [Export] public Godot.Collections.Dictionary<GameEnemyCode, EnemyBaseStatRow> EnemyBaseStatRows { get; set; }
}