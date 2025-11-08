using System;
using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public class RangeWeapon : Weapon
{
    public RangeWeapon(int attackShapeId) : base(attackShapeId)
    {
    }

    public override List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map)
    {
        var direction = CalculateDirection(startPosition, playerTargetPosition);

		switch (AttackShapeInfo.shapeType)
		{
			case AttackShapeType.ScatterShot: return CalculateShotgun();
			case AttackShapeType.Ranged: return CalculateCrossbow();
			default: throw new ArgumentOutOfRangeException();
		}

		List<Vector2I> CalculateShotgun()
		{
			switch (direction)
			{
				case AttackDirection.Down:
				{
					List<Vector2I> result = CalculateMiddleLine(direction);
					result.AddRange(CalculateDiagonalLine(AttackDirection.Left, AttackDirection.Down));
					result.AddRange(CalculateDiagonalLine(AttackDirection.Right, AttackDirection.Down));
					return result;
				}
				case AttackDirection.Up: 
				{
					List<Vector2I> result = CalculateMiddleLine(direction);
					result.AddRange(CalculateDiagonalLine(AttackDirection.Left, AttackDirection.Up));
					result.AddRange(CalculateDiagonalLine(AttackDirection.Right, AttackDirection.Up));
					return result;
				}
				case AttackDirection.Left: 
				{
					List<Vector2I> result = CalculateMiddleLine(direction);
					result.AddRange(CalculateDiagonalLine(AttackDirection.Left, AttackDirection.Up));
					result.AddRange(CalculateDiagonalLine(AttackDirection.Left, AttackDirection.Down));
					return result;
				}
				case AttackDirection.Right: 
				{
					List<Vector2I> result = CalculateMiddleLine(direction);
					result.AddRange(CalculateDiagonalLine(AttackDirection.Right, AttackDirection.Up));
					result.AddRange(CalculateDiagonalLine(AttackDirection.Right, AttackDirection.Down));
					return result;
				}
				default: throw new ArgumentOutOfRangeException();
			}

			List<Vector2I> CalculateDiagonalLine(AttackDirection horizontal, AttackDirection vertical)
			{
				List<Vector2I> middleLine = [];
				var position = startPosition;
				Tile tile;
				do
				{
					position += horizontal.DirectionVector() + vertical.DirectionVector();
					tile = map.GetTileByCartesianCoord(position);
					middleLine.Add(position);
				} while (tile != null && tile.BattleEntity == null);
			
				return middleLine;
			}
		}
		
		List<Vector2I> CalculateCrossbow()
		{
			switch (direction)
			{
				case AttackDirection.Down:
				{
					return CalculateMiddleLine(direction);
				}
				case AttackDirection.Up: 
				{
					return CalculateMiddleLine(direction);
				}
				case AttackDirection.Left: 
				{
					return CalculateMiddleLine(direction);
				}
				case AttackDirection.Right: 
				{
					return CalculateMiddleLine(direction);
				}
				default: throw new ArgumentOutOfRangeException();
			}
		}
		
		List<Vector2I> CalculateMiddleLine(AttackDirection attackDirection)
		{
			List<Vector2I> middleLine = [];
			var position = startPosition;
			Tile tile;
			do
			{
				position += attackDirection.DirectionVector();
				tile = map.GetTileByCartesianCoord(position);
				middleLine.Add(position);
			} while (tile != null && tile.BattleEntity == null);
			
			return middleLine;
		}
    }
}