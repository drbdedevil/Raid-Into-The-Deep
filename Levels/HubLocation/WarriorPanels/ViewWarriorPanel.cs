using Godot;
using System;

public partial class ViewWarriorPanel : Control
{
	[Signal]
	public delegate void WarriorPanelMouseEnterEventHandler(string characterDataId);
	[Signal]
	public delegate void WarriorPanelMouseExitEventHandler(string characterDataId);

	public bool bShouldChangeCharacterList = true;
	public bool bShouldShowThatCharacterHasSkillPoints = true;
	public override void _Ready()
	{
		MouseEntered += OnMouseEnter;
		MouseExited += OnMouseExit;

		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
		SkillStar skillStar = GetNode<SkillStar>("PanelContainer/MarginContainer2/SkillStar");
		if (bShouldShowThatCharacterHasSkillPoints)
		{
			var experienceDatas = GameDataManager.Instance.charactersExperienceLevelsDatabase.Levels;
			if (warrior.characterData.SkillPoints > 0 && warrior.characterData.Level < experienceDatas.Count)
			{
				skillStar.Start();
			}
			else
			{
				skillStar.Stop();
			}
		}
		else
		{
			skillStar.Stop();
		}
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (!bShouldChangeCharacterList)
		{
			return;
		}

		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
			{
				Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
				warrior.ReplaceCharacter();
			}
		}
	}

	public void SetCharacterInfosToWarriorPanel(CharacterData InCharacterData, bool ShouldHideAbilityToChange = false)
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
		warrior.SetCharacterInfos(InCharacterData);
		warrior.bShouldHideAbilityToChange = ShouldHideAbilityToChange;
	}

	public string GetCharacterID()
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");
		return warrior.characterData.ID;
	}

	private void OnMouseEnter()
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");

		EmitSignal(SignalName.WarriorPanelMouseEnter, warrior.characterData.ID);
	}

	private void OnMouseExit()
	{
		Warrior warrior = GetNode<Warrior>("PanelContainer/MarginContainer/WarriorPanel");

		EmitSignal(SignalName.WarriorPanelMouseExit, warrior.characterData.ID);
	}
}
