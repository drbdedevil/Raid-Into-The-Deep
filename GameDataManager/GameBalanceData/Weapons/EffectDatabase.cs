using Godot;
using System;


[GlobalClass]
public partial class EffectDatabase : Resource
{
    /// <summary>
    /// Пример распределения весов:
    /// - Обычные эффекты: 1.0
    /// - Редкие эффекты: 0.3
    /// - Очень редкие: 0.1
    /// - Легендарные: 0.01
    /// </summary>
    [Export(PropertyHint.None, "Пример распределения весов:\n- Обычные эффекты: 1.0\n- Редкие эффекты: 0.3\n- Очень редкие: 0.1\n- Легендарные: 0.01")] 
    public Godot.Collections.Array<EffectInfo> Effects { get; set; } = new();
}
