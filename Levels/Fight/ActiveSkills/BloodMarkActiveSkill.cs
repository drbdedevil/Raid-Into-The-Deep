using System.Collections.Generic;
using Godot;
using System.Linq;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using RaidIntoTheDeep.Levels.Fight;

public class BloodMarkActiveSkill : ActiveSkill
{
	public BloodMarkActiveSkill(EEffectType InEffectType, PlayerEntity playerEntity, ESkillType SkillType) : base(InEffectType, playerEntity)
	{
		// CreateEffect(InEffectType);
		skillType = SkillType;
	}

	public override List<Vector2I> CalculateShapeAttackPositions(Vector2I startPosition, Vector2I playerTargetPosition, MapManager map)
    {
		List<Vector2I> result = new();
		List<Tile> tiles = PathFinder.FindTilesToChoose(map.GetTileByCartesianCoord(startPosition), map, 3);

		Tile tile = tiles.FirstOrDefault(tile => tile.CartesianPosition == playerTargetPosition);
		if (tile != null)
        {
			result.Add(tile.CartesianPosition);
        }

		return result;
    }

	public override void PlaySkillSound()
    {
        SoundManager.Instance.PlaySoundOnce("res://Sound/Skills/BloodMark.wav", 0.2f);
    }

	// public abstract List<TargetWeaponAttackDamage> CalculateDamageForEntities(BattleEntity attacker, List<Tile> attackedTiles);
}