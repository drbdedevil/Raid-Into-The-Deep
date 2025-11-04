using Godot;
using System;
using System.Collections.Generic;

public partial class SavesScene : Control
{
    [Export]
    public PackedScene saveButtonScene;
    public override void _Ready()
    {
        Button closeButton = GetNode<Button>("ColorRect/CloseButton");
        closeButton.Pressed += OnCloseButtonPressed;

        VBoxContainer vBoxContainer = GetNode<VBoxContainer>("ColorRect/ScrollContainer/VBoxContainer");
        foreach (Node child in vBoxContainer.GetChildren())
        {
            child.QueueFree();
        }

        List<string> saves = GameDataManager.Instance.GetSaveList();
        foreach (string save in saves)
        {
            SaveButton saveButton = saveButtonScene.Instantiate() as SaveButton;
            saveButton.SetInfo(save);
            vBoxContainer.AddChild(saveButton);
            vBoxContainer.MoveChild(saveButton, 0);
        }
    }
    
    private void OnCloseButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Levels/Menu/MainMenu.tscn");
    }
}
