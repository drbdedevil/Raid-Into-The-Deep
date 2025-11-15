using Godot;
using System;
using System.Linq;

public partial class EntityEffectView : Control
{
    public void SetEffectTexture(EEffectType InEffectType)
    {
        TextureRect effectTexture = GetNode<TextureRect>("TextureRect2/MarginContainer/EffectTexture");

        EffectInfo effectInfo = GameDataManager.Instance.effectDatabase.Effects.FirstOrDefault(effect => effect.effectType == InEffectType);
        if (effectInfo != null)
        {
            effectTexture.Texture = effectInfo.texture2D;

            return;
        }

        switch (InEffectType)
        {
            case EEffectType.ReserveDamage:
                Texture2D texture1 = GD.Load<Texture2D>("res://Textures/EffectsTextures/ReserveDamage.png");
                effectTexture.Texture = texture1;
                break;
            case EEffectType.BattleFrenzy:
                Texture2D texture2 = GD.Load<Texture2D>("res://Textures/EffectsTextures/BattleFrenzy.png");
                effectTexture.Texture = texture2;
                break;
            case EEffectType.BloodMark:
                Texture2D texture3 = GD.Load<Texture2D>("res://Textures/EffectsTextures/BloodMark.png");
                effectTexture.Texture = texture3;
                break;
            case EEffectType.Defense:
                Texture2D texture4 = GD.Load<Texture2D>("res://Textures/EffectsTextures/Defense.png");
                effectTexture.Texture = texture4;
                break;
            case EEffectType.SevereWound:
                Texture2D texture5 = GD.Load<Texture2D>("res://Textures/EffectsTextures/SevereWound.png");
                effectTexture.Texture = texture5;
                break;
            default:
                break;
        }
    }
}
