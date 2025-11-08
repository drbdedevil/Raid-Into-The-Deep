using System;
using Godot;

public enum AttackDirection
{
    Up,
    Down,
    Left,
    Right
}

static class AttackDirectionExtensions
{
    public static Vector2I DirectionVector(this AttackDirection direction)
    {
        switch (direction)
        {
            case AttackDirection.Right: return Vector2I.Right;
            case AttackDirection.Down: return Vector2I.Down;
            case AttackDirection.Up : return Vector2I.Up;
            case AttackDirection.Left: return Vector2I.Left;
            default: throw new ApplicationException();
        }
    }
}