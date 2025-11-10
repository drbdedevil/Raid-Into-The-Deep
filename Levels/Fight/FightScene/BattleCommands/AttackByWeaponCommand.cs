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

            if (tile.BattleEntity != null && tile.BattleEntity is IEffectHolder)
            {
                _battleEntity.Weapon.CreateEffectByWeaponData();
                EntityEffect effect = _battleEntity.Weapon.effect as EntityEffect;
                if (tile.BattleEntity.appliedEffects.Any(eff => eff.EffectType == EEffectType.Sleep))
                {
                    tile.BattleEntity.appliedEffects.RemoveAll(eff => eff.EffectType == EEffectType.Sleep);
                }
                else
                {
                    if (effect.EffectType == EEffectType.Freezing || effect.EffectType == EEffectType.Weakening)
                    {
                        effect.entityHolder = tile.BattleEntity;
                        tile.BattleEntity.appliedEffects.Add(effect);
                    }
                    else
                    {
                        tile.BattleEntity.rawEffects.Add(effect);
                    }
                }
            }
        }
        
        WeaponRow row = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == _battleEntity.Weapon.weaponData.Name);
        if (row != null)
        {
            float volum = _battleEntity.Weapon.weaponData.Name == "Мортира" ? 0.2f : 0.6f;
            SoundManager.Instance.PlaySoundOnce(row.SoundPath, volum);
        }
        else
        {
            WeaponRow row2 = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == _battleEntity.Weapon.AttackShapeInfo.Name);
            if (row2 != null)
            {
                float volum = _battleEntity.Weapon.weaponData.Name == "Мортира" ? 0.2f : 0.6f;
                SoundManager.Instance.PlaySoundOnce(row2.SoundPath, volum);
            }
        }

        var entitiesToAttack = _battleEntity.Weapon.CalculateDamageForEntities(_battleEntity, _tilesForAttack);
        foreach (var targetWeaponAttackDamage in entitiesToAttack)
        {
            if (targetWeaponAttackDamage.EntityToAttack is PlayerEntity playerWarrior && _battleEntity is EnemyEntity)
            {
                targetWeaponAttackDamage.EntityToAttack.Health -= targetWeaponAttackDamage.Damage;
                if (targetWeaponAttackDamage.EntityToAttack.Health <= 0)
                {
                    _fightSceneManager.RemovePlayerWarrior(playerWarrior);
                    SoundManager.Instance.PlaySoundOnce("res://Sound/Death.wav", 0.6f);
                }
            }
            else if (targetWeaponAttackDamage.EntityToAttack is EnemyEntity enemyEntity && _battleEntity is PlayerEntity)
            {
                targetWeaponAttackDamage.EntityToAttack.Health -= targetWeaponAttackDamage.Damage;
                if (targetWeaponAttackDamage.EntityToAttack.Health <= 0)
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