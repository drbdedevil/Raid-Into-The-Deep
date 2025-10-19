using Godot;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.Scripts
{
    public class Tile 
    {
        public Tile(Vector2I cartesianCoord, Vector2I isometricCoord, Vector2I tileTextureSize)
        {
            CartesianPosition = cartesianCoord;
            IsometricPosition = isometricCoord;
            TileTextureSize = tileTextureSize;
        }
        
        public Vector2I CartesianPosition { get; }
        public Vector2I IsometricPosition { get; }
        public Vector2I TileTextureSize { get; }
        
        public BattleEntity? BattleEntity { get; set; }
        
    }
}