using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.EffectManagerLogic;

public partial class ObstacleEffect : Effect
{
	public ObstacleEffect(MapManager InMapManager)
	{
		mapManager = InMapManager;
	}
	public ObstacleEntity obstacleHolder;
	public MapManager mapManager { get; set; }
}

public class FireObstacleEffect : ObstacleEffect
{
	public FireObstacleEffect(MapManager mapManager, int duration = 3) : base(mapManager)
	{
		EffectType = EEffectType.Fire;
		TargetType = EEffectTarget.Obstacle;
		Duration = duration;
		IsTemporary = true;
		IsPending = true;
	}
	public int DamageByEffect = 5;

	// Метод примет эффект в конце хода, также его можно и вызывать, если персонаж пройдёт по пути, на котором есть
	// этот obstacle с таким эффектом
	public override void OnApply()
	{
		if (obstacleHolder.Tile.BattleEntity != null)
		{
			FireEntityEffect entityEffect = new FireEntityEffect(2);
			entityEffect.DamageByEffect = DamageByEffect;
			entityEffect.entityHolder = obstacleHolder.Tile.BattleEntity;
			obstacleHolder.Tile.BattleEntity.appliedEffects.Add(entityEffect);
			GD.Print("Применился огонь на клетке для " + obstacleHolder.Tile.BattleEntity.Id);
		}
		// GD.Print(obstacleHolder.Tile.CartesianPosition);
	}
	public override void OnRemove()
	{
		mapManager.RemoveObstacleFromTile(obstacleHolder.Tile);
	}
}

public class PoisonObstacleEffect : ObstacleEffect
{
	public PoisonObstacleEffect(MapManager mapManager, int duration = 3) : base(mapManager)
	{
		EffectType = EEffectType.Poison;
		TargetType = EEffectTarget.Obstacle;
		Duration = duration;
		IsTemporary = true;
		IsPending = true;
	}
	public int DamageByEffect = 5;
	public override void OnApply()
	{
		if (obstacleHolder.Tile.BattleEntity != null)
		{
			PoisonEntityEffect entityEffect = new PoisonEntityEffect(2);
			entityEffect.DamageByEffect = DamageByEffect;
			entityEffect.entityHolder = obstacleHolder.Tile.BattleEntity;
			obstacleHolder.Tile.BattleEntity.appliedEffects.Add(entityEffect);
			GD.Print("Применилось ядовитое облако на клетке для " + obstacleHolder.Tile.BattleEntity.Id);
		}
		// GD.Print(obstacleHolder.Tile.CartesianPosition);
	}
	public override void OnRemove()
	{
		mapManager.RemoveObstacleFromTile(obstacleHolder.Tile);
	}
}

public class HealObstacleEffect : ObstacleEffect
{
	public HealObstacleEffect(MapManager mapManager, int duration = 3) : base(mapManager)
	{
		EffectType = EEffectType.ObstacleHeal;
		TargetType = EEffectTarget.Obstacle;
		Duration = duration;
		IsTemporary = true;
		IsPending = true;
	}

	public int Heal = 0;
	public override void OnApply()
	{
		// List<Tile> tilesForHealing = new();
		List<Vector2I> coordsForHealing = new();
		Vector2I tileCoord = obstacleHolder.Tile.CartesianPosition;
		coordsForHealing.Add(tileCoord + new Vector2I(0, 1));
		coordsForHealing.Add(tileCoord + new Vector2I(0, -1));
		coordsForHealing.Add(tileCoord + new Vector2I(1, 0));
		coordsForHealing.Add(tileCoord + new Vector2I(-1, 0));

		foreach (Vector2I coordForHealing in coordsForHealing)
		{
			Tile tileForHealing = mapManager.GetTileByCartesianCoord(coordForHealing);
			if (tileForHealing != null && tileForHealing.BattleEntity != null)
			{
				tileForHealing.BattleEntity.ApplyHeal(obstacleHolder, Heal);
				GD.Print("Применилось лечение для " + tileForHealing.BattleEntity.Id);
			}
		}
	}
	public override void OnRemove()
	{
		mapManager.RemoveObstacleFromTile(obstacleHolder.Tile);
	}
}

// Этот эффект нужен лишь для того, чтобы стенка уничтожалась через какое-то время
// Создал отдельный эффект, чтобы не переписывать логику "таймера" для удаления в другом месте
public class WallObstacleEffect : ObstacleEffect
{
	public WallObstacleEffect(MapManager mapManager, int duration = 3) : base(mapManager)
	{
		EffectType = EEffectType.Wall;
		TargetType = EEffectTarget.Obstacle;
		Duration = duration;
		IsTemporary = true;
		IsPending = true;
	}

	public override void OnApply()
	{

	}
	public override void OnRemove()
	{
		mapManager.RemoveObstacleFromTile(obstacleHolder.Tile);
	}
}