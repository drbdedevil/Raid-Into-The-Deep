using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MapGenerator : Node
{
    [Export] public int Rows = 17;
    [Export(PropertyHint.Range, "1,3")] public int StartNodesPerRowMin = 3;
    [Export(PropertyHint.Range, "3,10")] public int StartNodesPerRowMax = 5;
    [Export(PropertyHint.Range, "1,3")] public int NodesPerRowMin = 3;
    [Export(PropertyHint.Range, "3,10")] public int NodesPerRowMax = 6;

    private Random random = new Random();

    public List<List<MapNode>> Generate()
    {
        var map = new List<List<MapNode>>();

        // Start nodes
        int startedNodes = GD.RandRange(StartNodesPerRowMin, StartNodesPerRowMax);
        map.Add(new List<MapNode>());
        for (int i = 0; i < startedNodes; ++i)
        {
            int Place = i;
            int Ost = (NodesPerRowMax - startedNodes) / 2;

            map[0].Add(new MapNode { Row = 0, Col = Place + Ost, Type = MapNodeType.Battle });
        }

        // Intermediate nodes
        for (int row = 1; row < Rows - 1; ++row)
        {
            int count = random.Next(NodesPerRowMin, NodesPerRowMax + 1);
            List<MapNode> nodes = new List<MapNode>();

            for (int col = 0; col < count; ++col)
            {
                MapNodeType type = GetRandomType(row, map[row - 1]);
                nodes.Add(new MapNode { Row = row, Col = col, Type = type, IsActive = false });
            }

            map.Add(nodes);
        }

        // Bosses nodes
        map.Add(new List<MapNode> {
            new MapNode { Row = Rows - 1, Col = 0, Type = MapNodeType.Boss, IsActive = false },
            new MapNode { Row = Rows - 1, Col = 1, Type = MapNodeType.Boss, IsActive = false },
            new MapNode { Row = Rows - 1, Col = 2, Type = MapNodeType.Boss, IsActive = false }
        });

        ConnectNodes(map);

        return map;
    }

    private void ConnectNodes(List<List<MapNode>> map)
    {
        for (int row = 0; row < map.Count - 1; ++row)
        {
            List<MapNode> current = map[row];
            List<MapNode> next = map[row + 1];

            int countCurrent = current.Count;
            int countNext = next.Count;

            if (countCurrent == countNext)
            {
                for (int i = 0; i < countCurrent; ++i)
                {
                    current[i].Next.Add(next[i]);
                }
            }
            else if (countCurrent > countNext)
            {
                bool fromEnd = random.Next(2) == 0;

                if (fromEnd)
                {
                    int currentIndex = countCurrent - 1;
                    int remainingCurrents = countCurrent;

                    for (int i = countNext - 1; i >= 0; --i)
                    {
                        int remainingNexts = i + 1;

                        int minConnections = Math.Max(1, remainingCurrents - (remainingNexts - 1));
                        int maxConnections = Math.Max(minConnections, remainingCurrents / remainingNexts);

                        int connections = random.Next(minConnections, maxConnections + 1);
                        connections = Math.Min(connections, remainingCurrents);

                        for (int j = 0; j < connections; ++j)
                        {
                            current[currentIndex - j].Next.Add(next[i]);
                        }

                        currentIndex -= connections;
                        remainingCurrents -= connections;
                    }

                    while (currentIndex >= 0)
                    {
                        current[currentIndex].Next.Add(next[0]);
                        currentIndex--;
                    }
                }
                else
                {
                    int currentIndex = 0;
                    int remainingCurrents = countCurrent;

                    for (int i = 0; i < countNext; ++i)
                    {
                        int remainingNexts = countNext - i;

                        int minConnections = Math.Max(1, remainingCurrents - (remainingNexts - 1));
                        int maxConnections = Math.Max(minConnections, remainingCurrents / remainingNexts);

                        int connections = random.Next(minConnections, maxConnections + 1);
                        connections = Math.Min(connections, remainingCurrents);

                        for (int j = 0; j < connections; ++j)
                        {
                            current[currentIndex + j].Next.Add(next[i]);
                        }

                        currentIndex += connections;
                        remainingCurrents -= connections;
                    }

                    while (currentIndex < countCurrent)
                    {
                        current[currentIndex].Next.Add(next[countNext - 1]);
                        currentIndex++;
                    }
                }
            }
            else if (countCurrent < countNext)
            {
                bool fromEnd = random.Next(2) == 0;

                if (!fromEnd)
                {
                    int nextIndex = 0;
                    int remainingNext = countNext;

                    for (int i = 0; i < countCurrent; ++i)
                    {
                        int remainingCurrents = countCurrent - i;

                        int minConnections = Math.Max(1, remainingNext - (remainingCurrents - 1));
                        int maxConnections = Math.Max(minConnections, remainingNext / remainingCurrents);

                        int connections = random.Next(minConnections, maxConnections + 1);
                        connections = Math.Min(connections, remainingNext);

                        for (int j = 0; j < connections; ++j)
                        {
                            current[i].Next.Add(next[nextIndex]);
                            nextIndex++;
                        }

                        remainingNext -= connections;
                    }

                    while (nextIndex < countNext)
                    {
                        current[countCurrent - 1].Next.Add(next[nextIndex]);
                        nextIndex++;
                    }
                }
                else
                {
                    int nextIndex = countNext - 1;
                    int remainingNext = countNext;

                    for (int i = countCurrent - 1; i >= 0; --i)
                    {
                        int remainingCurrents = i + 1;

                        int minConnections = Math.Max(1, remainingNext - (remainingCurrents - 1));
                        int maxConnections = Math.Max(minConnections, remainingNext / remainingCurrents);

                        int connections = random.Next(minConnections, maxConnections + 1);
                        connections = Math.Min(connections, remainingNext);

                        for (int j = 0; j < connections; ++j)
                        {
                            current[i].Next.Add(next[nextIndex]);
                            nextIndex--;
                        }

                        remainingNext -= connections;
                    }

                    while (nextIndex >= 0)
                    {
                        current[0].Next.Add(next[nextIndex]);
                        nextIndex--;
                    }
                }
            }

            {
                int i = random.Next(current.Count);
                int existingMin = int.MaxValue;
                int existingMax = int.MinValue;

                foreach (var conn in current[i].Next)
                {
                    int idx = next.IndexOf(conn);
                    existingMin = Math.Min(existingMin, idx);
                    existingMax = Math.Max(existingMax, idx);
                }

                int lowerBound = (i == 0) ? 0 : GetMaxConnectedIndex(current[i - 1], next);
                int upperBound = (i == current.Count - 1) ? next.Count - 1 : GetMinConnectedIndex(current[i + 1], next);

                List<int> candidates = new();
                for (int n = lowerBound; n <= upperBound; ++n)
                {
                    if (n < existingMin || n > existingMax)
                        candidates.Add(n);
                }

                if (candidates.Count > 0)
                {
                    int newNextIndex = candidates[random.Next(candidates.Count)];
                    var target = next[newNextIndex];
                    if (!current[i].Next.Contains(target))
                        current[i].Next.Add(target);
                }
            }
        }
    }

    private MapNodeType GetRandomType(int currentRow, List<MapNode> prevRow)
    {
        if (currentRow == 9)
        {
            return MapNodeType.Treasure;
        }
        else if (currentRow == Rows - 2)
        {
            return MapNodeType.Rest;
        }

        var raceMapEventRows = GameDataManager.Instance.raceMapDatabase.RaceMapInfos;
        float totalWeight = 0f;
        foreach (var raceMapEventRow in raceMapEventRows)
        {
            totalWeight += raceMapEventRow.Weight;
        }
        double randomValue = GD.RandRange(0, totalWeight);

        float currentHeight = 0f;
        int raceMapEventIndex = 0;
        for (; raceMapEventIndex < raceMapEventRows.Count; ++raceMapEventIndex)
        {
            currentHeight += raceMapEventRows[raceMapEventIndex].Weight;
            if (currentHeight >= randomValue)
            {
                break;
            }
        }

        MapNodeType result = raceMapEventRows[raceMapEventIndex].mapNodeType;
        if (currentRow < 6 && (result == MapNodeType.EliteBattle || result == MapNodeType.Rest))
        {
            result = MapNodeType.Battle;
        }
        MapNode eliteBattle = prevRow.FirstOrDefault(mapNode => mapNode.Type == MapNodeType.EliteBattle);
        if (eliteBattle != null && result == MapNodeType.EliteBattle)
        {
            result = MapNodeType.Battle;
        }
        MapNode rest = prevRow.FirstOrDefault(mapNode => mapNode.Type == MapNodeType.Rest);
        if (rest != null && result == MapNodeType.Rest || currentRow == Rows - 3 && result == MapNodeType.Rest)
        {
            result = MapNodeType.Battle;
        }

        return result;
    }

    private int GetMaxConnectedIndex(MapNode node, List<MapNode> next)
    {
        int max = -1;
        foreach (var n in node.Next)
        {
            int idx = next.IndexOf(n);
            if (idx > max) max = idx;
        }
        return max;
    }

    private int GetMinConnectedIndex(MapNode node, List<MapNode> next)
    {
        int min = int.MaxValue;
        foreach (var n in node.Next)
        {
            int idx = next.IndexOf(n);
            if (idx < min) min = idx;
        }
        return min;
    }
}
