using System;
using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public class MeleeWeapon : Weapon
{
    public MeleeWeapon(int attackShapeId) : base(attackShapeId)
    {
    }

    public override List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map)
    {
        var direction = CalculateDirection(startPosition, playerTargetPosition);

		switch (AttackShapeInfo.shapeType)
		{
			case AttackShapeType.Melee: return CalculateMelee();
			case AttackShapeType.LongMelee: return CalculateLongMelee();
			case AttackShapeType.Sweep: return CalculateSweep();
			default: throw new ArgumentOutOfRangeException();
		}
		

		List<Vector2I> CalculateMelee()
		{
			switch (direction)
			{
				case AttackDirection.Down: return [startPosition + Vector2I.Down];
				case AttackDirection.Up: return [startPosition + Vector2I.Up];
				case AttackDirection.Left: return [startPosition + Vector2I.Left];
				case AttackDirection.Right: return [startPosition + Vector2I.Right];
				default: throw new ArgumentOutOfRangeException();
			}
		}
		
		List<Vector2I> CalculateLongMelee()
		{
			switch (direction)
			{
				case AttackDirection.Down: return [startPosition + Vector2I.Down * 2];
				case AttackDirection.Up: return [startPosition + Vector2I.Up * 2];
				case AttackDirection.Left: return [startPosition + Vector2I.Left * 2];
				case AttackDirection.Right: return [startPosition + Vector2I.Right * 2];
				default: throw new ArgumentOutOfRangeException();
			}
		}
		
		List<Vector2I> CalculateSweep()
		{
			switch (direction)
			{
				case AttackDirection.Down: return [startPosition + Vector2I.Down + Vector2I.Left, startPosition + Vector2I.Down, startPosition + Vector2I.Down + Vector2I.Right];
				case AttackDirection.Up: return [startPosition + Vector2I.Up + Vector2I.Left, startPosition + Vector2I.Up, startPosition + Vector2I.Up + Vector2I.Right];
				case AttackDirection.Left: return [startPosition + Vector2I.Left + Vector2I.Up,startPosition +   Vector2I.Left,startPosition +  Vector2I.Left + Vector2I.Down];
				case AttackDirection.Right: return [startPosition + Vector2I.Right + Vector2I.Up,  startPosition + Vector2I.Right, startPosition + Vector2I.Right + Vector2I.Down];
				default: throw new ArgumentOutOfRangeException();
			}
		}
    }
}