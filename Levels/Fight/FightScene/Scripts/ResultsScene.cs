using Godot;
using System;

public partial class ResultsScene : Control
{
    private ColorRect _window;
    private ColorRect _overlay;
    private bool bIsVictory = false;

    [Export]
    public PackedScene runMapScene;

    [Export]
    public PackedScene hubLocationScene;

    public override void _Ready()
    {
        _window = GetNode<ColorRect>("ColorRect2");
        _overlay = GetNode<ColorRect>("ColorRect");

        Button continueButton = GetNode<Button>("ColorRect2/ColorRect/ColorRect/VBoxContainer/Button");
        continueButton.Pressed += OnContinueButtonPressed;
    }

    public void SetVictoryInfo()
    {
        bIsVictory = true;

        Label InfoLabel = GetNode<Label>("ColorRect2/ColorRect/ColorRect/VBoxContainer/Label");
        InfoLabel.Text = "ПОБЕДА";
        InfoLabel.AddThemeColorOverride("font_color", new Color(0.118f, 0.427f, 0.027f));
    }
    public void SetDefeatInfo()
    {
        bIsVictory = false;

        Label InfoLabel = GetNode<Label>("ColorRect2/ColorRect/ColorRect/VBoxContainer/Label");
        InfoLabel.Text = "ПОРАЖЕНИЕ";
        InfoLabel.AddThemeColorOverride("font_color", new Color(0.751f, 0f, 0.09f));
    }
    public void ShowPopup()
    {
        Visible = true;
        _overlay.Color = new Color(0,0,0,0);
        _window.Scale = Vector2.Zero;

        var tween = CreateTween();

        // Анимация фона
        tween.TweenProperty(_overlay, "color", new Color(0f, 0f, 0f, 0.824f), 0.25f)
             .SetTrans(Tween.TransitionType.Sine)
             .SetEase(Tween.EaseType.Out);

        // Анимация увеличения окна
        tween.TweenProperty(_window, "scale", Vector2.One, 0.25f)
             .SetTrans(Tween.TransitionType.Back)
             .SetEase(Tween.EaseType.Out);

        tween.Play();
    }

    public void HidePopup()
    {
        var tween = CreateTween();

        tween.TweenProperty(_window, "scale", Vector2.Zero, 0.2f)
             .SetTrans(Tween.TransitionType.Sine)
             .SetEase(Tween.EaseType.In);

        tween.TweenProperty(_overlay, "color", new Color(0,0,0,0), 0.2f)
             .SetTrans(Tween.TransitionType.Sine)
             .SetEase(Tween.EaseType.In);

        tween.Play();

        tween.Finished += () => Visible = false;
    }

    private void OnContinueButtonPressed()
    {
        if (bIsVictory)
        {
            if (GameDataManager.Instance.runMapDataManager.pressedMapNode.Type == MapNodeType.SpiderBoss)
            {
                GameDataManager.Instance.currentData.commandBlockData.SpiderBossDefeated = true;
                GameDataManager.Instance.currentData.runMapData.bShouldShowRegenerateButton = true;
            }
            else if (GameDataManager.Instance.runMapDataManager.pressedMapNode.Type == MapNodeType.TankBoss)
            {
                GameDataManager.Instance.currentData.commandBlockData.TankDefeated = true;
                GameDataManager.Instance.currentData.runMapData.bShouldShowRegenerateButton = true;
            }
            else if (GameDataManager.Instance.runMapDataManager.pressedMapNode.Type == MapNodeType.VegetableBoss)
            {
                GameDataManager.Instance.currentData.commandBlockData.VegetableDefeated = true;
                GameDataManager.Instance.currentData.runMapData.bShouldShowRegenerateButton = true;
            }

            GameDataManager.Instance.runMapDataManager.PassMapNode();
            SceneTree sceneTree = Engine.GetMainLoop() as SceneTree;
            sceneTree.ChangeSceneToPacked(runMapScene);
        }
        else
        {
            SceneTree sceneTree = Engine.GetMainLoop() as SceneTree;
            sceneTree.ChangeSceneToPacked(hubLocationScene);
        }
    }
}
