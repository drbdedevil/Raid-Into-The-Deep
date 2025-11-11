using Godot;
using RaidIntoTheDeep.Levels.Fight.FightScene.Scripts;
using RaidIntoTheDeep.Levels.Fight.Weapons;

namespace RaidIntoTheDeep.Levels.Fight;

public partial class EnemyEntity : BattleEntity
{
    // надо будет написать метод для спавна рандомного оружия врагам
    public EnemyEntity(Tile tile, string id, int speed, int health, int damage, GameEnemyCode enemyId)
        : base(tile, WeaponFactory.CreateWeaponByAttackShapeType(AttackShapeType.ScatterShot, new WeaponData()), id, speed, health, damage)
    {
        EnemyId = enemyId;
        MaxHealth = health;

        InitFieldsByEnemyId();
    }
    
    public int MaxHealth { get; set; }
    public Texture2D EnemyTexture { get; private set; }
    public string EnemyName { get; private set; }
    public int EnemyLevel { get; private set; }

    public GameEnemyCode EnemyId { get; set; }

    private void InitFieldsByEnemyId()
    {
        switch (EnemyId)
        {
            case GameEnemyCode.Spider:
                EnemyTexture = GD.Load<Texture2D>("res://Textures/Fight/FightScene/Spider.png");
                EnemyName = "Паук шушера";
                break;
            case GameEnemyCode.Bug:
                EnemyTexture = GD.Load<Texture2D>("res://Textures/Fight/FightScene/Bug.png");
                EnemyName = "Махонькая букашка";
                break;
            case GameEnemyCode.Centipede:
                EnemyTexture = GD.Load<Texture2D>("res://Textures/Fight/FightScene/Centipede.png");
                EnemyName = "Отвратная сороконожка";
                break;
            case GameEnemyCode.EvilEye:
                EnemyTexture = GD.Load<Texture2D>("res://Textures/Fight/FightScene/EvilEye.png");
                EnemyName = "Злой глаз";
                break;
            case GameEnemyCode.Root:
                EnemyTexture = GD.Load<Texture2D>("res://Textures/Fight/FightScene/Root.png");
                EnemyName = "Грязный корень";
                break;
            case GameEnemyCode.SpiderBoss:
                EnemyTexture = GD.Load<Texture2D>("res://Textures/Fight/FightScene/SpiderBoss.png");
                EnemyName = "Паук, пожиратель племён";
                break;
            case GameEnemyCode.Vegetable:
                EnemyTexture = GD.Load<Texture2D>("res://Textures/Fight/FightScene/Tank.png");
                EnemyName = "Прогрызатель туннелей";
                break;
            case GameEnemyCode.Wasp:
                EnemyTexture = GD.Load<Texture2D>("res://Textures/Fight/FightScene/Wasp.png");
                EnemyName = "Вредная оса";
                break;
            case GameEnemyCode.Tank:
                EnemyTexture = GD.Load<Texture2D>("res://Textures/Fight/FightScene/Vegetable.png"); 
                EnemyName = "Гигантская жуколовка";
                break;
            default:
                break;
        }
    }
}