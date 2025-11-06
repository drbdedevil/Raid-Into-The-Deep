using Godot;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.Scripts
{
    public class Tile 
    {
        public Tile(Vector2I cartesianCoord, Vector2I isometricCoord, Vector2I tileTextureSize, bool isClosedToSetPlayerWarrior)
        {
            CartesianPosition = cartesianCoord;
            IsometricPosition = isometricCoord;
            TileTextureSize = tileTextureSize;
            IsClosedToSetPlayerWarrior = isClosedToSetPlayerWarrior;
        }
        
        public Vector2I CartesianPosition { get; }
        public Vector2I IsometricPosition { get; }
        public Vector2I TileTextureSize { get; }
        public bool IsClosedToSetPlayerWarrior { get; } = false;
        
        public Fight.BattleEntity? BattleEntity { get; set; }


        public bool IsAllowedToSetBattleEntity => BattleEntity is null;
    }
}