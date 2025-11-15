using Godot;
using System;
using System.Collections.Generic;

public partial class EntityEffectsPanel : ColorRect
{
    [Export]
    public PackedScene EntityEffectViewScene;
    public void SetEffectsInfos(List<Effect> InAppliedEffects, Effect InWeaponEffect = null)
    {
        HBoxContainer effectsHBoxContainer = GetNode<HBoxContainer>("MarginContainer/ColorRect/MarginContainer/HBoxContainer");
        foreach (Node child in effectsHBoxContainer.GetChildren())
        {
            child.QueueFree();
        }

        foreach (Effect effect in InAppliedEffects)
        {
            EntityEffectView entityEffectView = EntityEffectViewScene.Instantiate() as EntityEffectView;
            entityEffectView.SetEffectTexture(effect.EffectType);
            effectsHBoxContainer.AddChild(entityEffectView);
        }
        if (InWeaponEffect != null && (InWeaponEffect.EffectType == EEffectType.ResistanceToStun || InWeaponEffect.EffectType == EEffectType.Pushing))
        {
            EntityEffectView entityEffectView = EntityEffectViewScene.Instantiate() as EntityEffectView;
            entityEffectView.SetEffectTexture(InWeaponEffect.EffectType);
            effectsHBoxContainer.AddChild(entityEffectView);
        }
    }
    public void ChangeToFitAndReplace(int count, Effect InWeaponEffect = null)
    {
        bool EntityHasSelfEffect = false;
        if (InWeaponEffect != null)
        {
            if (InWeaponEffect.EffectType == EEffectType.ResistanceToStun || InWeaponEffect.EffectType == EEffectType.Pushing)
            {
                EntityHasSelfEffect = true;
            }
        }

        ColorRect colorRect = GetNode<ColorRect>(".");

        float resultWidth = 0;
        if (count == 0)
        {
            resultWidth = 26f;
        }
        else
        {
            resultWidth = count * 22f + 4f;
        }
        if (EntityHasSelfEffect && count >= 1)
        {
            resultWidth += 22f;
        }

        colorRect.SetSize(new Vector2(resultWidth, colorRect.Size.Y));
        // colorRect.SetPosition(new Vector2(colorRect.Position.X, -colorRect.Size.Y));
    }
    public void SetPositionEnemyOffset(Vector2 InPosition)
    {
        Position = InPosition - new Vector2(Size.X, 0);
    }
    public void SetPositionWarriorOffset(Vector2 InPosition)
    {
        Position = InPosition + new Vector2(169, 0);
    }
}
