using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight
{
    public partial class BattleEntity : Node2D 
    {
        public BattleEntity(Tile tile, string id, int speed, int health, int damage)
        {
            Tile = tile;
            Id = id;
            Speed = speed;
            Health = health;
            Damage = damage;
        }
        
        /// <summary>
        /// Клетка на которой стоит персонаж
        /// </summary>
        public Tile Tile { get; set; }
        
        public string Id { get; set; }
        
        public int Speed { get; set; }
        
        public int Health { get; set; }
        
        public int Damage { get; set; }

    }
}