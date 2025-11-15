using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.EffectManagerLogic;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public abstract class Weapon
{
	public Weapon(int attackShapeId, WeaponData InWeaponData)
	{
		AttackShapeInfo = GameDataManager.Instance.attackShapeDatabase.AttackShapes[attackShapeId];
		weaponData = InWeaponData;

		DefineWeaponTypeByName();
		CreateEffectByWeaponData();
	}

	public AttackShapeInfo AttackShapeInfo { get; private set; }
	public WeaponData weaponData { get; private set; }
	public Effect effect { get; private set; }
	public EWeaponType weaponType { get; private set; }
	
	protected AttackDirection CalculateDirection( Vector2 startPosition, Vector2 targetPosition)
	{
		var dx = targetPosition.X - startPosition.X;
		var dy = targetPosition.Y - startPosition.Y;
		
		if (Mathf.Abs(dx) > Mathf.Abs(dy))
		{
			if (dx > 0)
				return AttackDirection.Right;
			return AttackDirection.Left;
		}
		if (dy > 0)
			return AttackDirection.Down;
		return AttackDirection.Up;
	}
	public abstract List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map);

	public void CreateEffectByWeaponData()
	{
		// навешивание эффектов на оружие врагов
		if (weaponData.EffectID == -1)
        {
			WeaponRow weaponRow = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == weaponData.Name);
			if (weaponRow != null)
            {
                switch (weaponRow.weaponType)
                {
					case EWeaponType.Sword:
						effect = new FreezingEntityEffect(3);
						return;
					case EWeaponType.Dagger:
						effect = new PoisonEntityEffect(3);
						return;
					case EWeaponType.Artillery:
						effect = new FireEntityEffect(3);
						return;
					case EWeaponType.Rapier:
						effect = new StunEntityEffect(3);
						return;	
                    default:
						break;
                }
            }
            effect = null;
			return;
        }

		EffectInfo effectInfo = GameDataManager.Instance.effectDatabase.Effects[weaponData.EffectID];
		switch (effectInfo.effectType)
		{
			case EEffectType.Poison:
				effect = new PoisonEntityEffect(effectInfo.duration);
				break;
			case EEffectType.Fire:
				effect = new FireEntityEffect(effectInfo.duration);
				break;
			case EEffectType.Stun:
				effect = new StunEntityEffect(effectInfo.duration);
				break;
			case EEffectType.ResistanceToStun:
				effect = new ResistanceToStunEntityEffect(effectInfo.duration);
				break;
			case EEffectType.Freezing:
				effect = new FreezingEntityEffect(effectInfo.duration);
				break;
			case EEffectType.Weakening:
				effect = new WeakeningEntityEffect(effectInfo.duration);
				break;
			case EEffectType.Pushing:
				effect = new PushingEntityEffect(effectInfo.duration);
				break;
			case EEffectType.Sleep:
				effect = new SleepEntityEffect(effectInfo.duration);
				break;
			default:
				break;
		}

	}
	private void DefineWeaponTypeByName()
    {
        switch (weaponData.Name)
        {
            case "Меч":
				weaponType = EWeaponType.Sword;
				break;
			case "Копьё":
				weaponType = EWeaponType.Spear;
				break;
			case "Рапира":
				weaponType = EWeaponType.Rapier;
				break;
			case "Кинжал":
				weaponType = EWeaponType.Dagger;
				break;
			case "Дробовик":
				weaponType = EWeaponType.Shotgun;
				break;
			case "Мортира":
				weaponType = EWeaponType.Artillery;
				break;
			case "Арбалет":
				weaponType = EWeaponType.Crossbow;
				break;
			default:
				break;	
        }
    }

	public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}

