using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

public class SimpleImage : DrawableGameComponent
{
    protected Texture2D texture;
    public Vector2 Position { get; set; }
    public Animation<Rectangle> Animation { get; private set; }
    public Color Color { get; set; }
    public float Rotation { get; set; }

    public SimpleImage(Game game, Texture2D texture, Vector2 position, int layer, bool enable, bool visible, Color? color = null, float rotation = 0f, Animation<Rectangle> animation = null, int? updatePriority = null) : base(game)
    {
        this.texture = texture;
        Position = position;
        this.DrawOrder = layer;
        this.Enabled = enable;
        this.Visible = visible;
        this.Color = color ?? Color.White;
        Rotation = rotation;
        Animation = animation;
        this.UpdateOrder = updatePriority ?? layer;
    }

    public override void Draw(GameTime gameTime)
    {
        var spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        spriteBatch.Draw(
            texture,
            Position,
            Animation?.NextFrame(),
            this.Color,
            Rotation,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            DrawOrder * 0.1f
        );
    }

    public void ChangeTexture(Texture2D texture) {
        Animation = null;
        this.texture = texture;
    }

    public void ChangeAnimatedTexture(Texture2D texture, Animation<Rectangle> animation)
    {
        this.texture = texture;
        Animation = animation;
    }
}