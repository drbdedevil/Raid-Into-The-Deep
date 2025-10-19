using Godot;
using BattleEntity = RaidIntoTheDeep.Levels.Fight.FightScene.Scripts.BattleEntity;

namespace RaidIntoTheDeep.Levels.Fight.PrepareFightScene;

public partial class PrepareFightManager : Node2D
{

    private char[,] _enemiesOnMap = new char[,]
    {
        {'e', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x'},
        {'x', 'x', 'x', 'x', 'x', 'x', 'x', 'e'}
    };

    private FightScene.Scripts.MapManager _mapManager = new FightScene.Scripts.MapManager();
    
    public override void _Ready()
    {
        _mapManager = GetNode<FightScene.Scripts.MapManager>("Map");
        var enemyScene = GD.Load<PackedScene>("res://Levels/Fight/FightScene/Enemy.tscn");

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (_enemiesOnMap[y, x] == 'e')
                {
                    var enemy = enemyScene.Instantiate<BattleEntity>();
                    var tile = _mapManager.GetTileByCartesianCoord(new Vector2I(x, y));
                    _mapManager.SetEnemyOnTile(tile!, enemy);
                    GD.Print("Here");
                }
            }
        }
        
    }
}