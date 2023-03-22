using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine;

public abstract class GameState : IDisposable
{
    public GameState()
    {
        Input.KeyDown += HandleInput;
        Input.ButtonDown += HandleInput;
    }
    public virtual void Dispose()
    {
        Input.KeyDown -= HandleInput;
        Input.ButtonDown -= HandleInput;
        foreach (GameComponent gameObject in _components)
            gameObject.Dispose();
    }
    protected readonly GameComponentCollection _components = new GameComponentCollection();
    public abstract void LoadContent(ContentManager Content);
    public abstract void UnloadContent(ContentManager Content);
    public abstract void HandleInput(object s, ButtonEventArgs e);
    public abstract void HandleInput(object s, InputKeyEventArgs e);
    public event EventHandler<GameState> OnStateSwitched;
    protected void SwitchState(GameState gameState)
    {
        OnStateSwitched?.Invoke(this, gameState);
    }
    public virtual void Update(GameTime gameTime)
    {
        foreach (GameComponent gameObject in _components.OrderBy(a => (a as GameComponent).UpdateOrder))
            if (gameObject.Enabled) gameObject.Update(gameTime);
    }
    public void Draw(GameTime gameTime)
    {
        foreach (DrawableGameComponent gameObject in _components.TakeWhile(a => a is DrawableGameComponent).OrderBy(a => (a as DrawableGameComponent).DrawOrder))
        {
            if (gameObject.Visible)
                gameObject.Draw(gameTime);
        }
    }
 }