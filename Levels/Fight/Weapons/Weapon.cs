using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public abstract class Weapon
{
	public Weapon(int attackShapeId, WeaponData InWeaponData)
	{
		AttackShapeInfo = GameDataManager.Instance.attackShapeDatabase.AttackShapes[attackShapeId];
		weaponData = InWeaponData;

		effect = CreateEffectByWeaponData();
	}

	public AttackShapeInfo AttackShapeInfo { get; private set; }
	public WeaponData weaponData { get; private set; }
	public Effect effect { get; private set; }
	
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

	protected Effect CreateEffectByWeaponData()
    {
		EffectInfo effectInfo = GameDataManager.Instance.effectDatabase.Effects[weaponData.EffectID];
		return new EntityEffect(effectInfo.effectType, 0);
    }
}
