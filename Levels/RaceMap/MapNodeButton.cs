using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
        if (!GameDataManager.Instance.currentData.livingSpaceData.ReservedCharacters.Any() && 
            !GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.Any())
        {
            NotificationSystem.Instance.ShowMessage("У тебя никого нет, найми и возьми в свою команду!", EMessageType.Alert);
            return;
        }
        
        if (GameDataManager.Instance.currentData.livingSpaceData.UsedCharacters.Count == 0)
        {
            NotificationSystem.Instance.ShowMessage("Возьми с собой кого-нибудь!", EMessageType.Alert);
            return;
        }

        GD.Print("Pressed on: " + mapNode.Type);

        RunMapDataManager runMapDataManager = GameDataManager.Instance.runMapDataManager;
        switch (mapNode.Type)
        {
            case MapNodeType.Battle:
                runMapDataManager.RunBattle(mapNode);
                GameDataManager.Instance.currentData.commandBlockData.RaidCount += 1;
                SoundManager.Instance.PlaySoundOnce("res://Sound/Music/EnterTheBattle.mp3", 0.4f);
                break;
            case MapNodeType.SpiderBoss:
                runMapDataManager.RunBossBattle(mapNode, MapNodeType.SpiderBoss);
                GameDataManager.Instance.currentData.commandBlockData.RaidCount += 1;
                SoundManager.Instance.PlaySoundOnce("res://Sound/Music/EnterTheBattle.mp3", 0.4f);
                break;
            case MapNodeType.TankBoss:
                runMapDataManager.RunBossBattle(mapNode, MapNodeType.TankBoss);
                GameDataManager.Instance.currentData.commandBlockData.RaidCount += 1;
                SoundManager.Instance.PlaySoundOnce("res://Sound/Music/EnterTheBattle.mp3", 0.4f);
                break;
            case MapNodeType.VegetableBoss:
                runMapDataManager.RunBossBattle(mapNode, MapNodeType.VegetableBoss);
                GameDataManager.Instance.currentData.commandBlockData.RaidCount += 1;
                SoundManager.Instance.PlaySoundOnce("res://Sound/Music/EnterTheBattle.mp3", 0.4f);
                break;
            case MapNodeType.EliteBattle:
                runMapDataManager.RunEliteBattle(mapNode);
                GameDataManager.Instance.currentData.commandBlockData.RaidCount += 1;
                SoundManager.Instance.PlaySoundOnce("res://Sound/Music/EnterTheBattle.mp3", 0.4f);
                break;
            case MapNodeType.RandomEvent:
                int randValue = GD.RandRange(0, 100);
                if (randValue >= 50)
                {
                    runMapDataManager.HealTeam();
                    SoundManager.Instance.PlaySoundOnce("res://Sound/Music/RaceMap/Otdych.wav", 0.4f);
                    mapNode.Type = MapNodeType.Rest;
                }
                else
                {
                    runMapDataManager.GiveAwardToPlayer();
                    SoundManager.Instance.PlaySoundOnce("res://Sound/Interface/CreateWeapon4.wav", 0.4f);
                    mapNode.Type = MapNodeType.Treasure;
                }
                GameDataManager.Instance.Save();
                SuccesfulAction();
                break;
            case MapNodeType.Rest:
                runMapDataManager.HealTeam();
                SoundManager.Instance.PlaySoundOnce("res://Sound/Music/RaceMap/Otdych.wav", 0.4f);
                GameDataManager.Instance.Save();
                SuccesfulAction();
                break;
            case MapNodeType.Treasure:
                runMapDataManager.GiveAwardToPlayer();
                SoundManager.Instance.PlaySoundOnce("res://Sound/Interface/CreateWeapon4.wav", 0.4f);
                GameDataManager.Instance.Save();
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
