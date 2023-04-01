using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Engine;
using Microsoft.Xna.Framework.Graphics;

namespace LaPodrida2;

public struct CardData
{
    public enum Suits
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }
    public int Value { get; }
    public int Points { get => Value is 1 ? 20 : Value; }
    public Suits Suit { get; }
    public bool IsGolden { get; }
    public bool IsFaceUp { get; set; }
    public bool Won { get; set; }
    public bool Used { get; set; }
    public CardData(int value, Suits suit, bool isGolden)
    {
        Value = value;
        Suit = suit;
        IsGolden = isGolden;
        IsFaceUp = false;
        Won = false;
        Used = false;
    }

    public KeyValuePair<Texture2D, Animation<Rectangle>> GetTexture()
    {
        if (!IsFaceUp)
            return new KeyValuePair<Texture2D, Animation<Rectangle>>(textures["back"], null);

        if (IsGolden && Won)
            throw new ArgumentException("A card can't be golden and won at the same time.");

        return new KeyValuePair<Texture2D, Animation<Rectangle>>(
            textures[$"{(IsGolden ? "golden_" : "")}{(Won ? "won_": "")}{Suit}"],
            animations[Value - 1]
            );
    }
    public static CardData CreateRandom(bool isGolden)
    {
        var suit = (Suits)Random.Shared.Next(0, 4);
        var value = Random.Shared.Next(1, 14);
        return new CardData(value, suit, isGolden);
    }
    private static Dictionary<string, Texture2D> textures;
    private static List<Animation<Rectangle>> animations;
    public static void LoadTextures(ContentManager content, bool isCasino)
    {
        textures = new Dictionary<string, Texture2D>();
        foreach (var suit in Enum.GetValues<Suits>())
        {
            textures[$"{suit}"] = content.Load<Texture2D>($"Textures/Cards/{suit}");
            textures[$"golden_{suit}"] = content.Load<Texture2D>($"Textures/Cards/golden_{suit}");
            textures[$"won_{suit}"] = content.Load<Texture2D>($"Textures/Cards/won_{suit}");
        }
        if (isCasino)
            textures.Add("back", content.Load<Texture2D>("Textures/Cards/back_red"));
        else
            textures.Add("back", content.Load<Texture2D>("Textures/Cards/back_blue"));

        animations = new List<Animation<Rectangle>>();
        var animation = Animation<Rectangle>.TextureAnimation(new Point(88, 124), new Point(440, 372), false, 1).GetFrames();
        foreach (var frame in animation)
            animations.Add(new Animation<Rectangle>(new[] { frame }, false, 1));
    }
    public static void UnloadTextures(ContentManager content)
    {
        animations.Clear();
        foreach (var texture in textures.Values)
            content.UnloadAsset(texture.Name);
        textures.Clear();
    }
}