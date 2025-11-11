using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;

public class AttackByWeaponCommand : Command
{
    private BattleEntity _battleEntity { get; set; }
    private List<Tile>  _tilesForAttack { get; set; }
    
    private FightSceneManager _fightSceneManager { get; set; }
    
    public AttackByWeaponCommand(BattleEntity battleEntity, List<Tile> tilesForAttack, FightSceneManager fightSceneManager)
    {
        _battleEntity = battleEntity;
        _tilesForAttack = tilesForAttack;
        _fightSceneManager = fightSceneManager;
    }
    
    public override void Execute()
    {
        foreach (var tile in _tilesForAttack)
        {
            GD.Print($"чувачок с Id-{_battleEntity.Id} ударил по тайлу {tile}");

            if (tile.BattleEntity == null)
                continue;

            _battleEntity.Weapon.CreateEffectByWeaponData();
            var effect = _battleEntity.Weapon.effect as EntityEffect;
            if (effect == null)
                continue;

            if (tile.BattleEntity.HasEffect(EEffectType.Sleep))
            {
                tile.BattleEntity.RemoveEffect(tile.BattleEntity.GetEffect(EEffectType.Sleep));
                GD.Print($"Сущность {tile.BattleEntity.Id} проснулась от удара!");
                if (effect.EffectType is EEffectType.Sleep)
                {
                    continue;
                }
            }

            if (effect.EffectType == EEffectType.Stun && 
                (tile.BattleEntity.HasEffect(EEffectType.ResistanceToStun, true)
              || tile.BattleEntity.HasEffect(EEffectType.ResistanceToStun)))
            {
                GD.Print($"Цель {tile.BattleEntity.Id} иммунна к оглушению!");
                continue;
            }

            if (effect.EffectType is EEffectType.Freezing 
                or EEffectType.Weakening 
                or EEffectType.Sleep
                or EEffectType.Stun)
            {
                if (effect.EffectType == EEffectType.Sleep && tile.BattleEntity.HasEffect(EEffectType.Sleep))
                    continue;

                effect.entityHolder = tile.BattleEntity;
                tile.BattleEntity.AddEffect(effect);

                effect.OnApply();
                GD.Print($"Эффект {effect.EffectType} применён сразу к {tile.BattleEntity.Id}");
            }

            else if (effect.EffectType != EEffectType.ResistanceToStun && 
                    effect.EffectType != EEffectType.Pushing)
            {
                effect.entityHolder = tile.BattleEntity;
                tile.BattleEntity.AddEffect(effect); // tile.BattleEntity.appliedEffects.Add(effect); - если захотим, чтобы эффекты стакались
                GD.Print($"Эффект {effect.EffectType} добавлен в очередь");
            }

            else if (effect.EffectType == EEffectType.Pushing)
            {
                GD.Print($"{_battleEntity.Id} ТОЛКНУЛ {tile.BattleEntity.Id}!");
                // Надо вызвать метод физического толчка
                // например tile.BattleEntity.PushFrom(_battleEntity.Position);
            }
        }

        WeaponRow row = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == _battleEntity.Weapon.weaponData.Name);
        if (row != null)
        {
            float volum = _battleEntity.Weapon.weaponData.Name == "Мортира" ? 0.2f : 0.5f;
            SoundManager.Instance.PlaySoundOnce(row.SoundPath, volum);
        }
        else
        {
            WeaponRow row2 = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == _battleEntity.Weapon.AttackShapeInfo.Name);
            if (row2 != null)
            {
                float volum = _battleEntity.Weapon.weaponData.Name == "Мортира" ? 0.2f : 0.5f;
                SoundManager.Instance.PlaySoundOnce(row2.SoundPath, volum);
            }
        }

        var entitiesToAttack = _battleEntity.Weapon.CalculateDamageForEntities(_battleEntity, _tilesForAttack);
        foreach (var targetWeaponAttackDamage in entitiesToAttack)
        {
            if (targetWeaponAttackDamage.EntityToAttack is PlayerEntity playerWarrior && _battleEntity is EnemyEntity)
            {
                targetWeaponAttackDamage.EntityToAttack.ApplyDamage(targetWeaponAttackDamage.Damage);
                if (targetWeaponAttackDamage.EntityToAttack.IsDead())
                {
                    _fightSceneManager.RemovePlayerWarrior(playerWarrior);
                    SoundManager.Instance.PlaySoundOnce("res://Sound/Death.wav", 0.6f);
                }
            }
            else if (targetWeaponAttackDamage.EntityToAttack is EnemyEntity enemyEntity && _battleEntity is PlayerEntity)
            {
                targetWeaponAttackDamage.EntityToAttack.ApplyDamage(targetWeaponAttackDamage.Damage);
                if (targetWeaponAttackDamage.EntityToAttack.IsDead())
                {
                    _fightSceneManager.RemoveEnemyWarrior(enemyEntity);
                    SoundManager.Instance.PlaySoundOnce("res://Sound/Death.wav", 0.6f);
                }
            }
        }
    }

    public override void UnExecute()
    {
        foreach (var tile in _tilesForAttack)
        {
            GD.Print($"чувачок с Id-{ _battleEntity.Id} отменил удар по тайлу {tile}");
        }
    }
}