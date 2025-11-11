using Godot;
using System.Linq;
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
            _speed = speed;
            Health = health;
            _damage = damage;
            Weapon = weapon;
        }

        public bool CanAct { get; set; } = true;

        public List<Effect> appliedEffects { get; set; } = new();
        // public List<Effect> rawEffects { get; set; } = new();

        public Weapon Weapon { get; set; }

        /// <summary>
        /// Клетка на которой стоит персонаж
        /// </summary>
        public Tile Tile { get; set; }

        public string Id { get; }

        private int _speed;
        public int Speed
        {
            get
            {
                if (appliedEffects.Any(appliedEffect => appliedEffect.EffectType == EEffectType.Freezing))
                {
                    return (int)Mathf.Floor(_speed / 2);
                }

                return _speed;
            }
            set { _speed = value; }
        }

        public int Health { get; set; }

        private int _damage;
        public int Damage
        {
            get
            {
                if (appliedEffects.Any(appliedEffect => appliedEffect.EffectType == EEffectType.Weakening))
                {
                    return (int)Mathf.Floor(_damage * 0.5);
                }

                return _damage;
            }
            set { _damage = value; }
        }

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

        public void AddEffect(Effect effect)
        {
            var existing = appliedEffects.FirstOrDefault(e => e.EffectType == effect.EffectType);
            if (existing != null)
            {
                existing.OnRemove();
                appliedEffects.Remove(existing);
            }

            appliedEffects.Add(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            effect.OnRemove();
            appliedEffects.Remove(effect);
        }

        public bool HasEffect(EEffectType type, bool IsOnWeapon = false)
        {
            if (IsOnWeapon)
            {
                return Weapon.effect.EffectType == type;
            }

            return appliedEffects.Any(e => e.EffectType == type);
        }
        public Effect GetEffect(EEffectType type)
        {
            return appliedEffects.FirstOrDefault(e => e.EffectType == type);
        }
        
        public virtual void ApplyDamage(int damage)
        {
            Health = Health - damage;
        }
    }
}