using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Engine;

// HOLA ESTIMADO JERONIMO LUIS SANCHEZ PERUGA
namespace LaPodrida2;

public class CasinoState : GameState
{
    #region Fields
    private Texture2D back;
    private Texture2D angry;
    private Texture2D cry;
    private Texture2D idle;
    private Texture2D inverted;
    private Texture2D laugh;
    private Texture2D monocle;
    private Texture2D musculito;
    private Texture2D nervous;
    private Texture2D sus;
    private Texture2D t;
    private SimpleImage bg;
    private SimpleImage table;
    private SimpleImage deck;
    private SimpleImage map;
    private SimpleImage gameBg;
    private SimpleImage electro20;
    private SimpleImage yourPaper;
    private SimpleImage electroPaper;
    private SimpleImage chorizo;
    private SimpleImage circle;
    private SimpleImage exploeffect;
    private TextComponent ccText;
    private CardData[] electroCards;
    private SimpleImage[] electroCardImages = new SimpleImage[3];
    private CardData[] goldenCards;
    private SimpleImage[] goldenCardImages = new SimpleImage[3];
    private CardData[] yourCards;
    private SimpleImage[] yourCardImages = new SimpleImage[3];
    private CardData[] dejavuCards;
    private int yourPlayedCard = -1;
    private int electroPlayedCard = -1;
    private int yourPoints = 0;
    private int electroPoints = 0;
    private TextComponent yourPointsLabel;
    private TextComponent electroPointsLabel;
    private bool yourRounds = false;
    private bool electroRounds = false;
    private int roundCount = 0;
    private int handCount = 0;
    private Song streetShit;
    private Song bgm;
    private Song conflict;
    private Button[] yourCardButtons = new Button[3];
    private Button deckButton;
    private Button[] cardPlacerButtons = new Button[9];
    private SimpleImage oscilatingOpacityImageReference;
    private SimpleImage plus1;
    private SimpleImage plus2;
    private SimpleImage equal;
    private TextComponent roundResult;
    private bool goingUp = false;
    private SoundEffect cardGrab;
    private SoundEffect cardPlace;
    private SoundEffect deckGrab;
    private SoundEffect deckGrabShuffle;
    private SoundEffect deckPlace;
    private SoundEffect explosion;
    private SoundEffect[] vo = new SoundEffect[15];
    /*
    00: "Well, here we are, the casino."
    01: "Yes, it's an outdoor casino, stop complaining about everything."
    02: "Whatever, I'll go inside and sign us up for the tournament."
    03: "Hey, check it out!"
    04: "We've gotta play against each other in the first round."
    05: "I'm gonna win, of course."
    06: "Ohh, don't be sad, I'll give you 1% of the prize!"
    07: "So, what are we waiting for, let's start!"
    08: "Come on, hand out the last round..."
    09: "Where did the deck go?"
    10: "Don't look at me, I didn't take it!"
    11: "Woah, where did that come from?"
    12: "WAIT, I'M NOT CHEATING, I SWEAR!"
    13: "NO, CHORIZO NO!"
    14: "OK, I ADMIT IT, I CHEATED,\nI CHEATED, STOP, PLEASE!"
    */
    private SoundEffect[] vo_win = new SoundEffect[6];
    private string[] cc_win = new string[6]
    {   
        "Off to a good start, I have the advantage.",
        "Hahaha, aperently you're unable to defeat an arduino!",
        "If I won a dolar each time I beat someone like you,\nI would be a billionaire right now.",
        "That was sweet, thank you random number generator!",
        "Who's laughing now?",
        "Dude, did you see my cards?! That was amazing!"
    };
    private Texture2D[] texture_win = new Texture2D[6];
    private SoundEffect[] vo_lose = new SoundEffect[6];
    private string[] cc_lose = new string[6]
    {
        "That's some begginers luck, I'll get you next time.",
        "Arduino not responding...",
        "Segment Fault (Core Dumped)",
        "Wha- Well you're lucky... Medibot failed...",
        "Once this match is over, you're gonna be working for me!",
        "HOW DID YOU GET THAT?!\nI WAS SUPPOSED TO WIN THAT!!!"
    };
    private Texture2D[] texture_lose = new Texture2D[6];
    #endregion

    public CasinoState(Game game) : base(game) {}

