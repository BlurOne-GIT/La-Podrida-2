using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

public class SimpleImage : DrawableGameComponent
{
    #region Fields
    protected Texture2D texture;
    private Alignment anchor;
    private float _rotation;
    private Vector2 pivot;
    #endregion

    #region Properties
    public Vector2 Position { get; set; }
    public Animation<Rectangle> Animation { get; private set; }
    public Color Color { get; set; }
    public float Opacity { get; set; } = 1f;
    public float Rotation { get => MathHelper.ToDegrees(_rotation); set => _rotation = MathHelper.ToRadians(value); }
    public float Scale { get; set; }
    #endregion

    public SimpleImage(Game game, Texture2D texture, Vector2 position, int layer, bool visible = true, Alignment anchor = Alignment.Center, Animation<Rectangle> animation = null, Color? color = null, float opacity = 1f, float rotation = 0f, float scale = 1f) : base(game)
    {
        this.texture = texture;
        Position = position;
        this.DrawOrder = layer;
        this.Visible = visible;
        this.anchor = anchor;
        Animation = animation;
        this.Color = color ?? Color.White;
        Opacity = opacity;
        Rotation = rotation;
        Scale = scale;
        RelocatePivot();
    }

    private void RelocatePivot()
    {   
        if (Animation is null)
            pivot = anchor switch
            {
                Alignment.TopLeft => Vector2.Zero,
                Alignment.TopCenter => new Vector2(texture.Width / 2, 0f),
                Alignment.TopRight => new Vector2(texture.Width, 0f),
                Alignment.CenterLeft => new Vector2(0f, texture.Height / 2),
                Alignment.Center => new Vector2(texture.Width / 2, texture.Height / 2),
                Alignment.CenterRight => new Vector2(texture.Width, texture.Height / 2),
                Alignment.BottomLeft => new Vector2(0f, texture.Height),
                Alignment.BottomCenter => new Vector2(texture.Width / 2, texture.Height),
                Alignment.BottomRight => new Vector2(texture.Width, texture.Height),
                _ => Vector2.Zero
            };
        else
            pivot = anchor switch
            {
                Alignment.TopLeft => Vector2.Zero,
                Alignment.TopCenter => new Vector2(Animation.CurrentFrame().Width / 2, 0f),
                Alignment.TopRight => new Vector2(Animation.CurrentFrame().Width, 0f),
                Alignment.CenterLeft => new Vector2(0f, Animation.CurrentFrame().Height / 2),
                Alignment.Center => new Vector2(Animation.CurrentFrame().Width / 2, Animation.CurrentFrame().Height / 2),
                Alignment.CenterRight => new Vector2(Animation.CurrentFrame().Width, Animation.CurrentFrame().Height / 2),
                Alignment.BottomLeft => new Vector2(0f, Animation.CurrentFrame().Height),
                Alignment.BottomCenter => new Vector2(Animation.CurrentFrame().Width / 2, Animation.CurrentFrame().Height),
                Alignment.BottomRight => new Vector2(Animation.CurrentFrame().Width, Animation.CurrentFrame().Height),
                _ => Vector2.Zero
            };
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch _spriteBatch = Game.Services.GetService<SpriteBatch>();
        _spriteBatch.Draw(
            texture,
            Position * LaPodrida2.Configs.PartialScale,
            Animation is not null ? Animation.NextFrame() : null,
            Color * Opacity,
            _rotation,
            pivot,
            Scale * LaPodrida2.Configs.PartialScale,
            SpriteEffects.None,
            DrawOrder * 0.1f
        );
    }

    public void ChangeTexture(Texture2D texture) {
        Animation = null;
        this.texture = texture;
        RelocatePivot();
    }

    public void ChangeAnimatedTexture(Texture2D texture, Animation<Rectangle> animation)
    {
        this.texture = texture;
        Animation = animation ?? Animation;
        RelocatePivot();
    }

    public void ChangeAnimation(Animation<Rectangle> animation)
    {
        Animation = animation;
        RelocatePivot();
    }
}

public enum Alignment
{
    TopLeft,
    TopCenter,
    TopRight,
    CenterLeft,
    Center,
    CenterRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}