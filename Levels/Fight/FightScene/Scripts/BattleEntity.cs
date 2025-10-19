using Godot;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.Scripts
{
    public partial class BattleEntity : Node2D 
    {
        /// <summary>
        /// Клетка на которой стоит персонаж
        /// </summary>
        public Tile Tile { get; set; }
        
        public CharacterData Character { get; set; }
        

    }
}