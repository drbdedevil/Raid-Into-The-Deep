using Godot;
using System;

[GlobalClass]
public partial class CharactersSprites : Resource
{
    [Export] public Godot.Collections.Array<Texture2D> CharactersSpritesArray { get; set; } = new();
}
