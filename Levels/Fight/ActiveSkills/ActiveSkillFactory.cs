using System.Linq;
using Godot;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public static class ActiveSkillFactory
{
    public static ActiveSkill CreateActiveSkillByName(string activeSkillName, PlayerEntity playerEntity)
    {
        switch (activeSkillName)
        {
            case "Обратная волна": return new ReserveDamageActiveSkill(EEffectType.ReserveDamage, playerEntity);
            case "Тяжёлая рана": return new SevereWoundActiveSkill(EEffectType.SevereWound, playerEntity);
            case "Ядовитое облако": return new PoisonCloudActiveSkill(EEffectType.Poison, playerEntity);
            case "Тихая медитация": return new SilentMeditationsActiveSkill(EEffectType.NONE, playerEntity);
            case "Железная стена": return new IronWallActiveSkill(EEffectType.Wall, playerEntity);
            case "Боевой раж": return new BattleFrenzyActiveSkill(EEffectType.BattleFrenzy, playerEntity);
            case "Кровавая метка": return new BloodMarkActiveSkill(EEffectType.BloodMark, playerEntity);
            case "Снять эффекты": return new RemoveEffectsActiveSkill(EEffectType.NONE, playerEntity);
            case "Защита": return new DefenseActiveSkill(EEffectType.Defense, playerEntity);
            case "Скачок": return new LeapActiveSkill(EEffectType.NONE, playerEntity);
            case "Поджог": return new ArsonActiveSkill(EEffectType.Fire, playerEntity);
            case "Тотем восстановления": return new RestorationTotemActiveSkill(EEffectType.ObstacleHeal, playerEntity);
            case "NONE": return null;
            default:
                GD.PrintErr("НЕТ ТАКОГО ИМЕНИ НАВЫКА");
                return null;
        }
    }
}