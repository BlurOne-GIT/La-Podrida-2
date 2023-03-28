using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

public class Button : DrawableGameComponent
{
    #region Events
    public event EventHandler LeftClicked;
    public event EventHandler MiddleClicked;
    public event EventHandler RightClicked;
    public event EventHandler XButton1Clicked;
    public event EventHandler XButton2Clicked;
    #endregion

    #region Properties
    public SimpleImage Image { get; private set; }
    public HoverDetector HoverDetector { get; private set; }
    public Rectangle ActionBox { get; private set; }
    #endregion

    //Constructor
    public Button(Game game, Rectangle actionBox, SimpleImage texture = null, Alignment anchor = Alignment.Center, bool enabled = true, bool hasHover = false) : base(game)
    {
        actionBox.Location = anchor switch
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

        if (hasHover)
            HoverDetector = new HoverDetector(game, actionBox, Alignment.TopLeft);
        
        ActionBox = new Rectangle((int)(actionBox.X * LaPodrida2.Configs.PartialScale), (int)(actionBox.Y * LaPodrida2.Configs.PartialScale), (int)(actionBox.Width * LaPodrida2.Configs.PartialScale), (int)(actionBox.Height * LaPodrida2.Configs.PartialScale));
        Image = texture;
        Enabled = enabled;
        Visible = texture is not null;
        if (texture is not null)
            DrawOrder = texture.DrawOrder;

        //ResetRectangle(null, null);
        //Configs.ResolutionChanged += ResetRectangle;
        Input.ButtonDown += Check;
        
    }

    #region Methods

    public override void Draw(GameTime gameTime) 
    {
        if (Image.Visible)
            Image.Draw(gameTime);
    }

    private void Check(object s, ButtonEventArgs e)
    {
        if (!Enabled)
            return;

        if (ActionBox.Contains(e.Position))
        {
            switch (e.Button)
            {
                case "LeftButton":
                    LeftClicked?.Invoke(this, new EventArgs());
                    break;
                case "MiddleButton":
                    MiddleClicked?.Invoke(this, new EventArgs());
                    break;
                case "RightButton":
                    RightClicked?.Invoke(this, new EventArgs());
                    break;
                case "XButton1":
                    XButton1Clicked?.Invoke(this, new EventArgs());
                    break;
                case "XButton2":
                    XButton2Clicked?.Invoke(this, new EventArgs());
                    break;
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (HoverDetector is not null && HoverDetector.Enabled)
            HoverDetector.Update(gameTime);
        base.Update(gameTime);
    }

    //private void ResetRectangle(object s, EventArgs e) => rectangle = new Rectangle((int)_position.X * Configs.Scale + Configs.XOffset, (int)_position.Y * Configs.Scale + Configs.YOffset, _size.X * Configs.Scale, _size.Y * Configs.Scale);

    public new void Dispose()
    {
        //Configs.ResolutionChanged -= ResetRectangle;
        Input.ButtonDown -= Check;
        base.Dispose();
    }
    #endregion
}