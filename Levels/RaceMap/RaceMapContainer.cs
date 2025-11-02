using Godot;
using System;

public partial class RaceMapContainer : ScrollContainer
{
    private bool bIsPanning = false;
    private Vector2 LastMousePos = new Vector2(0f, 0f);

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                bIsPanning = mouseEvent.Pressed;
                LastMousePos = mouseEvent.Position;
                // GD.Print("RIGHT MOUSE");
            }
        }
        else if (@event is InputEventMouseMotion mouseMotion && bIsPanning)
        {
            Vector2 delta = mouseMotion.Position - LastMousePos;
            LastMousePos = mouseMotion.Position;
            ScrollHorizontal -= (int)delta.X;
            ScrollVertical -= (int)delta.Y;
            // GD.Print("SKROLL");
        }
	}
}
