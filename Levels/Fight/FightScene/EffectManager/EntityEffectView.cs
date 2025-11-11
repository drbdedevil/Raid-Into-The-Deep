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
        }
    }
}
