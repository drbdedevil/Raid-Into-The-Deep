using System.Collections.Generic;
using Godot;
using System;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;


public class ArsonActiveSkill : ActiveSkill
{
	public ArsonActiveSkill(EEffectType InEffectType, PlayerEntity playerEntity) : base(InEffectType, playerEntity)
	{
		// CreateEffect(InEffectType);
	}

	public override List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map)
	{
		var direction = CalculateDirection(startPosition, playerTargetPosition);

		switch (direction)
		{
			case AttackDirection.Down: return [startPosition + Vector2I.Down + Vector2I.Left, startPosition + Vector2I.Down, startPosition + Vector2I.Down + Vector2I.Right];
			case AttackDirection.Up: return [startPosition + Vector2I.Up + Vector2I.Left, startPosition + Vector2I.Up, startPosition + Vector2I.Up + Vector2I.Right];
			case AttackDirection.Left: return [startPosition + Vector2I.Left + Vector2I.Up,startPosition +   Vector2I.Left,startPosition +  Vector2I.Left + Vector2I.Down];
			case AttackDirection.Right: return [startPosition + Vector2I.Right + Vector2I.Up,  startPosition + Vector2I.Right, startPosition + Vector2I.Right + Vector2I.Down];
			default: throw new ArgumentOutOfRangeException();
		}
	}

	// public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}
