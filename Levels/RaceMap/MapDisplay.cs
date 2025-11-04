using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class MapDisplay : Node
{
    [Export] public PackedScene mapNodeScene;
    [Export] public float spacingX = 200;
    [Export] public float spacingY = 150;
    [Export] public float randomOffsetX = 50f;
    [Export] public float randomOffsetY = 30f;
    public override void _Ready()
    {
        Control mapControl = GetTree().Root.FindChild("MapControl", true, false) as Control;
        mapControl.Resized += Display;

        GameDataManager.Instance.runMapDataManager.OnRunMapListUpdate += Display;
    }
    public override void _ExitTree()
	{
		GameDataManager.Instance.runMapDataManager.OnRunMapListUpdate -= Display;
	}

    private void Display()
    {
        Control mapControl = GetTree().Root.FindChild("MapControl", true, false) as Control;
        MapGenerator mapGenerator = GetTree().Root.FindChild("MapGenerator", true, false) as MapGenerator;

        if (mapGenerator != null && mapControl != null)
        {
            foreach (Node child in mapControl.GetChildren())
            {
                child.QueueFree();
            }

            List<List<MapNode>> map = new List<List<MapNode>>();
            if (GameDataManager.Instance.currentData.runMapData.bShouldRegenerate)
            {
                map = mapGenerator.Generate();
                GameDataManager.Instance.currentData.runMapData.runMapList = map;
            }
            else
            {
                map = GameDataManager.Instance.currentData.runMapData.runMapList;
            }

            float centerX = mapControl.GetSize().X / 2;

            Random random = new Random();

            Dictionary<MapNode, MapNodeButton> nodeButtons = new Dictionary<MapNode, MapNodeButton>();

            int rowIndex = 0;
            foreach (List<MapNode> row in map)
            {
                int mapNodeCount = row.Count;
                float supposedWidth = (spacingX + 20f) * mapNodeCount;

                float nextSupposedWidth = 0;
                List<MapNode> nextRow = null;
                if (map.Count - 1 != rowIndex)
                {
                    nextRow = map[rowIndex + 1];
                    nextSupposedWidth = (spacingX + 20f) * nextRow.Count;
                }

                foreach (MapNode mapNode in row)
                {
                    RaceMapEventRow raceMapEventRow = GameDataManager.Instance.raceMapDatabase.RaceMapInfos.FirstOrDefault(raceRow => raceRow.mapNodeType == mapNode.Type);

                    if (raceMapEventRow != null)
                    {
                        MapNodeButton mapNodeButton = mapNodeScene.Instantiate() as MapNodeButton;
                        mapNodeButton.TextureNormal = raceMapEventRow.unpassedNodeIcon;
                        mapNodeButton.TextureHover = raceMapEventRow.hoverNodeIcon;
                        mapNodeButton.TexturePressed = raceMapEventRow.pressedNodeIcon;
                        mapNodeButton.TextureDisabled = raceMapEventRow.disabledNodeIcon;
                        mapNodeButton.mapNode = mapNode;

                        if (!mapNode.IsActive)
                        {
                            mapNodeButton.Disabled = true;
                        }
                        if (mapNode.IsPassed)
                        {
                            mapNodeButton.TextureDisabled = raceMapEventRow.passedNodeIcon;
                        }

                        mapControl.AddChild(mapNodeButton);

                        Vector2 randomOffset = new Vector2();
                        if (GameDataManager.Instance.currentData.runMapData.bShouldRegenerate)
                        {
                            randomOffset = new Vector2(
                                (float)(random.NextDouble() * 2 - 1) * randomOffsetX,
                                (float)(random.NextDouble() * 2 - 1) * randomOffsetY
                            );
                            mapNodeButton.SetRandomOffset(randomOffset);
                            randomOffset = mapNodeButton.GetRandomOffset();
                        }
                        else
                        {
                            randomOffset = mapNodeButton.GetRandomOffset();
                        }

                        mapNodeButton.Position = new Vector2(
                            mapNode.Col * spacingX + centerX - supposedWidth / 2,
                            (mapNode.Row + 1) * spacingY
                        ) + randomOffset;

                        nodeButtons[mapNode] = mapNodeButton;
                    }
                }

                rowIndex++;
            }

            foreach (List<MapNode> row in map)
            {
                foreach (MapNode mapNode in row)
                {
                    if (!nodeButtons.ContainsKey(mapNode)) continue;

                    MapNodeButton startButton = nodeButtons[mapNode];

                    foreach (MapNode next in mapNode.Next)
                    {
                        if (!nodeButtons.ContainsKey(next)) continue;

                        MapNodeButton endButton = nodeButtons[next];

                        Line2D line = new Line2D();
                        line.Width = 1.8f;

                        Vector2 startPos = startButton.Position + new Vector2(15f, 30f);
                        Vector2 endPos = endButton.Position + new Vector2(15f, 0f);

                        line.AddPoint(startPos);
                        line.AddPoint(endPos);

                        if (mapNode.IsPassed && next.IsActive)
                        {
                            line.DefaultColor = new Color("#d19f2c");
                        }
                        else if (mapNode.IsPassed && next.IsPassed)
                        {
                            line.DefaultColor = new Color("#271b00ff");
                        }
                        else
                        {
                            line.DefaultColor = new Color("#715b2aff");
                        }

                        mapControl.AddChild(line);
                    }
                }
            }
        }
        
        GameDataManager.Instance.currentData.runMapData.bShouldRegenerate = false;
    }
}
