using System.Linq;

namespace RaidIntoTheDeep.Levels.Fight.Weapons;

public static class ActiveSkillFactory
{
    public static ActiveSkill CreateActiveSkillByName(string activeSkillName)
    {
        switch (activeSkillName)
        {
            case "Обратная волна": return new ReserveDamageActiveSkill(EEffectType.ReserveDamage);
            case "Тяжёлая рана": return null;
            case "Ядовитое облако": return null;
            case "Тихая медитация": return null;
            case "Железная стена": return null;
            case "Боевой раж": return new BattleFrenzyActiveSkill(EEffectType.BattleFrenzy);
            case "Кровавая метка": return null;
            case "Снять эффекты": return null;
            case "Защита": return null;
            case "Скачок": return null;
            case "Поджог": return null;
            case "Тотем восстановления": return null;
            default: return null;
        }
    }
}