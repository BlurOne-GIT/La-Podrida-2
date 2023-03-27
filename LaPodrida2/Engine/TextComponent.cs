using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

public class TextComponent : DrawableGameComponent
{
    #region Fields
    private SpriteFont font;
    private Alignment anchor;
    private string _text;
    private float _rotation;
    private Vector2 pivot;
    #endregion

    #region Properties
    public Vector2 Position { get; set; }
    public Color Color { get; set; }
    public float Opacity { get; set; }
    public float Rotation { get => MathHelper.ToDegrees(_rotation); set => _rotation = MathHelper.ToRadians(_rotation); }
    public float Scale { get; set; }
    public string Text { get => _text; set {_text = value; RelocatePivot();} }

    #endregion

    public TextComponent(Game game, SpriteFont font, string defaultText, Vector2 position, int layer, bool visible = true, Alignment anchor = Alignment.CenterLeft, Color? color = null, float opacity = 1f, float rotation = 0f, float scale = 1f) : base(game)
    {
        this.font = font;
        Position = position;
        DrawOrder = layer;
        Visible = visible;
        this.anchor = anchor;
        this.Color = color ?? Color.White;
        Opacity = opacity;
        Rotation = rotation;
        Scale = scale;
        Text = defaultText;
    }

    private void RelocatePivot()
    {
        pivot = anchor switch
        {
            Alignment.TopLeft => Vector2.Zero,
            Alignment.TopCenter => new Vector2(font.MeasureString(_text).X / 2, 0f),
            Alignment.TopRight => new Vector2(font.MeasureString(_text).X, 0f),
            Alignment.CenterLeft => new Vector2(0f, font.MeasureString(_text).Y / 2),
            Alignment.Center => new Vector2(font.MeasureString(_text).X / 2, font.MeasureString(_text).Y / 2),
            Alignment.CenterRight => new Vector2(font.MeasureString(_text).X, font.MeasureString(_text).Y / 2),
            Alignment.BottomLeft => new Vector2(0f, font.MeasureString(_text).Y),
            Alignment.BottomCenter => new Vector2(font.MeasureString(_text).X / 2, font.MeasureString(_text).Y),
            Alignment.BottomRight => new Vector2(font.MeasureString(_text).X, font.MeasureString(_text).Y),
            _ => Vector2.Zero
        };
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch _spriteBatch = Game.Services.GetService<SpriteBatch>();
        _spriteBatch.DrawString
        (
            font,
            Text,
            Position,
            Color * Opacity,
            _rotation,
            pivot,
            Scale,
            SpriteEffects.None,
            DrawOrder * 0.1f
        );
    }
}