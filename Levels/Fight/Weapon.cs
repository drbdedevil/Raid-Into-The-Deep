using System;
using System.Collections.Generic;
using Godot;

namespace RaidIntoTheDeep.Levels.Fight;

public class Weapon
{
	public Weapon(int attackShapeId)
	{
		AttackShapeInfo = GameDataManager.Instance.attackShapeDatabase.AttackShapes[attackShapeId];
	}

	public AttackShapeInfo AttackShapeInfo { get; private set; }
	
	public List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I targetPosition)
	{
		var direction = CalculateDirection();

		switch (AttackShapeInfo.shapeType)
		{
			case AttackShapeType.Melee: return CalculateMelee();
			case AttackShapeType.LongMelee: return CalculateLongMelee();
			case AttackShapeType.Sweep: return CalculateSweep();
			default: throw new ArgumentOutOfRangeException();
		}
		
		

		AttackDirection CalculateDirection()
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
