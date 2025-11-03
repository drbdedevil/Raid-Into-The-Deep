using Godot;
using System;
using System.Collections.Generic;

public partial class MapNodeButton : TextureButton
{
    public MapNode mapNode { get; set; }
    public bool bShouldBeDisabled = true;

    public override void _Ready()
    {
        Pressed += OnMapNodeButtonPressed;
    }

    private void OnMapNodeButtonPressed()
    {
        GD.Print("Pressed on: " + mapNode.Type);

        RunMapDataManager runMapDataManager = GameDataManager.Instance.runMapDataManager;
        switch (mapNode.Type)
        {
            case MapNodeType.Battle:
                runMapDataManager.RunBattle(mapNode);
                break;
            case MapNodeType.Boss:
                runMapDataManager.RunBossBattle(mapNode);
                break;
            case MapNodeType.EliteBattle:
                runMapDataManager.RunEliteBattle(mapNode);
                break;
            case MapNodeType.RandomEvent:
                int randValue = GD.RandRange(0, 100);
                if (randValue >= 50)
                {
                    runMapDataManager.HealTeam();
                    mapNode.Type = MapNodeType.Rest;
                }
                else
                {
                    runMapDataManager.GiveAwardToPlayer();
                    mapNode.Type = MapNodeType.Treasure;
                }
                SuccesfulAction();
                break;
            case MapNodeType.Rest:
                runMapDataManager.HealTeam();
                SuccesfulAction();
                break;
            case MapNodeType.Treasure:
                runMapDataManager.GiveAwardToPlayer();
                SuccesfulAction();
                break;
            default:
                break;
        }
    }
    public void SuccesfulAction()
    {
        mapNode.PassMapNode();

        GameDataManager.Instance.runMapDataManager.EmitSignal(RunMapDataManager.SignalName.OnRunMapListUpdate);
    }
    
    public void SetRandomOffset(Vector2 value)
    {
        mapNode.randomOffsetX = value.X;
        mapNode.randomOffsetY = value.Y;
    }
    public Vector2 GetRandomOffset()
    {
        return new Vector2(mapNode.randomOffsetX, mapNode.randomOffsetY);
    }
}
