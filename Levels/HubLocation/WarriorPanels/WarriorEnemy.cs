using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using RaidIntoTheDeep.Levels.Fight;

public partial class WarriorEnemy : Node, IStackPage
{
	private EnemyEntity _enemyEntity = null;
	public EnemyEntity EnemyEntity
	{
		get
		{
			return _enemyEntity;
		}
		set
		{
			_enemyEntity = value;
		}
	}
	public override void _Ready()
	{
		
	}

	public void SetEnemyInfos(EnemyEntity enemyEntity)
	{
		_enemyEntity = enemyEntity;

		TextureRect textureRect = GetNode<TextureRect>("TextureRect/HBoxContainer/MarginContainer/TextureRect");
		textureRect.Texture = enemyEntity.EnemyTexture;

		Label NameLabel = GetNode<Label>("TextureRect/HBoxContainer/VBoxContainer/NameLabel");
		NameLabel.Text = enemyEntity.EnemyName;

		Label LevelLabel = GetNode<Label>("TextureRect/HBoxContainer/VBoxContainer/HBoxContainer/LevelLabel");
		LevelLabel.Text = enemyEntity.EnemyLevel.ToString();

		ProgressBar progressBar = GetNode<ProgressBar>("TextureRect/HBoxContainer/VBoxContainer/MarginContainer3/ProgressBar");
		progressBar.Value = enemyEntity.Health;
		progressBar.MaxValue = enemyEntity.MaxHealth;
	}
	

	public void OnShow()
	{
		var ListButton = GetNode<Button>("TextureRect/HBoxContainer/MarginContainer/TextureRect/MarginContainer/Button");
		ListButton.ReleaseFocus();
		// ListButton.ButtonDown += OnListButtonPressed;
	}
	public void OnHide()
	{
		// var ListButton = GetNode<Button>("TextureRect/HBoxContainer/MarginContainer/TextureRect/MarginContainer/Button");
		// ListButton.ButtonDown -= OnListButtonPressed;
	}
}
