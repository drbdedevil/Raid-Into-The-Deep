using Godot;
using System;
using System.Linq;

public partial class BigWeaponPanel : Control
{
    public void SetWeaponInfos(WeaponData InWeaponData)
    {
        Label NameLabel = GetNode<Label>("VBoxContainer/HBoxContainer/MarginContainer/VBoxContainer/NameLabel");
		NameLabel.Text = InWeaponData.Name;

        TextureRect attackShapeRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer3/HBoxContainer/TextureRect");
        Label TextLabel = GetNode<Label>("VBoxContainer/HBoxContainer/MarginContainer/VBoxContainer/Label");
		attackShapeRect.Texture = GameDataManager.Instance.attackShapeDatabase.AttackShapes[InWeaponData.AttackShapeID].texture2D;
		TextureRect weaponRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
		var existingWeapon = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == InWeaponData.Name);
		if (existingWeapon != null)
		{
            weaponRect.Texture = existingWeapon.WeaponTexture;
            TextLabel.Text = existingWeapon.Description;
		}

		Label NumberLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/HBoxContainer/TextureRect/NumberLabel");
		NumberLabel.Text = InWeaponData.Damage.ToString();

		EffectInfo effectInfo = GameDataManager.Instance.effectDatabase.Effects[InWeaponData.EffectID];
		TextureRect effectTexture = GetNode<TextureRect>("VBoxContainer/HBoxContainer2/HBoxContainer2/TextureRect2/MarginContainer/EffectTexture");
		effectTexture.Texture = effectInfo.texture2D;
    }
}
