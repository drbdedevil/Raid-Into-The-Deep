using Godot;
using System.Linq;
using System.Collections.Generic;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using RaidIntoTheDeep.Levels.Fight.Weapons;

namespace RaidIntoTheDeep.Levels.Fight
{
    public partial class BattleEntity : Node2D, IEffectHolder
    {
        public BattleEntity(Tile tile, Weapon weapon, string id, int speed, int health, int damage, int damageByEffect)
        {
            Tile = tile;
            Id = id;
            _speed = speed;
            Health = health;
            _damage = damage;
            _damageByEffect = damageByEffect;
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
                int result = _speed;
                if (appliedEffects.Any(appliedEffect => appliedEffect.EffectType == EEffectType.Freezing))
                {
                    result = (int)Mathf.Floor(result / 2);
                }
                if (appliedEffects.Any(appliedEffect => appliedEffect.EffectType == EEffectType.SevereWound))
                {
                    result = (int)Mathf.Floor(result / 2);
                }

                return result;
            }
            set { _speed = value; }
        }

        public int Health { get; set; }

        private int _damageByEffect;
        public int DamageByEffect
        {
            get { return _damageByEffect; }
            set { _damage = value; }
        }

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

        public virtual void ApplyDamage(BattleEntity instigator, int damage)
        {
            if (appliedEffects.Any(appliedEffect => appliedEffect.EffectType == EEffectType.Defense))
            {
                damage /= 2;
            }

            if (instigator != null)
            {
                if (appliedEffects.Any(appliedEffect => appliedEffect.EffectType == EEffectType.ReserveDamage))
                {
                    instigator.ApplyDamage(null, damage / 2);
                }
            }

            Health = Health - damage;
        }
        public virtual bool IsDead()
        {
            return Health <= 0;
        }
    }
}