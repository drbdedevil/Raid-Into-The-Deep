using Godot;
using System;

public partial class DifficultySelection : Control
{
    private ColorRect _window;
    private ColorRect _overlay;

    private EGameMode currentGameMode = EGameMode.None;

    public override void _Ready()
    {
        _window = GetNode<ColorRect>("ColorRect2");
        _overlay = GetNode<ColorRect>("ColorRect");

        Button backButton = GetNode<Button>("ColorRect2/ColorRect/ColorRect/VBoxContainer/HBoxContainer2/BackButton");
        backButton.Pressed += OnBackButtonPressed;

        Button createGameButton = GetNode<Button>("ColorRect2/ColorRect/ColorRect/VBoxContainer/HBoxContainer2/CreateButtonGame");
        createGameButton.Pressed += OnCreateGameButtonPressed;

        TextureButton usualButton = GetNode<TextureButton>("ColorRect2/ColorRect/ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/UsualButton");
        usualButton.Pressed += OnUsualButtonPressed;

        TextureButton simpleButton = GetNode<TextureButton>("ColorRect2/ColorRect/ColorRect/VBoxContainer/HBoxContainer/VBoxContainer2/SimpleButton");
        simpleButton.Pressed += OnSimpleButtonPressed;

        FreshGameMode();
    }

    public void ShowPopup()
    {
        currentGameMode = EGameMode.None;
        FreshGameMode();

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

    private void FreshGameMode()
    {
        TextureButton usualButton = GetNode<TextureButton>("ColorRect2/ColorRect/ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/UsualButton");
        TextureButton simpleButton = GetNode<TextureButton>("ColorRect2/ColorRect/ColorRect/VBoxContainer/HBoxContainer/VBoxContainer2/SimpleButton");
        usualButton.Disabled = false;
        simpleButton.Disabled = false;

        switch (currentGameMode)
        {
            case EGameMode.Usual:
                usualButton.Disabled = true;
                break;
            case EGameMode.Simple:
                simpleButton.Disabled = true;
                break;
            default:
                break;
        }
    }

    private void OnBackButtonPressed()
    {
        HidePopup();
    }

    private void OnCreateGameButtonPressed()
    {
        if (currentGameMode == EGameMode.None)
        {
            NotificationSystem.Instance.ShowMessage("Необходимо выбрать режим!", EMessageType.Warning);
            return;
        }

        string saveName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

		GameDataManager.Instance.CreateNewGame(currentGameMode);
		GameDataManager.Instance.SetSavePath(saveName);
		GameDataManager.Instance.Save();

		GetTree().ChangeSceneToFile("res://Levels/HubLocation/HubLocation.tscn");
    }

    private void OnUsualButtonPressed()
    {
        currentGameMode = EGameMode.Usual;
        FreshGameMode();
    }

    private void OnSimpleButtonPressed()
    {
        currentGameMode = EGameMode.Simple;
        FreshGameMode();
    }
}
