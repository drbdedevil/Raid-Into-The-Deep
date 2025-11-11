using Godot;
using RaidIntoTheDeep.Levels.Fight;
using System;

public partial class ViewEnemyWarriorPanel : Control
{
	[Signal]
	public delegate void EnemyPanelMouseEnterEventHandler(EnemyEntity enemyEntity);
	[Signal]
	public delegate void EnemyPanelMouseExitEventHandler(EnemyEntity enemyEntity);

	public override void _Ready()
	{
		MouseEntered += OnMouseEnter;
        MouseExited += OnMouseExit;
	}

	public void SetEnemyInfosToWarriorEnemyPanel(EnemyEntity enemyEntity)
	{
		WarriorEnemy warrior = GetNode<WarriorEnemy>("PanelContainer/MarginContainer/WarriorEnemyPanel");
		warrior.SetEnemyInfos(enemyEntity);
	}

	public EnemyEntity GetEnemyEntity()
	{
		WarriorEnemy warrior = GetNode<WarriorEnemy>("PanelContainer/MarginContainer/WarriorEnemyPanel");
		return warrior.EnemyEntity;
    }

	private void OnMouseEnter()
	{
		WarriorEnemy warrior = GetNode<WarriorEnemy>("PanelContainer/MarginContainer/WarriorEnemyPanel");

		EmitSignal(SignalName.EnemyPanelMouseEnter, warrior.EnemyEntity);
    }

    private void OnMouseExit()
	{
		WarriorEnemy warrior = GetNode<WarriorEnemy>("PanelContainer/MarginContainer/WarriorEnemyPanel");

		EmitSignal(SignalName.EnemyPanelMouseExit, warrior.EnemyEntity);
    }
}
