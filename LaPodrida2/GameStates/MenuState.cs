using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Engine;

namespace LaPodrida2;

public class MenuState : GameState
{
    #region Fields
    private SimpleImage logo;
    private SimpleImage logo2;
    private SimpleImage bg;
    private SimpleImage bg2;
    private SimpleImage filler;
    private SoundEffect futsteps;
    private SoundEffect laPodridaShout;
    private SoundEffect metal;
    private SoundEffect twoShout;
    private SoundEffect fade;
    private SoundEffect hover;
    private SoundEffect click;
    private SoundEffect error;
    private SoundEffect impact;
    private SoundEffect[] vo = new SoundEffect[6];
    /*
    00: "Welcome to LA PODRIDA 2,"
    01: "the long awaited sequel to the hit game LA PODRIDA 1."
    02: "Well, let's get going, would you like to..."
    03: "Play alone in singleplayer mode?"
    04: "Or play with your friends in multiplayer mode?"
    05: "Don't lie, you've got no friends."
    06: "Fine, I guess you can play with me then."
    */
    private Song bgm;
    private Texture2D playTexture;
    private Texture2D playHoveredTexture;
    private Texture2D configTexture;
    private Texture2D configHoveredTexture;
    private Texture2D exitTexture;
    private Texture2D exitHoveredTexture;
    private Texture2D backTexture;
    private Texture2D backHoveredTexture;
    private Texture2D singleplayerTexture;
    private Texture2D singleplayerHoveredTexture;
    private Texture2D multiplayerTexture;
    private Texture2D multiplayerHoveredTexture;
    private SimpleImage musicSprite;
    private SimpleImage sfxSprite;
    private Button playButton;
    private Button configButton;
    private Button exitButton;
    private Button musicPlusButton;
    private Button musicMinusButton;
    private Button sfxPlusButton;
    private Button sfxMinusButton;
    private Button backButton;
    private Button singleplayerButton;
    private Button multiplayerButton;
    private TextComponent musicText;
    private TextComponent sfxText;
    private TextComponent ccText;
    #endregion

    public MenuState(Game game) : base(game) {}

