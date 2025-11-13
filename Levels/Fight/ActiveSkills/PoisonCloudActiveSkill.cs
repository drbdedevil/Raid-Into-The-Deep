using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public class PoisonCloudActiveSkill : ActiveSkill
{
	public PoisonCloudActiveSkill(EEffectType InEffectType, PlayerEntity playerEntity) : base(InEffectType, playerEntity)
	{
		// CreateEffect(InEffectType);
	}

	public override List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map)
	{
		var direction = CalculateDirection(startPosition, playerTargetPosition);
		switch (direction)
        {
            case AttackDirection.Up: 
                return CalculateTargetPositions(startPosition - new Vector2I(0, startPosition.Y - playerTargetPosition.Y));
            case AttackDirection.Down:
                return CalculateTargetPositions(startPosition + new Vector2I(0, playerTargetPosition.Y - startPosition.Y) );
            case AttackDirection.Left:
                return CalculateTargetPositions(startPosition - new Vector2I(startPosition.X - playerTargetPosition.X, 0));
            case AttackDirection.Right:
                return CalculateTargetPositions(startPosition + new Vector2I(playerTargetPosition.X - startPosition.X, 0));
            default: return [];
        }

        List<Vector2I> CalculateTargetPositions(Vector2I artilleryTargetPosition)
        {
            var tile = map.GetTileByCartesianCoord(artilleryTargetPosition);
            if (tile != null && tile.BattleEntity is not PlayerEntity)
            {
                return
                [
                    artilleryTargetPosition, artilleryTargetPosition + Vector2I.Up,
                    artilleryTargetPosition + Vector2I.Right + Vector2I.Up, artilleryTargetPosition + Vector2I.Right
                ];
            }

            return [];
        }
	}

	// public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}
