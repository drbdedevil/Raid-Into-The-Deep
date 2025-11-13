using System.Collections.Generic;
using System.Linq;
using Godot;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;


public class LeapActiveSkill : ActiveSkill
{
	public LeapActiveSkill(EEffectType InEffectType, PlayerEntity playerEntity) : base(InEffectType, playerEntity)
	{
		// CreateEffect(InEffectType);
	}

	public override List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map)
	{
		List<Vector2I> result = new();
		List<Tile> tiles = PathFinder.FindTilesToMove(map.GetTileByCartesianCoord(startPosition), map, playerEntityOwner.Speed);

		Tile tile = tiles.FirstOrDefault(tile => tile.CartesianPosition == playerTargetPosition);
		if (tile != null)
        {
			result.Add(tile.CartesianPosition);
        }

		return result;
	}

	// public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}