    public override void LoadContent()
    {
        #region Textures
        playTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\Play");
        playHoveredTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\PlayHovered");
        configTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\Config");
        configHoveredTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\ConfigHovered");
        exitTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\Exit");
        exitHoveredTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\ExitHovered");
        backTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\Back");
        backHoveredTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\BackHovered");
        singleplayerTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\1p");
        singleplayerHoveredTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\1pHovered");
        multiplayerTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\Multiplayer");
        multiplayerHoveredTexture = Game.Content.Load<Texture2D>(@"Textures\Menu\MultiplayerHovered");
        #endregion

        #region SimpleImages
        logo = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\Logo"), new Vector2(-150, 150), 7, scale: 0.6f);
        logo2 = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\2logo"), new Vector2(560, 130), 7, visible: false);
        var bgframes = new List<Rectangle>();
        for (int i = 0; i < 800; i++)
        {
            bgframes.Add(new Rectangle(i, i, 800, 800));
        }
        bg = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\bg"), Vector2.Zero, 0, visible: false, anchor: Alignment.TopLeft, animation: new Animation<Rectangle>(bgframes.ToArray(), true));
        bgframes = new List<Rectangle>();
        for (int i = 0; i < 800; i++)
        {
            bgframes.Add(new Rectangle(-i, i, 800, 800));
        }
        bg2 = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\shuttercock"), Vector2.Zero, 8, visible: false, anchor: Alignment.TopLeft, animation: new Animation<Rectangle>(bgframes.ToArray(), true));
        filler = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\filler"), Vector2.Zero, 10, visible: false, scale: 800, anchor: Alignment.TopLeft);
        musicSprite = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\Music"), new Vector2(350, 350), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(400, 100), new Point(400, 200), true, 30));
        sfxSprite = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\Sfx"), new Vector2(350, 500), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(400, 100), new Point(400, 200), true, 30));
        _components.Add(logo);
        _components.Add(logo2);
        _components.Add(bg);
        _components.Add(bg2);
        _components.Add(filler);
        _components.Add(musicSprite);
        _components.Add(sfxSprite);
        #endregion

        #region Buttons
        playButton = new Button(Game, new Rectangle(400, 350, 300, 100), new SimpleImage(Game, playTexture, new Vector2(400, 350), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(300, 100), new Point(300, 200), true, 30)), enabled: false, hasHover: true);
        configButton = new Button(Game, new Rectangle(400, 500, 400, 100), new SimpleImage(Game, configTexture, new Vector2(400, 500), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(400, 100), new Point(400, 200), true, 30)), enabled: false, hasHover: true);
        exitButton = new Button(Game, new Rectangle(400, 650, 300, 100), new SimpleImage(Game, exitTexture, new Vector2(400, 650), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(300, 100), new Point(300, 200), true, 30)), enabled: false, hasHover: true);
        backButton = new Button(Game, new Rectangle(400, 650, 300, 100), new SimpleImage(Game, backTexture, new Vector2(400, 650), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(300, 100), new Point(300, 200), true, 30)), enabled: false, hasHover: true);
        singleplayerButton = new Button(Game, new Rectangle(220, 475, 300, 400), new SimpleImage(Game, singleplayerTexture, new Vector2(220, 475), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(300, 400), new Point(600, 400), true, 30), scale: 2f), enabled: false, hasHover: true);
        multiplayerButton = new Button(Game, new Rectangle(580, 475, 300, 400), new SimpleImage(Game, multiplayerTexture, new Vector2(580, 475), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(300, 400), new Point(600, 400), true, 30), scale: 2f), enabled: false, hasHover: true);
        Texture2D texture = Game.Content.Load<Texture2D>(@"Textures\Menu\Minus");
        musicMinusButton = new Button(Game, new Rectangle(100, 350, 100, 100), new SimpleImage(Game, texture, new Vector2(100, 350), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30)), enabled: false, hasHover: true);
        sfxMinusButton = new Button(Game, new Rectangle(100, 500, 100, 100), new SimpleImage(Game, texture, new Vector2(100, 500), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30)), enabled: false, hasHover: true);
        texture = Game.Content.Load<Texture2D>(@"Textures\Menu\Plus");
        musicPlusButton = new Button(Game, new Rectangle(700, 350, 100, 100), new SimpleImage(Game, texture, new Vector2(700, 350), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30)), enabled: false, hasHover: true);
        sfxPlusButton = new Button(Game, new Rectangle(700, 500, 100, 100), new SimpleImage(Game, texture, new Vector2(700, 500), 9, false, animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30)), enabled: false, hasHover: true);
        _components.Add(playButton);
        _components.Add(configButton);
        _components.Add(exitButton);
        _components.Add(backButton);
        _components.Add(musicPlusButton);
        _components.Add(sfxPlusButton);
        _components.Add(musicMinusButton);
        _components.Add(sfxMinusButton);
        _components.Add(singleplayerButton);
        _components.Add(multiplayerButton);
        #endregion

        #region Audios
        futsteps = Game.Content.Load<SoundEffect>(@"Audio\Menu\futsteps");
        laPodridaShout = Game.Content.Load<SoundEffect>(@"Audio\Menu\LAPODRIDA");
        metal = Game.Content.Load<SoundEffect>(@"Audio\Menu\metal");
        twoShout = Game.Content.Load<SoundEffect>(@"Audio\Menu\2");
        fade = Game.Content.Load<SoundEffect>(@"Audio\Menu\fade");
        bgm = Game.Content.Load<Song>(@"Audio\Menu\Eipeskeip");
        click = Game.Content.Load<SoundEffect>(@"Audio\Menu\clicked");
        hover = Game.Content.Load<SoundEffect>(@"Audio\Menu\select");
        error = Game.Content.Load<SoundEffect>(@"Audio\Menu\error");
        impact = Game.Content.Load<SoundEffect>(@"Audio\Menu\impact");
        for (int i = 0; i < 7; i++)
            vo[i] = Game.Content.Load<SoundEffect>($@"Audio\Menu\vo{i + 1}");
        #endregion

        #region TextComponents
        var romanAlexander = Game.Content.Load<SpriteFont>(@"Other\romanAlexander");
        musicText = new TextComponent(Game, romanAlexander, $"{Configs.MusicVolume}", new Vector2(600, 350), 9, false, Alignment.Center);
        sfxText = new TextComponent(Game, romanAlexander, $"{Configs.SfxVolume}", new Vector2(600, 500), 9, false, Alignment.Center);
        ccText = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\consolas"), "I don't know what else you expected to find here..", new Vector2(400, 750), 9, false, Alignment.Center);
        _components.Add(musicText);
        _components.Add(sfxText);
        _components.Add(ccText);
        #endregion
    }

    public override void UnloadContent()
    {
        #region Textures
        Game.Content.UnloadAsset(@"Textures\Menu\Play");
        Game.Content.UnloadAsset(@"Textures\Menu\PlayHovered");
        Game.Content.UnloadAsset(@"Textures\Menu\Config");
        Game.Content.UnloadAsset(@"Textures\Menu\ConfigHovered");
        Game.Content.UnloadAsset(@"Textures\Menu\Exit");
        Game.Content.UnloadAsset(@"Textures\Menu\ExitHovered");
        Game.Content.UnloadAsset(@"Textures\Menu\Back");
        Game.Content.UnloadAsset(@"Textures\Menu\BackHovered");
        Game.Content.UnloadAsset(@"Textures\Menu\1p");
        Game.Content.UnloadAsset(@"Textures\Menu\1pHovered");
        Game.Content.UnloadAsset(@"Textures\Menu\Multiplayer");
        Game.Content.UnloadAsset(@"Textures\Menu\MultiplayerHovered");
        #endregion

        #region SimpleImages
        Game.Content.UnloadAsset(@"Textures\Menu\logo");
        Game.Content.UnloadAsset(@"Textures\Menu\2logo");
        Game.Content.UnloadAsset(@"Textures\Menu\bg");
        Game.Content.UnloadAsset(@"Textures\Menu\shuttercock");
        Game.Content.UnloadAsset(@"Textures\Menu\filler");
        Game.Content.UnloadAsset(@"Textures\Menu\Music");
        Game.Content.UnloadAsset(@"Textures\Menu\Sfx");
        #endregion

        #region Buttons
        Game.Content.UnloadAsset(@"Textures\Menu\Minus");
        Game.Content.UnloadAsset(@"Textures\Menu\Plus");
        #endregion

        #region Audios
        Game.Content.UnloadAsset(@"Audio\Menu\futsteps");
        Game.Content.UnloadAsset(@"Audio\Menu\LAPODRIDA");
        Game.Content.UnloadAsset(@"Audio\Menu\metal");
        Game.Content.UnloadAsset(@"Audio\Menu\2");
        Game.Content.UnloadAsset(@"Audio\Menu\fade");
        Game.Content.UnloadAsset(@"Audio\Menu\Eipeskeip");
        Game.Content.UnloadAsset(@"Audio\Menu\clicked");
        Game.Content.UnloadAsset(@"Audio\Menu\select");
        Game.Content.UnloadAsset(@"Audio\Menu\error");
        Game.Content.UnloadAsset(@"Audio\Menu\impact");
        Game.Content.UnloadAsset(@"Audio\Menu\vo1");
        Game.Content.UnloadAsset(@"Audio\Menu\vo2");
        Game.Content.UnloadAsset(@"Audio\Menu\vo3");
        Game.Content.UnloadAsset(@"Audio\Menu\vo4");
        Game.Content.UnloadAsset(@"Audio\Menu\vo5");
        Game.Content.UnloadAsset(@"Audio\Menu\vo6");
        Game.Content.UnloadAsset(@"Audio\Menu\vo7");
        #endregion

        #region TextComponents
        Game.Content.UnloadAsset(@"Other\romanAlexander");
        Game.Content.UnloadAsset(@"Other\consolas");
        #endregion
    }

    public override void Initialize()
    {
        base.Initialize();
        Game.Window.Title = "???";
        playButton.LeftClicked += PlayButton_Clicked;
        playButton.HoverDetector.Hovered += PlayButton_Hovered;
        playButton.HoverDetector.Unhovered += PlayButton_Unhovered;
        configButton.LeftClicked += ConfigButton_Clicked;
        configButton.HoverDetector.Hovered += ConfigButton_Hovered;
        configButton.HoverDetector.Unhovered += ConfigButton_Unhovered;
        exitButton.LeftClicked += ExitButton_Clicked;
        exitButton.HoverDetector.Hovered += ExitButton_Hovered;
        exitButton.HoverDetector.Unhovered += ExitButton_Unhovered;
        backButton.LeftClicked += BackButton_Clicked;
        backButton.HoverDetector.Hovered += BackButton_Hovered;
        backButton.HoverDetector.Unhovered += BackButton_Unhovered;
        musicPlusButton.LeftClicked += MusicPlusButton_Clicked;
        musicMinusButton.LeftClicked += MusicMinusButton_Clicked;
        sfxPlusButton.LeftClicked += SfxPlusButton_Clicked;
        sfxMinusButton.LeftClicked += SfxMinusButton_Clicked;
        musicPlusButton.HoverDetector.Hovered += VolumeHoverSound;
        musicMinusButton.HoverDetector.Hovered += VolumeHoverSound;
        sfxPlusButton.HoverDetector.Hovered += VolumeHoverSound;
        sfxMinusButton.HoverDetector.Hovered += VolumeHoverSound;
        singleplayerButton.LeftClicked += SingleplayerButton_Clicked;
        singleplayerButton.HoverDetector.Hovered += SingleplayerButton_Hovered;
        singleplayerButton.HoverDetector.Unhovered += SingleplayerButton_Unhovered;
        multiplayerButton.LeftClicked += MultiplayerButton_Clicked;
        multiplayerButton.HoverDetector.Hovered += MultiplayerButton_Hovered;
        multiplayerButton.HoverDetector.Unhovered += MultiplayerButton_Unhovered;
    }

    public override void Dispose()
    {
        playButton.LeftClicked -= PlayButton_Clicked;
        playButton.HoverDetector.Hovered -= PlayButton_Hovered;
        playButton.HoverDetector.Unhovered -= PlayButton_Unhovered;
        exitButton.LeftClicked -= ExitButton_Clicked;
        exitButton.HoverDetector.Hovered -= ExitButton_Hovered;
        exitButton.HoverDetector.Unhovered -= ExitButton_Unhovered;
        configButton.LeftClicked -= ConfigButton_Clicked;
        configButton.HoverDetector.Hovered -= ConfigButton_Hovered;
        configButton.HoverDetector.Unhovered -= ConfigButton_Unhovered;
        backButton.LeftClicked -= BackButton_Clicked;
        backButton.HoverDetector.Hovered -= BackButton_Hovered;
        backButton.HoverDetector.Unhovered -= BackButton_Unhovered;
        musicPlusButton.LeftClicked -= MusicPlusButton_Clicked;
        musicMinusButton.LeftClicked -= MusicMinusButton_Clicked;
        sfxPlusButton.LeftClicked -= SfxPlusButton_Clicked;
        sfxMinusButton.LeftClicked -= SfxMinusButton_Clicked;
        musicPlusButton.HoverDetector.Hovered -= VolumeHoverSound;
        musicMinusButton.HoverDetector.Hovered -= VolumeHoverSound;
        sfxPlusButton.HoverDetector.Hovered -= VolumeHoverSound;
        sfxMinusButton.HoverDetector.Hovered -= VolumeHoverSound;
        singleplayerButton.LeftClicked -= SingleplayerButton_Clicked;
        singleplayerButton.HoverDetector.Hovered -= SingleplayerButton_Hovered;
        singleplayerButton.HoverDetector.Unhovered -= SingleplayerButton_Unhovered;
        multiplayerButton.LeftClicked -= MultiplayerButton_Clicked;
        multiplayerButton.HoverDetector.Hovered -= MultiplayerButton_Hovered;
        multiplayerButton.HoverDetector.Unhovered -= MultiplayerButton_Unhovered;

        base.Dispose();
    }

    public override void Update(GameTime gameTime)
    {
        if (bg.Visible)
            logo2.Rotation += 0.5f;

        base.Update(gameTime);
    }

    public override void HandleInput(object s, InputKeyEventArgs e)
    {
        if (logo.Position.X == -150)
            IntroSequence();

        base.HandleInput(s, e);
    }

    public override void HandleInput(object s, ButtonEventArgs e)
    {
        if (logo.Position.X == -150)
            IntroSequence();

        base.HandleInput(s, e);
    }

    private async void IntroSequence()
    {
        var futs = futsteps.CreateInstance();
        futs.Play();
        while (logo.Position.X < 360)
        {
            logo.Position += new Vector2(5, 0);
            logo.Rotation += Random.Shared.Next(-5, 6);
            await Task.Delay(17);
        }
        futs.Stop();
        logo.Rotation = 0;
        Game.Window.Title = "La Podrida";
        laPodridaShout.Play();
        await Task.Delay(laPodridaShout.Duration);
        logo2.Visible = true;
        logo2.Opacity = 0;
        while (logo2.Scale > 0.15f)
        {
            if (logo2.Opacity != 1)
                logo2.Opacity += 0.1f;
            logo2.Scale -= 0.05f;
            await Task.Delay(17);
        }
        Game.Window.Title = "La Podrida 2";
        metal.Play();
        twoShout.Play();
        await Task.Delay(metal.Duration);
        fade.Play();
        filler.Visible = true;
        filler.Opacity = 0;
        for (int i = 0; i < 100; i++)
        {
            filler.Opacity += 0.01f;
            await Task.Delay(TimeSpan.FromMilliseconds(47));
        }
        await Task.Delay(350);
        filler.Visible = false;
        bg.Visible = true;
        bg2.Visible = true;
        MediaPlayer.Play(bgm);
        MediaPlayer.IsRepeating = true;
        Game.Window.Title = "La Podrida 2 - A game by Alvaro Garcia Garcia and Jose Maria Fernandez Fernandez";
        AfterIntroElements();
    }

    private void AfterIntroElements()
    {
        playButton.Image.Visible = true;
        playButton.Enabled = true;
        configButton.Image.Visible = true;
        configButton.Enabled = true;
        exitButton.Image.Visible = true;
        exitButton.Enabled = true;
    }

    private void PlayButton_Hovered(object sender, EventArgs e)
    {
        hover.Play();
        playButton.Image.ChangeAnimatedTexture(playHoveredTexture, Animation<Rectangle>.TextureAnimation(new Point(500, 100), new Point(500, 200), true, 30));
    }

    private void PlayButton_Unhovered(object sender, EventArgs e)
    {
        playButton.Image.ChangeAnimatedTexture(playTexture, Animation<Rectangle>.TextureAnimation(new Point(300, 100), new Point(300, 200), true, 30));
    }

    private void ConfigButton_Hovered(object sender, EventArgs e)
    {
        hover.Play();
        configButton.Image.ChangeAnimatedTexture(configHoveredTexture, Animation<Rectangle>.TextureAnimation(new Point(600, 100), new Point(600, 200), true, 30));
    }

    private void ConfigButton_Unhovered(object sender, EventArgs e)
    {
        configButton.Image.ChangeAnimatedTexture(configTexture, Animation<Rectangle>.TextureAnimation(new Point(400, 100), new Point(400, 200), true, 30));
    }

    private void ExitButton_Hovered(object sender, EventArgs e)
    {
        hover.Play();
        exitButton.Image.ChangeAnimatedTexture(exitHoveredTexture, Animation<Rectangle>.TextureAnimation(new Point(500, 100), new Point(500, 200), true, 30));
    }

    private void ExitButton_Unhovered(object sender, EventArgs e)
    {
        exitButton.Image.ChangeAnimatedTexture(exitTexture, Animation<Rectangle>.TextureAnimation(new Point(300, 100), new Point(300, 200), true, 30));
    }

    private void BackButton_Hovered(object sender, EventArgs e)
    {
        hover.Play();
        backButton.Image.ChangeAnimatedTexture(backHoveredTexture, Animation<Rectangle>.TextureAnimation(new Point(500, 100), new Point(500, 200), true, 30));
    }

    private void BackButton_Unhovered(object sender, EventArgs e)
    {
        backButton.Image.ChangeAnimatedTexture(backTexture, Animation<Rectangle>.TextureAnimation(new Point(300, 100), new Point(300, 200), true, 30));
    }

    private async void PlayButton_Clicked(object sender, EventArgs e)
    {
        Configs.CloseFile();
        click.Play();

        playButton.Enabled = false;
        configButton.Enabled = false;
        exitButton.Enabled = false;
        playButton.Image.Visible = false;
        configButton.Image.Visible = false;
        exitButton.Image.Visible = false;

        await Task.Delay(500);

        vo[0].Play();
        ccText.Visible = true;
        ccText.Text = "Welcome to LA PODRIDA 2,";
        await Task.Delay(vo[0].Duration + TimeSpan.FromMilliseconds(200));
        vo[1].Play();
        ccText.Text = "the long awaited sequel to the hit game LA PODRIDA 1.";
        await Task.Delay(vo[1].Duration + TimeSpan.FromMilliseconds(500));
        vo[2].Play();
        ccText.Text = "Well, let's get going, would you like to...";
        await Task.Delay(vo[2].Duration + TimeSpan.FromMilliseconds(200));
        vo[3].Play();
        ccText.Text = "Play alone in singleplayer mode?";
        await Task.Delay(vo[3].Duration);
        singleplayerButton.Image.Opacity = 0;
        singleplayerButton.Image.Visible = true;
        while (singleplayerButton.Image.Scale > 1)
        {
            if (singleplayerButton.Image.Opacity != 1)
                singleplayerButton.Image.Opacity += 0.2f;
            singleplayerButton.Image.Scale -= 0.1f;
            await Task.Delay(17);
        }
        impact.Play();
        await Task.Delay(impact.Duration);
        vo[4].Play();
        ccText.Text = "Or play with your friends in multiplayer mode?";
        await Task.Delay(vo[4].Duration);
        multiplayerButton.Image.Opacity = 0;
        multiplayerButton.Image.Visible = true;
        while (multiplayerButton.Image.Scale > 1)
        {
            if (multiplayerButton.Image.Opacity != 1)
                multiplayerButton.Image.Opacity += 0.2f;
            multiplayerButton.Image.Scale -= 0.1f;
            await Task.Delay(17);
        }
        impact.Play();
        await Task.Delay(impact.Duration);
        
        singleplayerButton.Enabled = true;
        multiplayerButton.Enabled = true;
        ccText.Text = "";
    }

    private void ConfigButton_Clicked(object sender, EventArgs e)
    {
        click.Play();
        playButton.Image.Visible = false;
        playButton.Enabled = false;
        configButton.Image.Visible = false;
        configButton.Enabled = false;
        exitButton.Image.Visible = false;
        exitButton.Enabled = false;
        musicPlusButton.Image.Visible = true;
        musicPlusButton.Enabled = true;
        musicMinusButton.Image.Visible = true;
        musicMinusButton.Enabled = true;
        sfxPlusButton.Image.Visible = true;
        sfxPlusButton.Enabled = true;
        sfxMinusButton.Image.Visible = true;
        sfxMinusButton.Enabled = true;
        musicSprite.Visible = true;
        sfxSprite.Visible = true;
        backButton.Image.Visible = true;
        backButton.Enabled = true;
        musicText.Visible = true;
        sfxText.Visible = true;
        ccText.Visible = true;
    }

    private void VolumeHoverSound(object sender, EventArgs e) => hover.Play();

    private void MusicPlusButton_Clicked(object sender, EventArgs e)
    {
        click.Play();
        if (Configs.MusicVolume < 10)
            Configs.MusicVolume += 1;

        musicText.Text = $"{Configs.MusicVolume}";
    }

    private void MusicMinusButton_Clicked(object sender, EventArgs e)
    {
        click.Play();
        if (Configs.MusicVolume > 0)
            Configs.MusicVolume -= 1;

        musicText.Text = $"{Configs.MusicVolume}";
    }

    private void SfxPlusButton_Clicked(object sender, EventArgs e)
    {
        click.Play();
        if (Configs.SfxVolume < 10)
            Configs.SfxVolume += 1;

        sfxText.Text = $"{Configs.SfxVolume}";
    }

    private void SfxMinusButton_Clicked(object sender, EventArgs e)
    {
        click.Play();
        if (Configs.SfxVolume > 0)
            Configs.SfxVolume -= 1;

        sfxText.Text = $"{Configs.SfxVolume}";
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        click.Play();
        playButton.Image.Visible = true;
        playButton.Enabled = true;
        configButton.Image.Visible = true;
        configButton.Enabled = true;
        exitButton.Image.Visible = true;
        exitButton.Enabled = true;
        musicPlusButton.Image.Visible = false;
        musicPlusButton.Enabled = false;
        musicMinusButton.Image.Visible = false;
        musicMinusButton.Enabled = false;
        sfxPlusButton.Image.Visible = false;
        sfxPlusButton.Enabled = false;
        sfxMinusButton.Image.Visible = false;
        sfxMinusButton.Enabled = false;
        musicSprite.Visible = false;
        sfxSprite.Visible = false;
        backButton.Image.Visible = false;
        backButton.Enabled = false;
        musicText.Visible = false;
        sfxText.Visible = false;
        ccText.Visible = false;
    }

    private void ExitButton_Clicked(object sender, EventArgs e)
    {
        Game.Exit();
    }

    private void MultiplayerButton_Hovered(object sender, EventArgs e)
    {
        hover.Play();
        multiplayerButton.Image.ChangeAnimatedTexture(multiplayerHoveredTexture, Animation<Rectangle>.TextureAnimation(new Point(300, 400), new Point(600, 400), true, 30));
    }

    private void MultiplayerButton_Unhovered(object sender, EventArgs e)
    {
        multiplayerButton.Image.ChangeAnimatedTexture(multiplayerTexture, Animation<Rectangle>.TextureAnimation(new Point(300, 400), new Point(600, 400), true, 30));
    }

    private async void MultiplayerButton_Clicked(object sender, EventArgs e)
    {
        error.Play();
        vo[5].Play();
        singleplayerButton.Enabled = false;
        multiplayerButton.Enabled = false;
        MultiplayerButton_Unhovered(sender, e);
        multiplayerButton.Image.Color = new Color(0.75f, 0.75f, 0.75f);
        multiplayerButton.Image.Opacity = 0.75f;
        ccText.Text = "Don't lie, you've got no friends.";
        await Task.Delay(vo[5].Duration);
        singleplayerButton.Enabled = true;
        ccText.Text = "";
    }

    private void SingleplayerButton_Hovered(object sender, EventArgs e)
    {
        hover.Play();
        singleplayerButton.Image.ChangeAnimatedTexture(singleplayerHoveredTexture, Animation<Rectangle>.TextureAnimation(new Point(300, 400), new Point(600, 400), true, 30));
    }

    private void SingleplayerButton_Unhovered(object sender, EventArgs e)
    {
        singleplayerButton.Image.ChangeAnimatedTexture(singleplayerTexture, Animation<Rectangle>.TextureAnimation(new Point(300, 400), new Point(600, 400), true, 30));
    }

    private async void SingleplayerButton_Clicked(object sender, EventArgs e)
    {
        Game.Window.Title = "La Podrida 2 - Single Player";

        foreach (GameComponent gameObject in _components)
            gameObject.Enabled = false;


        click.Play();
        vo[6].Play();
        ccText.Text = "Fine, I guess you can play with me then.";
        await Task.Delay(vo[6].Duration);


        while (bg.Position.Y > -800)
        {
            foreach (GameComponent gameObject in _components)
            {
                if (gameObject is SimpleImage)
                    (gameObject as SimpleImage).Position += new Vector2(0f, -5f);

                if (gameObject is Button)
                    (gameObject as Button).Image.Position += new Vector2(0f, -5f);

                if (gameObject is TextComponent)
                    (gameObject as TextComponent).Position += new Vector2(0f, -5f);
            }
            await Task.Delay(17);
        }
        SwitchState(new TutorialState(Game));
    }
}