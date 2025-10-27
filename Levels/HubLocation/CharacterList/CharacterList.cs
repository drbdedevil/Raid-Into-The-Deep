using Godot;
using System;

public partial class CharacterList : ColorRect, IStackPage
{
    [Export]
    public PackedScene ChooseWeaponScene;
    [Export]
    public PackedScene SkillTreeScene;
    [Export]
    public PackedScene noWeaponPanelScene;
    [Export]
    public PackedScene weaponPanelScene;
    [Signal]
    public delegate void RequestBackEventHandler();

    public Node Parent = null;
    public Warrior warriorOwner { get; set; } = null;
    
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
        sceneInstance.weaponList = EWeaponList.Storage;

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

    public void ShowCharacterInfos()
    {
        if (warriorOwner != null)
        {
            Label LevelLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer/TextureRect/NumberLabel");
            Label HealthLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer2/TextureRect/NumberLabel");
            Label SpeedLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer3/TextureRect/NumberLabel");
            Label DamageLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer4/TextureRect/NumberLabel");
            Label DamageByEffectLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer5/TextureRect/NumberLabel");
            Label HealLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect2/VBoxContainer/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer6/TextureRect/NumberLabel");

            LevelLabel.Text = warriorOwner.characterData.Level.ToString();
            HealthLabel.Text = warriorOwner.characterData.Health.ToString();
            SpeedLabel.Text = warriorOwner.characterData.Speed.ToString();
            DamageLabel.Text = warriorOwner.characterData.Damage.ToString();
            DamageByEffectLabel.Text = warriorOwner.characterData.DamageByEffect.ToString();
            HealLabel.Text = warriorOwner.characterData.Heal.ToString();

            TextureRect characterTexture = GetNode<TextureRect>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect/MarginContainer/ColorRect/VBoxContainer/MarginContainer/TextureRect");
            characterTexture.Texture = GameDataManager.Instance.charactersSpritesDatabase.CharactersSpritesArray[warriorOwner.characterData.PortraitID];
            Label NameLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect/MarginContainer/ColorRect/VBoxContainer/NameLabel");
            NameLabel.Text = warriorOwner.characterData.Name;

            CheckWeaponInfos();
        }
    }
    private void CheckWeaponInfos()
    {
        MarginContainer marginContainer = GetNode<MarginContainer>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/GridContainer/ColorRect3/VBoxContainer/MarginContainer2/ColorRect/MarginContainer");
        foreach (Node child in marginContainer.GetChildren())
        {
            child.QueueFree();
        }
        
        if (warriorOwner.characterData.Weapon.ID == "NONE")
        {
            Control noWeaponPanel = noWeaponPanelScene.Instantiate() as Control;
            marginContainer.AddChild(noWeaponPanel);
        }
        else
        {
            BigWeaponPanel bigWeaponPanel = weaponPanelScene.Instantiate() as BigWeaponPanel;
            bigWeaponPanel.SetWeaponInfos(warriorOwner.characterData.Weapon);
            marginContainer.AddChild(bigWeaponPanel);
        }
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
