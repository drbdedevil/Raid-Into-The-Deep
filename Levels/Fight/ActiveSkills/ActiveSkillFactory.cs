using System.Linq;
using Godot;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public static class ActiveSkillFactory
{
    public static ActiveSkill CreateActiveSkillByName(string activeSkillName)
    {
        switch (activeSkillName)
        {
            case "Обратная волна": return new ReserveDamageActiveSkill(EEffectType.ReserveDamage);
            case "Тяжёлая рана": return new SevereWoundActiveSkill(EEffectType.SevereWound);
            case "Ядовитое облако": return new PoisonCloudActiveSkill(EEffectType.Poison);
            case "Тихая медитация": return new SilentMeditationsActiveSkill(EEffectType.NONE);
            case "Железная стена": return new IronWallActiveSkill(EEffectType.Wall);
            case "Боевой раж": return new BattleFrenzyActiveSkill(EEffectType.BattleFrenzy);
            case "Кровавая метка": return new BloodMarkActiveSkill(EEffectType.BloodMark);
            case "Снять эффекты": return new RemoveEffectsActiveSkill(EEffectType.NONE);
            case "Защита": return new DefenseActiveSkill(EEffectType.Defense);
            case "Скачок": return new LeapActiveSkill(EEffectType.NONE);
            case "Поджог": return new ArsonActiveSkill(EEffectType.Fire);
            case "Тотем восстановления": return new RestorationTotemActiveSkill(EEffectType.ObstacleHeal);
            default:
                GD.PrintErr("НЕТ ТАКОГО ИМЕНИ НАВЫКА");
                return null;
        }
    }
}