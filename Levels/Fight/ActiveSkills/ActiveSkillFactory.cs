using System.Linq;
using Godot;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public static class ActiveSkillFactory
{
    public static ActiveSkill CreateActiveSkillByName(string activeSkillName, PlayerEntity playerEntity)
    {
        switch (activeSkillName)
        {
            case "Обратная волна": return new ReserveDamageActiveSkill(EEffectType.ReserveDamage, playerEntity, ESkillType.ReverseWave);
            case "Тяжёлая рана": return new SevereWoundActiveSkill(EEffectType.SevereWound, playerEntity, ESkillType.SevereWound);
            case "Ядовитое облако": return new PoisonCloudActiveSkill(EEffectType.Poison, playerEntity, ESkillType.PoisonCloud);
            case "Тихая медитация": return new SilentMeditationsActiveSkill(EEffectType.NONE, playerEntity, ESkillType.SilentMeditations);
            case "Железная стена": return new IronWallActiveSkill(EEffectType.Wall, playerEntity, ESkillType.IronWall);
            case "Боевой раж": return new BattleFrenzyActiveSkill(EEffectType.BattleFrenzy, playerEntity, ESkillType.BattleFrenzy);
            case "Кровавая метка": return new BloodMarkActiveSkill(EEffectType.BloodMark, playerEntity, ESkillType.BloodMark);
            case "Снять эффекты": return new RemoveEffectsActiveSkill(EEffectType.NONE, playerEntity, ESkillType.RemoveEffects);
            case "Защита": return new DefenseActiveSkill(EEffectType.Defense, playerEntity, ESkillType.Defense);
            case "Скачок": return new LeapActiveSkill(EEffectType.NONE, playerEntity, ESkillType.Leap);
            case "Поджог": return new ArsonActiveSkill(EEffectType.Fire, playerEntity, ESkillType.Arson);
            case "Тотем восстановления": return new RestorationTotemActiveSkill(EEffectType.ObstacleHeal, playerEntity, ESkillType.RestorationTotem);
            case "NONE": return null;
            default:
                GD.PrintErr("НЕТ ТАКОГО ИМЕНИ НАВЫКА");
                return null;
        }
    }
}