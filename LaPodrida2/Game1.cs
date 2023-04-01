using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Engine;

namespace LaPodrida2;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState _currentGameState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowAltF4 = true;
        Window.AllowUserResizing = false;
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 800;
    }

    #region Framework Methods
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        if (GraphicsDevice.Adapter.CurrentDisplayMode.Height < 800)
        {
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
        }

        base.Initialize();
        Window.KeyDown += UpdateInputs;
        Window.KeyUp += UpdateInputs;

        Activated += Statics.Focus;
        Deactivated += Statics.UnFocus;
        Statics.Focus(null, null);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService<SpriteBatch>(_spriteBatch);
        Configs.Initialize(GraphicsDevice.Adapter.CurrentDisplayMode.Height < 800);
        UpdateMusicVolume(null, null);
        UpdateSfxVolume(null, null);
        Configs.MusicVolumeChanged += UpdateMusicVolume;
        Configs.SfxVolumeChaged += UpdateSfxVolume;

        // TODO: use this.Content to load your game content here
        SwitchGameState(new MenuState(this));
    }

    protected override void Update(GameTime gameTime)
    {
        Input.UpdateInputs(Mouse.GetState());
        Input.MousePoint = Mouse.GetState().Position;
        _currentGameState.Update(gameTime);

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
        _currentGameState.Draw(gameTime);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        Configs.CloseFile();
        base.OnExiting(sender, args);
    }
    #endregion

    #region Custom Methods
    private void SwitchGameState(GameState newGameState)
    {
        if (_currentGameState is not null)
        {
            _currentGameState.OnStateSwitched -= OnStateSwitched;
            _currentGameState.UnloadContent();
            _currentGameState.Dispose();
        }

        _currentGameState = newGameState;

        _currentGameState.LoadContent();
        _currentGameState.Initialize();

        _currentGameState.OnStateSwitched += OnStateSwitched;
    }

    private void OnStateSwitched(object s, GameState e) => SwitchGameState(e);

    private void UpdateInputs(object s, InputKeyEventArgs e) => Input.UpdateInputs(Keyboard.GetState());

    private void UpdateMusicVolume(object s, EventArgs e) {MediaPlayer.Volume = MathF.Pow((float)Configs.MusicVolume * 0.1f, 2); MediaPlayer.IsMuted = muter(Configs.MusicVolume);}
    private void UpdateSfxVolume(object s, EventArgs e) {SoundEffect.MasterVolume = MathF.Pow((float)Configs.SfxVolume * 0.1f, 2); SoundEffect.MasterVolume = muter(Configs.SfxVolume) ? 0 : SoundEffect.MasterVolume;}

    private Func<int, bool> muter = x => x == 0;
    #endregion
}
