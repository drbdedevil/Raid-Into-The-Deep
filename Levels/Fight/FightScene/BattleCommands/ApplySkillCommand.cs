using System.Collections.Generic;
using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;

namespace RaidIntoTheDeep.Levels.Fight.FightScene.BattleCommands;

public class ApplySkillCommand : Command
{
	private BattleEntity _battleEntity { get; set; }
	private List<Tile> _tilesForAttack { get; set; }
	private MapManager _mapManager { get; set; }

	public ApplySkillCommand(BattleEntity battleEntity, List<Tile> tilesForAttack, MapManager mapManager)
	{
		_battleEntity = battleEntity;
		_tilesForAttack = tilesForAttack;
		_mapManager = mapManager;
	}
	
	// наворотил говна из веток
	public override void Execute()
	{
		if (_battleEntity is PlayerEntity playerEntityActive)
		{
			if (playerEntityActive.activeSkill != null)
			{
				GD.Print($"чувачок с Id-{_battleEntity.Id} применил скилл");
				
				if (playerEntityActive.activeSkill.IsHasEffect())
				{
					Effect generatedEffect = playerEntityActive.activeSkill.GenerateEffect();
					if (generatedEffect.TargetType == EEffectTarget.Self)
					{
						if (generatedEffect is EntityEffect entityEffect)
						{
							entityEffect.entityHolder = playerEntityActive;
							playerEntityActive.AddEffect(generatedEffect);
							return;
						}
					}
				}
			} 
		}

		foreach (var tile in _tilesForAttack)
		{
			if (_battleEntity is PlayerEntity playerEntity)
			{
				if (playerEntity.activeSkill == null) continue;

				if (playerEntity.activeSkill.IsHasEffect())
				{
					Effect generatedEffect = playerEntity.activeSkill.GenerateEffect();
					if (tile.BattleEntity != null && generatedEffect.TargetType == EEffectTarget.Enemy)
					{
						if (generatedEffect is EntityEffect entityEffect)
						{
							if (generatedEffect is SevereWoundEntityEffect severeWoundEntityEffect)
							{
								severeWoundEntityEffect.DamageFromInstigator = playerEntity.Damage / 4;
							}
							else if (generatedEffect is BloodMarkEntityEffect bloodMarkEntityEffect)
							{
								bloodMarkEntityEffect.DamageFromInstigator = playerEntity.DamageByEffect * 2;
							}
							entityEffect.entityHolder = tile.BattleEntity;
							tile.BattleEntity.AddEffect(generatedEffect);
							GD.Print($"чувачок с Id-{_battleEntity.Id} применил скилл по тайлу {tile}");
						}
					}
					else if (generatedEffect.TargetType == EEffectTarget.Obstacle)
					{
						if (generatedEffect is ObstacleEffect obstacleEffect && (tile.ObstacleEntity == null ||
								tile.ObstacleEntity != null && tile.ObstacleEntity.IsImpassable == false))
						{
							obstacleEffect.mapManager = _mapManager;
							if (obstacleEffect.EffectType == EEffectType.Fire)
							{
								_mapManager.SetObstacleOnTile(tile, new ObstacleEntity(tile, ObstacleCode.Fire));
							}
							else if (obstacleEffect.EffectType == EEffectType.Poison)
							{
								_mapManager.SetObstacleOnTile(tile, new ObstacleEntity(tile, ObstacleCode.Poison));
							}
							else if (obstacleEffect.EffectType == EEffectType.ObstacleHeal)
							{
								_mapManager.SetObstacleOnTile(tile, new ObstacleEntity(tile, ObstacleCode.Totem - 3));
								if (obstacleEffect is HealObstacleEffect healObstacleEffect)
								{
									healObstacleEffect.Heal = playerEntity.Heal / 2;
								}
							}
							else if (obstacleEffect.EffectType == EEffectType.Wall && tile.BattleEntity == null)
							{
								_mapManager.SetObstacleOnTile(tile, new ObstacleEntity(tile, ObstacleCode.Wall));
							}

							if (tile.ObstacleEntity != null)
							{
								obstacleEffect.obstacleHolder = tile.ObstacleEntity;
								tile.ObstacleEntity.AddEffect(obstacleEffect);
								obstacleEffect.OnApply();
							}
						}
					}
				}
				else
				{
					if (tile.BattleEntity == null)
					{
						if (playerEntity.activeSkill is LeapActiveSkill leapActiveSkill)
						{
							if (tile.ObstacleEntity != null && tile.ObstacleEntity.IsImpassable == false)
							{
								_mapManager.SetBattleEntityOnTile(tile, playerEntity);
							}
							else if (tile.ObstacleEntity == null)
							{
								_mapManager.SetBattleEntityOnTile(tile, playerEntity);
							}
						}

						continue;
					}

					if (playerEntity.activeSkill is SilentMeditationsActiveSkill silentMeditationsActiveSkill)
					{
						tile.BattleEntity.ApplyHeal(playerEntity, tile.BattleEntity.Heal);
					}
					else if (playerEntity.activeSkill is RemoveEffectsActiveSkill removeEffectsActiveSkill)
					{
						List<Effect> effectsToDelete = new List<Effect>();
						foreach (Effect effect in tile.BattleEntity.appliedEffects)
						{
							if (EffectExtensions.IsNegative(effect.EffectType))
							{
								effectsToDelete.Add(effect);
							}
						}
						foreach (Effect effect in effectsToDelete)
						{
							tile.BattleEntity.RemoveEffect(effect);
						}
					}
				}
			}
		}
	}

	public override void UnExecute()
	{
		foreach (var tile in _tilesForAttack)
		{
			GD.Print($"чувачок с Id-{_battleEntity.Id} применил скилл по тайлу {tile}");
		}
	}
}
