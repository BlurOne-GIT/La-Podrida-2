using System;
using Microsoft.Xna.Framework;

namespace Engine;

public class HoverDetector : GameComponent
{
    public event EventHandler Hovered;
    public event EventHandler Unhovered;
    public Rectangle ActionBox { get; private set; }
    public bool Hovering { get; private set; }

    public HoverDetector(Game game, Rectangle actionBox, Alignment alignment = Alignment.Center, bool enabled = true) : base(game)
    {
        actionBox.Location = alignment switch
        {
            Alignment.TopLeft => actionBox.Location,
            Alignment.TopCenter => actionBox.Location - new Point(actionBox.Width / 2, 0),
            Alignment.TopRight => actionBox.Location - new Point(actionBox.Width, 0),
            Alignment.CenterLeft => actionBox.Location - new Point(0, actionBox.Height / 2),
            Alignment.Center => actionBox.Location - new Point(actionBox.Width / 2, actionBox.Height / 2),
            Alignment.CenterRight => actionBox.Location - new Point(actionBox.Width, actionBox.Height / 2),
            Alignment.BottomLeft => actionBox.Location - new Point(0, actionBox.Height),
            Alignment.BottomCenter => actionBox.Location - new Point(actionBox.Width / 2, actionBox.Height),
            Alignment.BottomRight => actionBox.Location - new Point(actionBox.Width, actionBox.Height),
            _ => actionBox.Location
        };

        ActionBox = actionBox;
        Enabled = enabled;
    }

    public override void Update(GameTime gameTime)
    {
        if (ActionBox.Contains(Input.MousePoint))
        {
            if (!Hovering)
            {
                Hovering = true;
                Hovered?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            if (Hovering)
            {
                Hovering = false;
                Unhovered?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}