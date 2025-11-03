using Godot;
using System;

public enum EMessageType
{
    Default = 0,
    Warning = 1,
    Alert = 2
}

public partial class NotificationMessage : ColorRect
{
    [Export] public float Duration = 2.0f;
    [Export] public float FadeTime = 0.5f;
    [Export] public float FadeInTime = 0.2f;

    [Export] public Color warningColor = new Color();
    [Export] public Color alertColor = new Color();

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

    public void SetMessage(string msg, EMessageType messageType = EMessageType.Default)
    {
        Label msgLabel = GetNode<Label>("Label");
        msgLabel.Text = msg;

        if (messageType == EMessageType.Warning)
        {
            this.Color = warningColor;
        }
        else if (messageType == EMessageType.Alert)
        {
            this.Color = alertColor;
        }

        CallDeferred(nameof(UpdateSize));
    }

    private void UpdateSize()
    {
        Label msgLabel = GetNode<Label>("Label");

        Vector2 labelSize = msgLabel.GetMinimumSize();
        CustomMinimumSize = new Vector2(CustomMinimumSize.X, labelSize.Y + 20);
    }
}
