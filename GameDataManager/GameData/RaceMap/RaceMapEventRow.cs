using Godot;
using System;

[GlobalClass]
public partial class RaceMapEventRow : Resource
{
    [Export] public Texture2D unpassedNodeIcon = new Texture2D();
    [Export] public Texture2D passedNodeIcon = new Texture2D();
    [Export] public Texture2D hoverNodeIcon = new Texture2D();
    [Export] public Texture2D pressedNodeIcon = new Texture2D();
    [Export] public Texture2D disabledNodeIcon = new Texture2D();
    [Export] public MapNodeType mapNodeType = MapNodeType.Start;
    [Export] public float Weight = 1f;
}
