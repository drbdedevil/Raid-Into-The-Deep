using Godot;
using System;

public partial class CapacityView : MarginContainer
{
    public override void _Ready()
	{

	}

	public Int32 GetCurrentValue()
	{
		Label CurrentLabel = GetNode<Label>("ColorRect/MarginContainer/HBoxContainer/CurrentLabel");
		return CurrentLabel.Text.ToInt();
	}
	public Int32 GetMaxValue()
	{
		Label MaxLabel = GetNode<Label>("ColorRect/MarginContainer/HBoxContainer/MaxLabel");
		return MaxLabel.Text.ToInt();
	}
	public void SetCurrentValue(Int32 Value)
	{
		Label CurrentLabel = GetNode<Label>("ColorRect/MarginContainer/HBoxContainer/CurrentLabel");
		CurrentLabel.Text = Value.ToString();
	}
	public void SetMaxValue(Int32 Value)
	{
		Label MaxLabel = GetNode<Label>("ColorRect/MarginContainer/HBoxContainer/MaxLabel");
		MaxLabel.Text = Value.ToString();
	}
}
