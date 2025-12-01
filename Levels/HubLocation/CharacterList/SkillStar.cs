using Godot;
using System;

public partial class SkillStar : Control
{
    // Цвет, чтобы Tween мог его менять
    [Export]
    private Color _color = Colors.Yellow;
    [Export]
    private Color _color2 = Colors.Yellow;
    private float _scale = 1f;

    private bool bShouldAnimate = false;
    private Tween _currentTween;

    public override void _Ready()
    {
        // Чтобы изначально было видно
        Modulate = Colors.White;

        // Запускаем мигание
        // AnimateBlink();
        AnimatePulse();
    }

    public override void _Draw()
    {
        Vector2 center = Size / 2;
        float r = Mathf.Min(Size.X, Size.Y) * 0.34f * _scale;
        DrawCircle(center, r, _color);

        float r2 = Mathf.Min(Size.X, Size.Y) * 0.35f * _scale;
        DrawArc(center, r2, 0, Mathf.Tau, 32, _color2, 1);

        float s = Mathf.Min(Size.X, Size.Y) * 0.2f * _scale;
        DrawLine(center + new Vector2(0, -s), center + new Vector2(0, s), _color2, 2);
        DrawLine(center + new Vector2(-s, 0), center + new Vector2(s, 0), _color2, 2);

        // GD.Print(_scale);
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    private async void AnimateBlink()
    {
        while (bShouldAnimate)
        {
            _currentTween = CreateTween();

            // Ярче
            _currentTween.TweenProperty(this, "modulate", new Color(1, 1, 1, 1), 1f);
            await ToSignal(_currentTween, "finished");

            _currentTween = CreateTween();
            // Темнее
            _currentTween.TweenProperty(this, "modulate", new Color(1, 1, 1, 0.4f), 1f);
            await ToSignal(_currentTween, "finished");
        }
    }
    private async void AnimatePulse()
    {
        while (bShouldAnimate)
        {
            // 1. Медленное уменьшение (например 1.0 → 0)
            _currentTween = CreateTween();
            _currentTween.TweenProperty(this, "_scale", 0.75f, 1.2f)  // медленно
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.Out);

            await ToSignal(_currentTween, "finished");

            // 2. Быстрое увеличение обратно (например 0 → 1.0)
            _currentTween = CreateTween();
            _currentTween.TweenProperty(this, "_scale", 1f, 0.25f) // быстро
                .SetTrans(Tween.TransitionType.Back)
                .SetEase(Tween.EaseType.Out);

            await ToSignal(_currentTween, "finished");
        }
    }

    public void Start()
    {
        Stop();

        Visible = true;
        bShouldAnimate = true;

        AnimatePulse();
    }
    public void Stop()
    {
        Visible = false;
        bShouldAnimate = false;

        _currentTween?.Kill();
        _currentTween = null;
    }
}
