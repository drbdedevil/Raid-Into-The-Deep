using System.Linq;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public static class ActiveSkillFactory
{
    public static ActiveSkill CreateActiveSkillByName(string activeSkillName)
    {
        switch (activeSkillName)
        {
            case "Обратная волна": return new ReserveDamageActiveSkill(EEffectType.ReserveDamage);
            case "Тяжёлая рана": return new SevereWoundActiveSkill(EEffectType.SevereWound);
            case "Ядовитое облако": return null;
            case "Тихая медитация": return new SilentMeditationsActiveSkill(EEffectType.NONE);
            case "Железная стена": return null;
            case "Боевой раж": return new BattleFrenzyActiveSkill(EEffectType.BattleFrenzy);
            case "Кровавая метка": return new BloodMarkActiveSkill(EEffectType.BloodMark);
            case "Снять эффекты": return new RemoveEffectsActiveSkill(EEffectType.NONE);
            case "Защита": return new DefenseActiveSkill(EEffectType.Defense);
            case "Скачок": return null;
            case "Поджог": return null;
            case "Тотем восстановления": return null;
            default: return null;
        }
    }
}