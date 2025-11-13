using Godot;
using RaidIntoTheDeep.Levels.Fight;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using System;

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

	// Метод примет эффект в конце хода, также его можно и вызывать, если персонаж пройдёт по пути, на котором есть
	// этот obstacle с таким эффектом
	public override void OnApply()
	{
		if (obstacleHolder.Tile.BattleEntity != null)
		{
			EntityEffect entityEffect = new FireEntityEffect(2);
			entityEffect.entityHolder = obstacleHolder.Tile.BattleEntity;
			obstacleHolder.Tile.BattleEntity.appliedEffects.Add(entityEffect);
			GD.Print("Применился огонь на клетке для " + obstacleHolder.Tile.BattleEntity.Id);
		}
		GD.Print(obstacleHolder.Tile.CartesianPosition);
	}
	public override void OnRemove()
	{
		mapManager.RemoveObstacleFromTile(obstacleHolder.Tile);
	}
}
