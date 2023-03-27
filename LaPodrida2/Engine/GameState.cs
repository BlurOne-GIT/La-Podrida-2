using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine;

public abstract class GameState : DrawableGameComponent
{
    public GameState(Game game) : base(game)
    {
        Input.KeyDown += HandleInput;
        Input.ButtonDown += HandleInput;
    }
    public new virtual void Dispose()
    {
        Input.KeyDown -= HandleInput;
        Input.ButtonDown -= HandleInput;
        foreach (GameComponent gameObject in _components)
            gameObject.Dispose();

        base.Dispose();
    }
    protected readonly GameComponentCollection _components = new GameComponentCollection();
    public virtual void HandleInput(object s, ButtonEventArgs e) {}
    public virtual void HandleInput(object s, InputKeyEventArgs e) {}
    public event EventHandler<GameState> OnStateSwitched;
    public new abstract void LoadContent();
    public new abstract void UnloadContent();
    protected void SwitchState(GameState gameState)
    {
        OnStateSwitched?.Invoke(this, gameState);
    }
    public override void Update(GameTime gameTime)
    {
        foreach (GameComponent gameObject in _components.OrderBy(a => (a as GameComponent).UpdateOrder))
            if (gameObject.Enabled) gameObject.Update(gameTime);
    }
    public override void Draw(GameTime gameTime)
    {
        var c = _components.Where(a => a is DrawableGameComponent && (a as DrawableGameComponent).Visible).OrderBy(a => (a as DrawableGameComponent).DrawOrder);
        foreach (DrawableGameComponent gameObject in c)
            gameObject.Draw(gameTime);
    }
 }