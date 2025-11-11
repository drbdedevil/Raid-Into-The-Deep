using Godot;
using RaidIntoTheDeep.Levels.Fight;
using System;

public partial class ViewEnemyWarriorPanel : Control
{
	public override void _Ready()
	{
		
	}

	public void SetEnemyInfosToWarriorEnemyPanel(EnemyEntity enemyEntity)
	{
		WarriorEnemy warrior = GetNode<WarriorEnemy>("PanelContainer/MarginContainer/WarriorEnemyPanel");
		warrior.SetEnemyInfos(enemyEntity);
	}
}
