using Godot;
using System;
using System.Linq;

public partial class SkillInfoPanel : Control
{
    public void ShowSkillInfos(CharacterData characterData, ESkillType currentSkill)
    {
        TextureRect skillRect = GetNode<TextureRect>("ColorRect/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer/TextureRect");
        Label NameLabel = GetNode<Label>("ColorRect/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer/NameLabel");
        Label TextLabel = GetNode<Label>("ColorRect/MarginContainer/ColorRect/MarginContainer/VBoxContainer/TextLabel");

        Label CurrentLevelLabel = GetNode<Label>("ColorRect/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer2/CurrentLevel");
        Label MaxLevelLabel = GetNode<Label>("ColorRect/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer2/MaxLevel");

        if (SkillExtensions.IsActive(currentSkill))
        {
            SkillRow skillRow = GameDataManager.Instance.activeSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillType == currentSkill);
            if (skillRow != null)
            {
                skillRect.Texture = skillRow.skillTextureBase;
                NameLabel.Text = skillRow.skillName;
                TextLabel.Text = skillRow.skillDescription;

                if (characterData.ActiveSkills.Contains(skillRow.skillName))
                {
                    CurrentLevelLabel.Text = "1";
                }
                else
                {
                    CurrentLevelLabel.Text = "0";
                }
                MaxLevelLabel.Text = "1";
            }
        }
        else if (SkillExtensions.IsPassive(currentSkill))
        {
            SkillRow skillRow = GameDataManager.Instance.passiveSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillType == currentSkill);
            if (skillRow != null)
            {
                skillRect.Texture = skillRow.skillTextureBase;
                NameLabel.Text = skillRow.skillName;
                TextLabel.Text = skillRow.skillDescription;

                PassiveSkillProgressionRow progressionRow = GameDataManager.Instance.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(progressionRow => progressionRow.skillType == currentSkill);
                if (progressionRow != null)
                {
                    MaxLevelLabel.Text = progressionRow.increments.Count.ToString();
                }

                if (characterData.PassiveSkillLevels.ContainsKey(skillRow.skillName))
                {
                    CurrentLevelLabel.Text = characterData.PassiveSkillLevels[skillRow.skillName].ToString();
                }
            }
        }
    }
    
    public void ShrinkInfoPanel()
    {
        MarginContainer marginContainer = GetNode<MarginContainer>("ColorRect/MarginContainer/ColorRect/MarginContainer");
        ColorRect colorRect = GetNode<ColorRect>("ColorRect");
        VBoxContainer vBoxContainer = GetNode<VBoxContainer>("ColorRect/MarginContainer/ColorRect/MarginContainer/VBoxContainer");

        HBoxContainer hBoxContainer1 = GetNode<HBoxContainer>("ColorRect/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer");
        HBoxContainer hBoxContainer2 = GetNode<HBoxContainer>("ColorRect/MarginContainer/ColorRect/MarginContainer/VBoxContainer/HBoxContainer2");
        Label TextLabel = GetNode<Label>("ColorRect/MarginContainer/ColorRect/MarginContainer/VBoxContainer/TextLabel");
        vBoxContainer.SetSize(new Vector2(vBoxContainer.Size.X, 0));
        // GD.Print(vBoxContainer.Size.Y);
        // GD.Print(colorRect.Size.Y);

        // GD.Print(hBoxContainer1.Size.Y);
        // GD.Print(TextLabel.Size.Y);
        // GD.Print(hBoxContainer2.Size.Y);

        colorRect.SetSize(new Vector2(colorRect.Size.X + 20, vBoxContainer.Size.Y + 20));
        // vBoxContainer.SetSize(new Vector2(colorRect.Size.X, colorRect.Size.Y));
        // GD.Print(colorRect.Position);
        colorRect.SetPosition(new Vector2(colorRect.Position.X, -colorRect.Size.Y));
        // GD.Print(colorRect.Position);
    }
}