    public override void LoadContent()
    {
        #region Textures
        back = Game.Content.Load<Texture2D>(@"Textures\Cards\back_red");
        CardData.LoadTextures(Game.Content, true);

        angry = Game.Content.Load<Texture2D>(@"Textures\Electro20\angry");
        cry = Game.Content.Load<Texture2D>(@"Textures\Electro20\cry");
        idle = Game.Content.Load<Texture2D>(@"Textures\Electro20\idle");
        inverted = Game.Content.Load<Texture2D>(@"Textures\Electro20\inverted");
        laugh = Game.Content.Load<Texture2D>(@"Textures\Electro20\laugh");
        monocle = Game.Content.Load<Texture2D>(@"Textures\Electro20\monocle");
        musculito = Game.Content.Load<Texture2D>(@"Textures\Electro20\musculito");
        nervous = Game.Content.Load<Texture2D>(@"Textures\Electro20\nervous");
        sus = Game.Content.Load<Texture2D>(@"Textures\Electro20\sus");
        t = Game.Content.Load<Texture2D>(@"Textures\Electro20\t");

        texture_win[0] = monocle;
        texture_win[1] = laugh;
        texture_win[2] = monocle;
        texture_win[3] = musculito;
        texture_win[4] = laugh;
        texture_win[5] = angry;

        texture_lose[0] = monocle;
        texture_lose[1] = t;
        texture_lose[2] = t;
        texture_lose[3] = inverted;
        texture_lose[4] = angry;
        texture_lose[5] = cry;
        #endregion

        #region SimpleImages
        bg = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\bg"), new Vector2(0f, 800f), 0, anchor: Alignment.TopLeft);
        table = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\Table"), new Vector2(400f, 800f), 2, anchor: Alignment.TopCenter);
        deck = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\deck_red"), new Vector2(400f, -200f), 8);
        gameBg = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\gameBg"), Vector2.Zero, 3, anchor: Alignment.TopLeft, visible: false, /*animation: Animation<Rectangle>.TextureAnimation(new Point(800, 800), new Point(1600, 800), true, 30),*/ opacity: 0f);
        map = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\map"), new Vector2(0f, 800f), 2, anchor: Alignment.TopLeft, visible: true, animation: Animation<Rectangle>.TextureAnimation(new Point(800, 600), new Point(800, 1200), true, 30));
        electro20 = new SimpleImage(Game, idle, new Vector2(400f, 1300f), 1, animation: Animation<Rectangle>.TextureAnimation(new Point(500), new Point(1000, 500), true, 30));
        yourPaper = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\paper"), new Vector2(670f, 650f), 2, visible: false);
        electroPaper = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\paper"), new Vector2(670f, 150f), 2, visible: false);
        chorizo = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\chorizo"), new Vector2(800f, 360f), 2, anchor: Alignment.CenterLeft, visible: false);
        circle = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\circle"), new Vector2(130f, 400f), 9, visible: false);
        exploeffect = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\explosion"), new Vector2(400f, 400f), 10, visible: false, scale: 3f, animation: Animation<Rectangle>.TextureAnimation(new Point(200, 282), new Point(3600, 282), false, 1));
        plus1 = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\plus"), new Vector2(333f, 400f), 10, visible: false, scale: .5f, color: Color.Black, animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30));
        plus2 = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\plus"), new Vector2(467f, 400f), 10, visible: false, scale: .5f, color: Color.Black, animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30));
        equal = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\equals"), new Vector2(602f, 400f), 10, visible: false, scale: .5f, color: Color.Black, animation: Animation<Rectangle>.TextureAnimation(new Point(135), new Point(270, 135), true, 30));

        map.Animation.Paused = true;
        for (int i = 0; i < 3; i++)
        {
            electroCardImages[i] = new SimpleImage(Game, back, new Vector2(400f, 100f), 6, false);
            goldenCardImages[i] = new SimpleImage(Game, back, new Vector2(400f, 100f), 7, false);
            yourCardImages[i] = new SimpleImage(Game, back, new Vector2(400f, 100f), 8, false);
            _components.Add(electroCardImages[i]);
            _components.Add(goldenCardImages[i]);
            _components.Add(yourCardImages[i]);
        }
        
        _components.Add(bg);
        _components.Add(table);
        _components.Add(deck);
        _components.Add(gameBg);
        _components.Add(map);
        _components.Add(electro20);
        _components.Add(yourPaper);
        _components.Add(electroPaper);
        _components.Add(chorizo);
        _components.Add(circle);
        _components.Add(exploeffect);
        _components.Add(plus1);
        _components.Add(plus2);
        _components.Add(equal);
        #endregion

        #region Buttons
        yourCardButtons[0] = new Button(Game, new Rectangle(265, 700, 88, 124), enabled: false, hasHover: false);
        yourCardButtons[1] = new Button(Game, new Rectangle(400, 700, 88, 124), enabled: false, hasHover: false);
        yourCardButtons[2] = new Button(Game, new Rectangle(535, 700, 88, 124), enabled: false, hasHover: false);
        _components.Add(yourCardButtons[0]);
        _components.Add(yourCardButtons[1]);
        _components.Add(yourCardButtons[2]);
        deckButton = new Button(Game, new Rectangle(130, 400, 88, 140), enabled: false, hasHover: false, texture: new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\deck_red"), new Vector2(130f, 400f), 4, false));
        _components.Add(deckButton);
        for (int i = 0; i < 6; i+=2)
        {
            cardPlacerButtons[i] = new Button(Game, new Rectangle(265 + (i/2 * 135), 250, 88, 124), enabled: false, hasHover: false, texture: new SimpleImage(Game, back, new Vector2(265 + (i/2 * 135), 250), 4, false));
            cardPlacerButtons[i+1] = new Button(Game, new Rectangle(265 + (i/2 * 135), 550, 88, 124), enabled: false, hasHover: false, texture: new SimpleImage(Game, back, new Vector2(265 + (i/2 * 135), 550), 4, false));
            cardPlacerButtons[i/2 + 6] = new Button(Game, new Rectangle(265 + (i/2 * 135), 400, 88, 124), enabled: false, hasHover: false, texture: new SimpleImage(Game, back, new Vector2(265 + (i/2 * 135), 400), 4, false));
            _components.Add(cardPlacerButtons[i]);
            _components.Add(cardPlacerButtons[i+1]);
            _components.Add(cardPlacerButtons[i/2 + 6]);
        }
        #endregion

        #region Audios
        cardGrab = Game.Content.Load<SoundEffect>(@"Audio\cardGrab");
        cardPlace = Game.Content.Load<SoundEffect>(@"Audio\cardPlace");
        deckGrab = Game.Content.Load<SoundEffect>(@"Audio\deckGrab");
        deckGrabShuffle = Game.Content.Load<SoundEffect>(@"Audio\deckGrabShuffle");
        deckPlace = Game.Content.Load<SoundEffect>(@"Audio\deckPlace");
        explosion = Game.Content.Load<SoundEffect>(@"Audio\Casino\explosion");
        streetShit = Game.Content.Load<Song>(@"Audio\Casino\street");
        bgm = Game.Content.Load<Song>(@"Audio\Casino\bgm");
        conflict = Game.Content.Load<Song>(@"Audio\Casino\conflict");
        for (int i = 0; i < 6; i++)
        {
            vo[i] = Game.Content.Load<SoundEffect>($@"Audio\Casino\vo{i}");
            vo_win[i] = Game.Content.Load<SoundEffect>($@"Audio\Casino\vo{i}win");
            vo_lose[i] = Game.Content.Load<SoundEffect>($@"Audio\Casino\vo{i}lose");
        }
        for (int i = 6; i < 15; i++)
            vo[i] = Game.Content.Load<SoundEffect>($@"Audio\Casino\vo{i}");
        #endregion

        #region TextComponents
        roundResult = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\romanAlexander"), "", new Vector2(670f, 400f), 10, anchor: Alignment.Center, color: Color.Black, visible: false);
        ccText = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\consolas"), "", new Vector2(400f, 750f), 10, anchor: Alignment.Center);
        yourPointsLabel = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\romanAlexander"), "0", new Vector2(670f, 650f), 2, anchor: Alignment.Center, color: Color.Black, visible: false);
        electroPointsLabel = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\romanAlexander"), "0", new Vector2(670f, 150f), 2, anchor: Alignment.Center, color: Color.Black, visible: false);
        _components.Add(roundResult);
        _components.Add(ccText);
        _components.Add(yourPointsLabel);
        _components.Add(electroPointsLabel);
        #endregion
    }

    public override void UnloadContent()
    {
        #region Textures
        Game.Content.UnloadAsset(@"Textures\Cards\back_red");
        CardData.UnloadTextures(Game.Content);
        Game.Content.UnloadAsset(@"Textures\Electro20\angry");
        Game.Content.UnloadAsset(@"Textures\Electro20\cry");
        Game.Content.UnloadAsset(@"Textures\Electro20\idle");
        Game.Content.UnloadAsset(@"Textures\Electro20\inverted");
        Game.Content.UnloadAsset(@"Textures\Electro20\laugh");
        Game.Content.UnloadAsset(@"Textures\Electro20\monocle");
        Game.Content.UnloadAsset(@"Textures\Electro20\musculito");
        Game.Content.UnloadAsset(@"Textures\Electro20\nervous");
        Game.Content.UnloadAsset(@"Textures\Electro20\sus");
        Game.Content.UnloadAsset(@"Textures\Electro20\t");
        #endregion
    
        #region SimpleImages
        Game.Content.UnloadAsset(@"Textures\Casino\bg");
        Game.Content.UnloadAsset(@"Textures\Casino\Table");
        Game.Content.UnloadAsset(@"Textures\Casino\deck_red");
        Game.Content.UnloadAsset(@"Textures\Casino\gameBg");
        Game.Content.UnloadAsset(@"Textures\Casino\map");
        Game.Content.UnloadAsset(@"Textures\Casino\paper");
        Game.Content.UnloadAsset(@"Textures\Casino\chorizo");
        Game.Content.UnloadAsset(@"Textures\Casino\circle");
        Game.Content.UnloadAsset(@"Textures\Casino\explosion");
        Game.Content.UnloadAsset(@"Textures\Menu\plus");
        Game.Content.UnloadAsset(@"Textures\Casino\equals");
        #endregion
        
        #region Audios
        Game.Content.UnloadAsset(@"Audio\cardGrab");
        Game.Content.UnloadAsset(@"Audio\cardPlace");
        Game.Content.UnloadAsset(@"Audio\deckGrab");
        Game.Content.UnloadAsset(@"Audio\deckGrabShuffle");
        Game.Content.UnloadAsset(@"Audio\deckPlace");
        Game.Content.UnloadAsset(@"Audio\Casino\explosion");
        Game.Content.UnloadAsset(@"Audio\Casino\street");
        Game.Content.UnloadAsset(@"Audio\Casino\bgm");
        Game.Content.UnloadAsset(@"Audio\Casino\conflict");
        for (int i = 0; i < 6; i++)
        {
            Game.Content.UnloadAsset($@"Audio\Casino\vo{i}");
            Game.Content.UnloadAsset($@"Audio\Casino\vo{i}win");
            Game.Content.UnloadAsset($@"Audio\Casino\vo{i}lose");
        }
        for (int i = 6; i < 15; i++)
            Game.Content.UnloadAsset($@"Audio\Casino\vo{i}");
        #endregion
    
        #region TextComponents
        Game.Content.UnloadAsset(@"Other\romanAlexander");
        Game.Content.UnloadAsset(@"Other\consolas");
        #endregion
    }

    public override void Initialize()
    {
        base.Initialize();
        MediaPlayer.Stop();
        yourCardButtons[0].LeftClicked += PlayCard;
        yourCardButtons[1].LeftClicked += PlayCard;
        yourCardButtons[2].LeftClicked += PlayCard;
        deckButton.LeftClicked += Decker;
        foreach (Button b in cardPlacerButtons)
        {
            b.LeftClicked += PlaceCard;
        }
        IntroSequence();
    }

    public override void Dispose()
    {
        MediaPlayer.Stop();
        yourCardButtons[0].LeftClicked -= PlayCard;
        yourCardButtons[1].LeftClicked -= PlayCard;
        yourCardButtons[2].LeftClicked -= PlayCard;
        deckButton.LeftClicked -= Decker;
        foreach (Button b in cardPlacerButtons)
        {
            b.LeftClicked -= PlaceCard;
        }
        base.Dispose();
    }

    public override void Update(GameTime gameTime)
    {
        if (oscilatingOpacityImageReference is not null)
        {
            if (goingUp)
            {
                oscilatingOpacityImageReference.Opacity += 0.01f;
                if (oscilatingOpacityImageReference.Opacity >= .8f)
                    goingUp = false;
            }
            else
            {
                oscilatingOpacityImageReference.Opacity -= 0.01f;
                if (oscilatingOpacityImageReference.Opacity <= .2f)
                    goingUp = true;
            }
        }

        if (chorizo is not null && chorizo.Visible)
            chorizo.Position += new Vector2(-1f, 0f);

        base.Update(gameTime);
    }

    private async void IntroSequence()
    {
        while (bg.Position.Y > 0)
        {
            bg.Position += new Vector2(0f, -5f);
            electro20.Position += new Vector2(0f, -5f);
            await Task.Delay(17);
        }
        MediaPlayer.Play(streetShit);
        vo[0].Play();
        ccText.Text = "Well, here we are, the casino.";
        await Task.Delay(vo[0].Duration + TimeSpan.FromMilliseconds(500));
        vo[1].Play();
        Game.Window.Title = "La Podrida 2 - Casino";
        ccText.Text = "Yes, it's an outdoor casino,\nstop complaining about everything.";
        electro20.ChangeAnimatedTexture(sus, null);
        await Task.Delay(vo[1].Duration + TimeSpan.FromMilliseconds(500));
        vo[2].Play();
        ccText.Text = "Whatever, I'll go inside and\nsign us up for the tournament.";
        electro20.ChangeAnimatedTexture(idle, null);
        await Task.Delay(vo[2].Duration);

        while (electro20.Scale > 0f)
        {
            electro20.Scale -= 0.05f;
            await Task.Delay(17);
        }
        await Task.Delay(1000);
        while (electro20.Scale < 1f)
        {
            electro20.Scale += 0.05f;
            await Task.Delay(17);
        }
        electro20.Scale = 1f;

        vo[3].Play();
        ccText.Text = "Hey, check it out!";
        electro20.ChangeAnimatedTexture(musculito, null);
        await Task.Delay(vo[3].Duration);

        map.Animation.Start();
        while (map.Position.Y > 100f)
        {
            map.Position += new Vector2(0f, -5f);
            await Task.Delay(17);
        }
        map.Animation.Paused = false;

        vo[4].Play();
        ccText.Text = "We've gotta play against each other in the first round.";
        await Task.Delay(vo[4].Duration);

        vo[5].Play();
        ccText.Text = "I'm gonna win, of course.";
        electro20.ChangeAnimatedTexture(laugh, null);

        while (map.Position.Y < 800f)
        {
            map.Position += new Vector2(0f, 5f);
            await Task.Delay(17);
        }
        map.Visible = false;

        vo[6].Play();
        ccText.Text = "Ohh, don't be sad, I'll give you 1% of the prize!";
        electro20.ChangeAnimatedTexture(monocle, null);
        await Task.Delay(vo[6].Duration);
        vo[7].Play();
        ccText.Text = "So, what are we waiting for, let's start!";
        electro20.ChangeAnimatedTexture(musculito, null);

        while (table.Position.Y > 450)
        {
            table.Position += new Vector2(0f, -5f);
            electro20.Position += new Vector2(0f, -2f);
            await Task.Delay(17);
        }

        while (deck.Position.Y < 500)
        {
            deck.Position += new Vector2(0f, 50f);
            await Task.Delay(17);
        }

        await Task.Delay(500);

        gameBg.Visible = true;
        while (gameBg.Opacity < 1f)
        {
            gameBg.Opacity += 0.02f;
            if (deck.Position.Y < 705)
                deck.Position += new Vector2(0f, 5f);
            if (deck.Position.Y >= 705 && deck.Position.Y < 708)
                deck.Position += new Vector2(0f, 1f);
            await Task.Delay(17);
        }
        bg.Visible = false;
        table.Visible = false;
        electro20.Visible = false;
        deck.Position = new Vector2(400f, 708f);

        MediaPlayer.Play(bgm);
        await Task.Delay(500);
        ccText.Text = "";
        //ccText.Position = new Vector2(400f, 50f);
        StateMachineHandler();
    }

    private void StateMachineHandler()
    {
        Game.Window.Title = $"La Podrida 2 - Hand {handCount+1}";
        if (handCount % 2 == 0)
        {
            if (handCount is 0) Decker(null, null);
            YourHandOut();
        } else
            ElectroHandOut();
    }

    private void YourHandOut()
    {
        for (int i = 0; i < 3; i++)
        {
            yourCardImages[i].Position = new Vector2(400f, 700f);
            electroCardImages[i].Position = new Vector2(400f, 700f);
            goldenCardImages[i].Position = new Vector2(400f, 700f);
        }
        if (handCount is not 0)
        {
            deckButton.Enabled = true;
            circle.Visible = true;
            oscilatingOpacityImageReference = circle;
        }
        CreateCards();
    }

    private async void Decker(object sender, EventArgs e)
    {
        deckButton.Enabled = false;
        deckButton.Image.Visible = false;
        circle.Visible = false;
        if (!goldenCardImages[2].Visible)
        {
            if (sender is not null)
            {
                deckGrabShuffle.Play();
                while (deck.Position.Y < 700)
                {
                    deck.Position += new Vector2(9f, 10f);
                    await Task.Delay(17);
                }
                deckPlace.Play();
            }
            
            deck.DrawOrder = 5;
            cardPlacerButtons[0].Enabled = true;
            cardPlacerButtons[0].Image.Visible = true;
            oscilatingOpacityImageReference = cardPlacerButtons[0].Image;
            return;
        }

        deck.DrawOrder = 9;

        oscilatingOpacityImageReference = null;
        deckGrab.Play();
        while (deck.Position.Y > 400)
        {
            deck.Position += new Vector2(-9f, -10f);
            await Task.Delay(17);
        }
        deckPlace.Play();

        while (yourCardImages[0].Position.Y < 700)
        {
            yourCardImages[0].Position += new Vector2(0f, 5f);
            yourCardImages[1].Position += new Vector2(0f, 5f);
            yourCardImages[2].Position += new Vector2(0f, 5f);
            electroCardImages[0].Position += new Vector2(0f, -10f);
            electroCardImages[1].Position += new Vector2(0f, -10f);
            electroCardImages[2].Position += new Vector2(0f, -10f);
            await Task.Delay(17);
        }

        electroCardImages[0].Visible = false;
        electroCardImages[1].Visible = false;
        electroCardImages[2].Visible = false;

        yourCards[0].IsFaceUp = true;
        yourCards[1].IsFaceUp = true;
        yourCards[2].IsFaceUp = true;
        yourCardImages[0].ChangeAnimatedTexture(yourCards[0].GetTexture().Key, yourCards[0].GetTexture().Value);
        yourCardImages[1].ChangeAnimatedTexture(yourCards[1].GetTexture().Key, yourCards[1].GetTexture().Value);
        yourCardImages[2].ChangeAnimatedTexture(yourCards[2].GetTexture().Key, yourCards[2].GetTexture().Value);

        goldenCards[0].IsFaceUp = true;
        goldenCardImages[0].ChangeAnimatedTexture(goldenCards[0].GetTexture().Key, goldenCards[0].GetTexture().Value);

        ElectroPlayCard();
    }

    private async void PlaceCard(object sender, EventArgs e)
    {
        int index = Array.IndexOf(cardPlacerButtons, sender);
        (sender as Button).Enabled = false;
        (sender as Button).Image.Visible = false;

        cardGrab.Play();
        if (index is 8)
        {
            while (deck.Position.Y > 700f)
            {
                deck.Position += new Vector2(0f, -1f);
                await Task.Delay(17);
            }
            deckButton.Enabled = true;
            deckButton.Image.Visible = true;
            oscilatingOpacityImageReference = deckButton.Image;
        } else {
            cardPlacerButtons[index+1].Enabled = true;
            cardPlacerButtons[index+1].Image.Visible = true;
            oscilatingOpacityImageReference = cardPlacerButtons[index+1].Image;
        }
        
        if (index > 5)
        {
            goldenCardImages[index-6].DrawOrder = 9;
            goldenCardImages[index-6].Visible = true;
            while (goldenCardImages[index-6].Position.Y > 400f)
            {
                goldenCardImages[index-6].Position += new Vector2(4.5f * (index-7), -10f);
                await Task.Delay(17);
            }
            goldenCardImages[index-6].DrawOrder = 7;
        } else if (index % 2 == 0)
        {
            electroCardImages[index/2].DrawOrder = 9;
            electroCardImages[index/2].Visible = true;
            while (electroCardImages[index/2].Position.Y > 250) {
                electroCardImages[index/2].Position += new Vector2(3f * (index/2-1), -10f);
                await Task.Delay(17);
            }
            electroCardImages[index/2].DrawOrder = 6;
        } else
        {
            yourCardImages[index/2].DrawOrder = 9;
            yourCardImages[index/2].Visible = true;
            while (yourCardImages[index/2].Position.Y > 550)
            {
                yourCardImages[index/2].Position += new Vector2(9f * ((index-1)/2-1), -10f);
                await Task.Delay(17);
            }
            yourCardImages[index/2].DrawOrder = 8;
        }
        cardPlace.Play();
    }

    private async void ElectroHandOut()
    {
        deck.DrawOrder = 5;
        deckGrabShuffle.Play();
        while (deck.Position.Y > 100)
        {
            deck.Position += new Vector2(9f, -10f);
            await Task.Delay(17);
        }
        deckPlace.Play();
        while (deck.Position.Y < 108)
        {
            deck.Position += new Vector2(0f, 1f);
            await Task.Delay(17);
        }
        for (int i = 0; i < 3; i++)
        {
            yourCardImages[i].Position = new Vector2(400f, 100f);
            electroCardImages[i].Position = new Vector2(400f, 100f);
            goldenCardImages[i].Position = new Vector2(400f, 100f);
        }
        for (int i = 0; i < 3; i++)
        {
            yourCardImages[i].Visible = true;
            int oldPos = yourCardImages[i].DrawOrder;
            yourCardImages[i].DrawOrder = 9;
            cardGrab.Play();
            while (yourCardImages[i].Position.Y < 550)
            {
                yourCardImages[i].Position += new Vector2(3f * (i-1), 10f);
                await Task.Delay(17);
            }
            cardPlace.Play();
            yourCardImages[i].DrawOrder = oldPos;
            electroCardImages[i].Visible = true;
            oldPos = electroCardImages[i].DrawOrder;
            electroCardImages[i].DrawOrder = 9;
            cardGrab.Play();
            while (electroCardImages[i].Position.Y < 250) {
                electroCardImages[i].Position += new Vector2(9 * (i-1), 10f);
                await Task.Delay(17);
            }
            cardPlace.Play();
            electroCardImages[i].DrawOrder = oldPos;
        }
        for (int i = 0; i < 3; i++)
        {
            goldenCardImages[i].Visible = true;
            cardGrab.Play();
            while (goldenCardImages[i].Position.Y < 400)
            {
                goldenCardImages[i].Position += new Vector2(4.5f * (i-1), 10f);
                await Task.Delay(17);
            }
            cardPlace.Play();
        }
        deck.Position = new Vector2(400f, 100f);
        deck.Visible = true;
        deck.DrawOrder = 9;
        deckGrab.Play();
        if (handCount is not 5)
        {
            while (deck.Position.Y < 400)
            {
                deck.Position += new Vector2(-9f, 10f);
                await Task.Delay(17);
            }
            deckPlace.Play();
            deck.DrawOrder = 8;
        }
        else
        {
            while (deck.Position.Y > -70)
            {
                deck.Position += new Vector2(0, -10f);
                await Task.Delay(17);
            }
            deck.Visible = false;
        }

        if (handCount != 3 && handCount != 5)
            CreateCards();
        else
            CheatedCreateCards();

        while (yourCardImages[0].Position.Y < 700)
        {
            yourCardImages[0].Position += new Vector2(0f, 5f);
            yourCardImages[1].Position += new Vector2(0f, 5f);
            yourCardImages[2].Position += new Vector2(0f, 5f);
            electroCardImages[0].Position += new Vector2(0f, -10f);
            electroCardImages[1].Position += new Vector2(0f, -10f);
            electroCardImages[2].Position += new Vector2(0f, -10f);
            await Task.Delay(17);
        }

        electroCardImages[0].Visible = false;
        electroCardImages[1].Visible = false;
        electroCardImages[2].Visible = false;

        yourCards[0].IsFaceUp = true;
        yourCards[1].IsFaceUp = true;
        yourCards[2].IsFaceUp = true;
        yourCardImages[0].ChangeAnimatedTexture(yourCards[0].GetTexture().Key, yourCards[0].GetTexture().Value);
        yourCardImages[1].ChangeAnimatedTexture(yourCards[1].GetTexture().Key, yourCards[1].GetTexture().Value);
        yourCardImages[2].ChangeAnimatedTexture(yourCards[2].GetTexture().Key, yourCards[2].GetTexture().Value);
        


        goldenCards[0].IsFaceUp = true;
        goldenCardImages[0].ChangeAnimatedTexture(goldenCards[0].GetTexture().Key, goldenCards[0].GetTexture().Value);
    
        yourCardButtons[0].Enabled = true;
        yourCardButtons[1].Enabled = true;
        yourCardButtons[2].Enabled = true;
    }

    private void CreateCards()
    {   
        yourCards = new CardData[3];
        electroCards = new CardData[3];
        goldenCards = new CardData[3];
        for (int i = 0; i < 3; i++)
        {
            Y:
            var y = CardData.CreateRandom(false);
            if (yourCards.Any(x => x.Value == y.Value && x.Suit == y.Suit) || electroCards.Any(x => x.Value == y.Value && x.Suit == y.Suit))
                goto Y;
            yourCards[i] = y;
            E:
            var e = CardData.CreateRandom(false);
            if (yourCards.Any(x => x.Value == e.Value && x.Suit == e.Suit) || electroCards.Any(x => x.Value == e.Value && x.Suit == e.Suit))
                goto E;
            electroCards[i] = e;
        }
        electroCards = electroCards.OrderBy(x => x.Value).ToArray();
        for (int i = 0; i < 3; i++)
        {
            G:
            var g = CardData.CreateRandom(true);
            if (yourCards.Any(x => x.Value == g.Value && x.Suit == g.Suit) || electroCards.Any(x => x.Value == g.Value && x.Suit == g.Suit) || goldenCards.Any(x => x.Value == g.Value && x.Suit == g.Suit))
                goto G;
            goldenCards[i] = g;
        }
        if (handCount is 0) dejavuCards = yourCards.Clone() as CardData[];
    }
    
    private void CheatedCreateCards()
    {
        yourCards = handCount is 5 ? dejavuCards : new CardData[3];
        if (handCount is not 5)
        {
            for (int i = 0; i < 3; i++)
            {
                G:
                var g = CardData.CreateRandom(true);
                if (goldenCards.Any(x => x.Value == g.Value && x.Suit == g.Suit) || yourCards.Any(x => x.Value == g.Value && x.Suit == g.Suit))
                    goto G;
                goldenCards[i] = g;
            }
        } 
        else
        {
            for (int i = 0; i < 3; i++)
            {
                int value = 1;
                H:
                var g = new CardData(value, goldenCards[i].Suit, true);
                if (goldenCards.Any(x => x.Value == g.Value && x.Suit == g.Suit) || yourCards.Any(x => x.Value == g.Value && x.Suit == g.Suit))
                {
                    if (value is 1)
                        value = 14;
                    value--;
                    goto H;
                }
                goldenCards[i] = g;
            }
        }
        electroCards = new CardData[3];
        for (int i = 0; i < 3; i++)
        {
            int value = i is 1 && handCount is 5 ? 2 : 13;
            CardData.Suits suit = i is 1 && handCount is 5 ? (CardData.Suits)Random.Shared.Next(0, 4) : goldenCards[i].Suit;
            E:
            if (goldenCards.Any(x => x.Value == value && x.Suit == suit) || yourCards.Any(x => x.Value == value && x.Suit == suit) || electroCards.Any(x => x.Value == value && x.Suit == suit))
            {
                if (i is 1 && handCount is 5) value++; else value--;
                goto E;
            }
            electroCards[i] = new CardData(value, suit, false);
        }
        if (handCount is not 5)
        {
            for (int i = 0; i < 3; i++)
            {
                Y:
                var y = CardData.CreateRandom(false);
                if (yourCards.Any(x => x.Value == y.Value && x.Suit == y.Suit) || electroCards.Any(x => x.Value == y.Value && x.Suit == y.Suit) || goldenCards.Any(x => x.Value == y.Value && x.Suit == y.Suit))
                    goto Y;
                yourCards[i] = y;
            }
        }
        electroCards = electroCards.OrderBy(x => x.Value).ToArray();
    }

    private async void PlayCard(object sender, EventArgs e)
    {
        yourCardButtons[0].Enabled = false;
        yourCardButtons[1].Enabled = false;
        yourCardButtons[2].Enabled = false;
        yourPlayedCard = yourCardButtons.ToList().IndexOf(sender as Button);
        yourCards[yourPlayedCard].Used = true;

        cardGrab.Play();
        while (yourCardImages[yourPlayedCard].Position.Y > 550f)
        {
            yourCardImages[yourPlayedCard].Position += new Vector2(9f * (roundCount - yourPlayedCard), -10f);
            await Task.Delay(17);
        }
        cardPlace.Play();

        if (electroPlayedCard is -1)
            ElectroPlayCard();
        else
            EvaluateRound();
    }

    private async void ElectroPlayCard()
    {
        electroPlayedCard = Array.IndexOf(electroCards, electroCards.FirstOrDefault((x => x.Suit == goldenCards[roundCount].Suit && !x.Used), electroCards.First(x => !x.Used)));
        electroCards[electroPlayedCard].IsFaceUp = true;
        electroCardImages[electroPlayedCard].ChangeAnimatedTexture(electroCards[electroPlayedCard].GetTexture().Key, electroCards[electroPlayedCard].GetTexture().Value);
        electroCardImages[electroPlayedCard].Position = new Vector2(400f, 100f);
        electroCardImages[electroPlayedCard].Visible = true;
        electroCards[electroPlayedCard].Used = true;
        
        cardGrab.Play();
        while (electroCardImages[electroPlayedCard].Position.Y < 250f)
        {
            electroCardImages[electroPlayedCard].Position += new Vector2(9f * (roundCount - 1), 10f);
            await Task.Delay(17);
        }
        cardPlace.Play();

        if (yourPlayedCard is -1)
        {
            for (int i = 0; i < 3; i++)
                if (!yourCards[i].Used)
                    yourCardButtons[i].Enabled = true;
        }
        else
            EvaluateRound();
    }

    private void EvaluateRound()
    {
        if (yourCards[yourPlayedCard].Value == electroCards[electroPlayedCard].Value && yourCards[yourPlayedCard].Suit != goldenCards[roundCount].Suit && electroCards[electroPlayedCard].Suit != goldenCards[roundCount].Suit)
        {
            HandleScore(handCount % 2 == 1);
            return;
        }    

        int yourValue = yourCards[yourPlayedCard].Points + (yourCards[yourPlayedCard].Suit == goldenCards[roundCount].Suit ? 100 : 0);
        int electroValue = electroCards[electroPlayedCard].Points + (electroCards[electroPlayedCard].Suit == goldenCards[roundCount].Suit ? 100 : 0);
        
        HandleScore(yourValue > electroValue);
    }

    private void HandleScore(bool youWon)
    {
        if (youWon)
        {
            yourCards[yourPlayedCard].Won = true;
            yourCardImages[yourPlayedCard].ChangeAnimatedTexture(yourCards[yourPlayedCard].GetTexture().Key, yourCards[yourPlayedCard].GetTexture().Value);
            if (yourRounds)
            {
                yourPoints += goldenCards[0].Points + goldenCards[1].Points + (goldenCards[2].IsFaceUp ? goldenCards[2].Points : 0);
                EndRound(true);
                return;
            }
            else
                yourRounds = true;
        } else
        {
            electroCards[electroPlayedCard].Won = true;
            electroCardImages[electroPlayedCard].ChangeAnimatedTexture(electroCards[electroPlayedCard].GetTexture().Key, electroCards[electroPlayedCard].GetTexture().Value);
            if (electroRounds)
            {
                electroPoints += goldenCards[0].Points + goldenCards[1].Points + (goldenCards[2].IsFaceUp ? goldenCards[2].Points : 0);
                EndRound(false);
                return;
            }
            else
                electroRounds = true;
        }
        
        roundCount++;
        yourPlayedCard = -1;
        electroPlayedCard = -1;
        goldenCards[roundCount].IsFaceUp = true;
        goldenCardImages[roundCount].ChangeAnimatedTexture(goldenCards[roundCount].GetTexture().Key, goldenCards[roundCount].GetTexture().Value);
        if (youWon)
        {
            for (int i = 0; i < 3; i++)
                if (!yourCards[i].Used)
                    yourCardButtons[i].Enabled = true;
        }
        else
            ElectroPlayCard();
    }

    private async void EndRound(bool youWon)
    {
        plus1.Visible = true;
        plus2.Visible = goldenCards[2].IsFaceUp;
        equal.Visible = true;
        roundResult.Text = $"{goldenCards[0].Points + goldenCards[1].Points + (goldenCards[2].IsFaceUp ? goldenCards[2].Points : 0)}";
        roundResult.Visible = true;

        await Task.Delay(3000);

        bg.Visible = true;
        table.Visible = true;
        yourPaper.Visible = true;
        electroPaper.Visible = true;
        yourPointsLabel.Visible = true;
        electroPointsLabel.Visible = true;
        electro20.Visible = true;
        electro20.ChangeAnimatedTexture(idle, null);
        while (gameBg.Opacity > 0f)
        {
            if (roundResult.Position.Y != 400f + 250f * (youWon ? 1 : -1))
                roundResult.Position += new Vector2(0f, 5f * (youWon ? 1 : -1));

            gameBg.Opacity -= 0.02f;
            deck.Opacity -= 0.02f;
            yourCardImages[0].Opacity -= 0.02f;
            yourCardImages[1].Opacity -= 0.02f;
            yourCardImages[2].Opacity -= 0.02f;
            electroCardImages[0].Opacity -= 0.02f;
            electroCardImages[1].Opacity -= 0.02f;
            electroCardImages[2].Opacity -= 0.02f;
            goldenCardImages[0].Opacity -= 0.02f;
            goldenCardImages[1].Opacity -= 0.02f;
            goldenCardImages[2].Opacity -= 0.02f;
            plus1.Opacity -= 0.02f;
            plus2.Opacity -= 0.02f;
            equal.Opacity -= 0.02f;
            await Task.Delay(17);
        }

        gameBg.Visible = false;
        deck.Visible = false;
        yourCardImages[0].Visible = false;
        yourCardImages[1].Visible = false;
        yourCardImages[2].Visible = false;
        electroCardImages[0].Visible = false;
        electroCardImages[1].Visible = false;
        electroCardImages[2].Visible = false;
        goldenCardImages[0].Visible = false;
        goldenCardImages[1].Visible = false;
        goldenCardImages[2].Visible = false;
        plus1.Visible = false;
        plus2.Visible = false;
        equal.Visible = false;
        roundResult.Visible = false;

        yourPointsLabel.Text = yourPoints.ToString();
        electroPointsLabel.Text = electroPoints.ToString();

        if (youWon)
        {
            vo_lose[handCount].Play();
            ccText.Text = cc_lose[handCount];
            electro20.ChangeAnimatedTexture(texture_lose[handCount], null);
            await Task.Delay(vo_lose[handCount].Duration);
        }
        else 
        {
            vo_win[handCount].Play();
            ccText.Text = cc_win[handCount];
            electro20.ChangeAnimatedTexture(texture_win[handCount], null);
            await Task.Delay(vo_win[handCount].Duration);
        }
        ccText.Text = "";

        if (handCount is 5)
        {
            OutroSequence();
            return;
        }

        handCount++;
        ResetTable();
        gameBg.Visible = true;
        deck.Visible = true;
        while (gameBg.Opacity < 1f)
        {
            gameBg.Opacity += 0.02f;
            deck.Opacity += 0.02f;
            await Task.Delay(17);
        }
        bg.Visible = false;
        table.Visible = false;
        electro20.Visible = false;
        yourPaper.Visible = false;
        electroPaper.Visible = false;
        yourPointsLabel.Visible = false;
        electroPointsLabel.Visible = false;
        StateMachineHandler();
    }

    private void ResetTable()
    {
        roundResult.Visible = false;
        roundResult.Position = new Vector2(670f, 400f);
        deck.Position = new Vector2(130f, 400f);
        yourCardImages[0].Opacity = 1f;
        yourCardImages[1].Opacity = 1f;
        yourCardImages[2].Opacity = 1f;
        electroCardImages[0].Opacity = 1f;
        electroCardImages[1].Opacity = 1f;
        electroCardImages[2].Opacity = 1f;
        goldenCardImages[0].Opacity = 1f;
        goldenCardImages[1].Opacity = 1f;
        goldenCardImages[2].Opacity = 1f;
        plus1.Opacity = 1f;
        plus2.Opacity = 1f;
        equal.Opacity = 1f;
        yourCardImages[0].ChangeTexture(back);
        yourCardImages[1].ChangeTexture(back);
        yourCardImages[2].ChangeTexture(back);
        electroCardImages[0].ChangeTexture(back);
        electroCardImages[1].ChangeTexture(back);
        electroCardImages[2].ChangeTexture(back);
        goldenCardImages[0].ChangeTexture(back);
        goldenCardImages[1].ChangeTexture(back);
        goldenCardImages[2].ChangeTexture(back);
        yourRounds = false;
        electroRounds = false;
        yourPlayedCard = -1;
        electroPlayedCard = -1;
        roundCount = 0;
    }

    private async void OutroSequence()
    {
        vo[8].Play();
        ccText.Text = "Come on, hand out the last round...";
        electro20.ChangeAnimatedTexture(laugh, null);
        await Task.Delay(vo[8].Duration);
        ccText.Text = "";
        gameBg.Visible = true;
        while (gameBg.Opacity < 1f)
        {
            gameBg.Opacity += 0.02f;
            await Task.Delay(17);
        }
        circle.Visible = true;
        oscilatingOpacityImageReference = circle;
        yourPaper.Visible = false;
        electroPaper.Visible = false;
        yourPointsLabel.Visible = false;
        electroPointsLabel.Visible = false;

        await Task.Delay(1500);
        MediaPlayer.Stop();
        await Task.Delay(1500);

        vo[9].Play();
        ccText.Text = "Where did the deck go?";
        electro20.ChangeAnimatedTexture(sus, null);
        await Task.Delay(vo[9].Duration);
        ccText.Text = "";
        circle.Visible = false;
        oscilatingOpacityImageReference = null;

        while (gameBg.Opacity > 0f)
        {
            gameBg.Opacity -= 0.02f;
            await Task.Delay(17);
        }

        await Task.Delay(1000);

        MediaPlayer.Play(conflict);
        vo[10].Play();
        ccText.Text = "Don't look at me, I didn't take it!";
        electro20.ChangeAnimatedTexture(angry, null);
        await Task.Delay(vo[10].Duration);

        electroCardImages[0].ChangeTexture(back);
        electroCardImages[0].Position = new Vector2(360f, 400f);
        electroCardImages[0].Opacity = 1f;
        electroCardImages[0].Visible = true;
        cardGrab.Play();
        while (electroCardImages[0].Position.Y < 500f)
        {
            electroCardImages[0].Position += new Vector2(0f, 10f);
            await Task.Delay(17);
        }

        await Task.Delay(500);

        vo[11].Play();
        ccText.Text = "Woah, where did that come from?";
        electro20.ChangeAnimatedTexture(sus, null);
        await Task.Delay(vo[11].Duration + TimeSpan.FromMilliseconds(500));

        vo[12].Play();
        ccText.Text = "WAIT, I'M NOT CHEATING, I SWEAR!";
        electro20.ChangeAnimatedTexture(nervous, null);
        await Task.Delay(vo[12].Duration);

        deck.Position = new Vector2(440f, 400f);
        deck.Opacity = 1f;
        deck.Visible = true;
        deckGrabShuffle.Play();
        electro20.ChangeAnimatedTexture(inverted, null);
        while (deck.Position.Y < 500f)
        {
            deck.Position += new Vector2(0f, 20f);
            await Task.Delay(17);
        }

        await Task.Delay(3000);

        chorizo.Visible = true;

        while (chorizo.Position.X > 700)
        {
            await Task.Delay(17);
        }

        vo[13].Play();
        ccText.Text = "NO, CHORIZO NO!";
        electro20.ChangeAnimatedTexture(nervous, null);
        await Task.Delay(vo[13].Duration);
        
        vo[14].Play();
        ccText.Text = "OK, I ADMIT IT, I CHEATED,\nI CHEATED, STOP, PLEASE!";
        electro20.ChangeAnimatedTexture(cry, null);

        while (chorizo.Position.X > electro20.Position.X)
        {
            electro20.Position += new Vector2(-0.5f, 0f);
            await Task.Delay(17);
        }

        explosion.Play();
        exploeffect.Animation.Start();
        exploeffect.Visible = true;

        await Task.Delay(289);

        SwitchState(new CreditsState(Game));
    }
}