using Godot;
using System;
using System.Linq;

public partial class BigSkillPanel : Control
{
    public void SetSkillInfos(string currentSkill)
    {
        SkillRow skillRow = GameDataManager.Instance.activeSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillName == currentSkill);
        if (skillRow != null)
        {
            TextureRect skillRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
            Label NameLabel = GetNode<Label>("VBoxContainer/HBoxContainer/MarginContainer/NameLabel");
            Label TextLabel = GetNode<Label>("VBoxContainer/TextLabel");

            skillRect.Texture = skillRow.skillTextureActive;
            NameLabel.Text = skillRow.skillName;
            TextLabel.Text = skillRow.skillDescription;
        }
    }
}
