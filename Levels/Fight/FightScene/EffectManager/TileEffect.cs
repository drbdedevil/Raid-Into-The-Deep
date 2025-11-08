using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using System;

public partial class TileEffect : Effect
{
    public Tile tileHolder;
    public TileEffect(EEffectType InEffectType, int InDuration = 0) : base(InEffectType, InDuration)
    {
        
    }
    public override void ApplyForHolder()
    {
        
    }
}
