using Godot;
using System;

public partial class NotificationMessage : ColorRect
{
    [Export] public float Duration = 2.0f;
    [Export] public float FadeTime = 0.5f;
    [Export] public float FadeInTime = 0.2f;

    private float timer = 0f;

    public override void _Ready()
    {
        Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, 0f);
    }

    public override void _Process(double delta)
    {
        timer += (float)delta;

        if (timer < FadeInTime)
        {
            float alpha = timer / FadeInTime;
            Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, alpha);
        }
        else if (timer > Duration)
        {
            float fade = 1 - (timer - Duration) / FadeTime;
            Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, fade);
            if (fade <= 0)
                QueueFree();
        }
        else
        {
            Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, 1f);
        }
    }

    public void SetMessage(string msg)
    {
        Label msgLabel = GetNode<Label>("Label");
        msgLabel.Text = msg;

        CallDeferred(nameof(UpdateSize));
    }

    private void UpdateSize()
    {
        Label msgLabel = GetNode<Label>("Label");

        Vector2 labelSize = msgLabel.GetMinimumSize();
        CustomMinimumSize = new Vector2(CustomMinimumSize.X, labelSize.Y + 20);
    }
}
