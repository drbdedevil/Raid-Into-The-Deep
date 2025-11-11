using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight;

public partial class BattleMapInitStateManager : Node
{
    public static BattleMapInitStateManager Instance { get; private set; }
    
    public List<Tile> Tiles { get; private set; } = [];
    public List<PlayerEntity> PlayerEntities { get; private set; } = [];
    public List<EnemyEntity> EnemyEntities { get; private set; } = [];
    public List<ObstacleEntity> ObstacleEntities { get; private set; } = [];
    
    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            QueueFree();
            return;
        }
        Instance = this;
        
    }

    /// <summary>
    /// По моей задумке будет вызываться ровно один раз при нажатии кнопки "В бой"
    /// </summary>
    public void SetMapData(List<Tile> tiles)
    {
        Tiles = tiles;
        foreach (var tile in Tiles)
        {
            if (tile.BattleEntity != null)
            {
                if (tile.BattleEntity is PlayerEntity playerEntity) PlayerEntities.Add(playerEntity);
                else if (tile.BattleEntity is EnemyEntity enemyEntity) EnemyEntities.Add(enemyEntity);
            }
            if (tile.ObstacleEntity is not null) ObstacleEntities.Add(tile.ObstacleEntity);
        }
    }
}