using Godot;
using System;

public partial class MapNodeButton : TextureButton
{
    public MapNode MapNode { get; set; }
    public Vector2 RandomOffset { get; set; }

    public override void _Ready()
    {
        Pressed += OnMapNodeButtonPressed;
    }
    
    private void OnMapNodeButtonPressed()
    {
        GD.Print("Pressed on: " + MapNode.Type);
    }
}
