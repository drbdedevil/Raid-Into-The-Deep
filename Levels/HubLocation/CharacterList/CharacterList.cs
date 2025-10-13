using Godot;
using System;

public partial class CharacterList : ColorRect, IStackPage
{
    [Export]
    public PackedScene ChooseWeaponScene;
    [Export]
	public PackedScene SkillTreeScene;
    [Signal]
    public delegate void RequestBackEventHandler();

    public Node Parent = null;
    
    public override void _Ready()
    {
        var WeaponButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect3/VBoxContainer/MarginContainer/HBoxContainer/TextureButton");
        WeaponButton.ButtonDown += OnChooseWeaponButtonPressed;

        var SkillButton = GetNode<TextureButton>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect4/VBoxContainer/MarginContainer/HBoxContainer/TextureButton");
        SkillButton.ButtonDown += OnSkillTreeButtonPressed;

        var BackButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HiddenPanel/TextureButton");
        BackButton.ButtonDown += OnBackButtonPressed;
    }

    private void OnChooseWeaponButtonPressed()
    {
        var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
        WeaponSelector sceneInstance = ChooseWeaponScene.Instantiate() as WeaponSelector;
        sceneInstance.Parent = this;

        navigator.PushInstance(sceneInstance);
    }
    private void OnSkillTreeButtonPressed()
    {
        var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
        Node sceneInstance = SkillTreeScene.Instantiate();
        
        navigator.PushInstance(sceneInstance);
    }
    private void OnBackButtonPressed()
    {
        EmitSignal(SignalName.RequestBack);
    }

    public void OnShow()
	{
        GD.Print("CharacterList Popup shown");
    }
    public void OnHide()
	{
        GD.Print("CharacterList Popup hidden");
    }
}
