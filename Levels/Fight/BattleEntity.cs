using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight
{
    public partial class BattleEntity : Node2D 
    {
        /// <summary>
        /// Клетка на которой стоит персонаж
        /// </summary>
        public Tile Tile { get; set; }
        
        public string ID { get; set; }

    }
}