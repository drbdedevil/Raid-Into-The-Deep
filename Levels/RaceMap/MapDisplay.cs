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

            List<List<MapNode>> map = mapGenerator.Generate();
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
                        mapControl.AddChild(mapNodeButton);

                        Vector2 randomOffset = new Vector2(
                            (float)(random.NextDouble() * 2 - 1) * randomOffsetX,
                            (float)(random.NextDouble() * 2 - 1) * randomOffsetY
                        );

                        mapNodeButton.RandomOffset = randomOffset;
                        mapNodeButton.MapNode = mapNode;

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
                        line.Width = 1.5f;

                        Vector2 startPos = startButton.Position + new Vector2(15f, 30f);
                        Vector2 endPos = endButton.Position + new Vector2(15f, 0f);

                        line.AddPoint(startPos);
                        line.AddPoint(endPos);

                        mapControl.AddChild(line);
                    }
                }
            }
        }
    }
}
