using Godot;
using System;

public partial class TrainingPits : Node, IStackPage
{
    


    public void OnShow()
	{
        GD.Print("TrainingPits Popup shown");
    }
    public void OnHide()
	{
        GD.Print("TrainingPits Popup hidden");
    }
}
