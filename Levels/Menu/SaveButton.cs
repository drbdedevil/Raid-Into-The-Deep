using Godot;
using System;

public partial class SaveButton : HBoxContainer
{
    public string saveName = "";

    public override void _Ready()
	{
        Button saveButton = GetNode<Button>("Button");
        saveButton.Pressed += OnSaveButtonPressed;

        Button deleteButton = GetNode<Button>("MarginContainer/DeleteButton");
        deleteButton.Pressed += OnDeleteButtonPressed;
	}

    public void SetInfo(string InSaveName)
    {
        saveName = InSaveName;

        Label label = GetNode<Label>("Button/Label");
        label.Text = saveName;
    }

    private void OnSaveButtonPressed()
    {
        GameDataManager.Instance.SetSavePath(saveName);
        GameDataManager.Instance.Load();

        GetTree().ChangeSceneToFile("res://Levels/HubLocation/HubLocation.tscn");
    }
    private void OnDeleteButtonPressed()
    {
        GameDataManager.Instance.DeleteSave(saveName);

        QueueFree();
    }
}
