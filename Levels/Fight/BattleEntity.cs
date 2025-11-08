using Godot;
using System.Collections.Generic;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using RaidIntoTheDeep.Levels.Fight.Weapons;

namespace RaidIntoTheDeep.Levels.Fight
{
    public partial class BattleEntity : Node2D, IEffectHolder
    {
        public BattleEntity(Tile tile, Weapon weapon, string id, int speed, int health, int damage)
        {
            Tile = tile;
            Id = id;
            Speed = speed;
            Health = health;
            Damage = damage;
            Weapon = weapon;
        }

        public List<Effect> appliedEffects { get; set; } = new();
        public List<Effect> rawEffects { get; set; } = new();
        
        public Weapon Weapon { get; set; }
        
        /// <summary>
        /// Клетка на которой стоит персонаж
        /// </summary>
        public Tile Tile { get; set; }
        
        public string Id { get; }
        
        public int Speed { get; set; }
        
        public int Health { get; set; }
        
        public int Damage { get; set; }
        
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BattleEntity battleEntity)
            {
                return Id == battleEntity.Id;
            }
            return false;
        }

    }
}