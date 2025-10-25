using Godot;
using System;
using System.Linq;

public partial class WeaponPanel : Control
{
	public WeaponData weaponData = new();

	[Signal]
	public delegate void OnWeaponPanelPressedEventHandler(WeaponPanel InWeaponPanel);

	public override void _Ready()
	{
		Button button = GetNodeOrNull<Button>("TextureRect/Button");
		if (button != null)
		{
			button.ButtonDown += OnButtonPressed;
		}
	}

	public void SetWeaponInfos(WeaponData InWeaponData)
	{
		weaponData = InWeaponData;

		Label NameLabel = GetNode<Label>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/NameLabel");
		NameLabel.Text = InWeaponData.Name;

		TextureRect attackShapeRect = GetNode<TextureRect>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer2/MarginContainer/TextureRect");
		attackShapeRect.Texture = GameDataManager.Instance.attackShapeDatabase.AttackShapes[InWeaponData.AttackShapeID].texture2D;

		TextureRect weaponRect = GetNode<TextureRect>("TextureRect/HBoxContainer/MarginContainer/TextureRect");
		var existingWeapon = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == InWeaponData.Name);
		if (existingWeapon != null)
		{
			weaponRect.Texture = existingWeapon.WeaponTexture;
		}

		Label NumberLabel = GetNode<Label>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer/TextureRect/HBoxContainer/NumberLabel");
		NumberLabel.Text = InWeaponData.Damage.ToString();

		EffectInfo effectInfo = GameDataManager.Instance.effectDatabase.Effects[weaponData.EffectID];
		TextureRect effectTexture = GetNode<TextureRect>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer2/TextureRect2/MarginContainer/EffectTexture");
		effectTexture.Texture = effectInfo.texture2D;

		Texture2D damageTexture = GD.Load<Texture2D>("res://Textures/HubLocation/NumberIcon.png");
		TextureRect damageTextureRect = GetNode<TextureRect>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer/TextureRect");
		damageTextureRect.Texture = damageTexture;
		damageTextureRect.SetCustomMinimumSize(new Vector2(23.0f, 23.0f));
	}
	public void SetWeaponInfosShackle(WeaponData InWeaponData)
	{
		weaponData = InWeaponData;

		Label NameLabel = GetNode<Label>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/NameLabel");
		NameLabel.Text = InWeaponData.Name;

		TextureRect attackShapeRect = GetNode<TextureRect>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer2/MarginContainer/TextureRect");
		attackShapeRect.Texture = GameDataManager.Instance.attackShapeDatabase.AttackShapes[InWeaponData.AttackShapeID].texture2D;

		TextureRect weaponRect = GetNode<TextureRect>("TextureRect/HBoxContainer/MarginContainer/TextureRect");
		var existingWeapon = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == InWeaponData.Name);
		if (existingWeapon != null)
		{
			weaponRect.Texture = existingWeapon.WeaponTexture;

			Label NumberLabel = GetNode<Label>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer/TextureRect/HBoxContainer/NumberLabel");
			NumberLabel.Text = existingWeapon.DamageRange.X.ToString();

			Label NumberLabel2 = GetNode<Label>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer/TextureRect/HBoxContainer/NumberLabel2");
			NumberLabel2.Text = existingWeapon.DamageRange.Y.ToString();
		}

		// Label effectLabel = GetNode<Label>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer2/TextureRect2/EffectLabel");
		// effectLabel.Text = "?";
	}

	public void HideRangeDamage()
	{
		Label label = GetNode<Label>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer/TextureRect/HBoxContainer/Label");
		label.Visible = false;
		Label labelN = GetNode<Label>("TextureRect/HBoxContainer/MarginContainer2/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer/TextureRect/HBoxContainer/NumberLabel2");
		labelN.Visible = false;
	}
	
	public void OnButtonPressed()
	{
		EmitSignal(SignalName.OnWeaponPanelPressed, this);
	}
}
