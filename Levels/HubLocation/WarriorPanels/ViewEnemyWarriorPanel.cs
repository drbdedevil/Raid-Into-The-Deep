using Godot;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.Weapons;
using System;
using System.Linq;

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

		MarginContainer marginContainer = GetNode<MarginContainer>("PanelContainer/WeaponINfs");
		marginContainer.Visible = true;

		EffectInfo effectInfo = GameDataManager.Instance.effectDatabase.Effects.FirstOrDefault(effect => effect.effectType == warrior.EnemyEntity.Weapon.effect.EffectType);
		if (effectInfo != null)
		{
			TextureRect effectRect = GetNode<TextureRect>("PanelContainer/WeaponINfs/ColorRect/TextureRect2/MarginContainer/EffectTexture");
			effectRect.Texture = effectInfo.texture2D;
		}
		WeaponRow weaponRow = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == warrior.EnemyEntity.Weapon.weaponData.Name);
		if (weaponRow != null)
		{
			TextureRect weaponRect = GetNode<TextureRect>("PanelContainer/WeaponINfs/ColorRect/TextureRect");
			weaponRect.Texture = weaponRow.WeaponTexture;
		}
	}

	private void OnMouseExit()
	{
		WarriorEnemy warrior = GetNode<WarriorEnemy>("PanelContainer/MarginContainer/WarriorEnemyPanel");

		EmitSignal(SignalName.EnemyPanelMouseExit, warrior.EnemyEntity);

		MarginContainer marginContainer = GetNode<MarginContainer>("PanelContainer/WeaponINfs");
		marginContainer.Visible = false;
	}
}
