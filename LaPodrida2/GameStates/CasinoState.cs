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
    private SimpleImage bg;
    private SimpleImage table;
    private SimpleImage deck;
    private SimpleImage map;
    private SimpleImage gameBg;
    private TextComponent ccText;
    private CardData?[] electroCards = new CardData?[3];
    private SimpleImage[] electroCardImages = new SimpleImage[3];
    private CardData[] goldenCards = new CardData[3];
    private SimpleImage[] goldenCardImages = new SimpleImage[3];
    private CardData[] yourCards = new CardData[3];
    private SimpleImage[] yourCardImages = new SimpleImage[3];
    private CardData? yourPlayedCard;
    private CardData? electroPlayedCard;
    private bool yourPoints = false;
    private bool electroPoints = false;
    private bool yourTurn = true;
    private int roundCount = 0;
    private int handCount = 0;
    private Song bgm;
    private Button[] yourCardButtons = new Button[3];
    private Button deckButton;
    private Button[] cardPlacerButtons = new Button[9];
    private SimpleImage oscilatingOpacityImageReference;
    private bool goingUp = false;


    public CasinoState(Game game) : base(game) {}

    public override void LoadContent()
    {
        #region Textures
        Texture2D back = Game.Content.Load<Texture2D>(@"Textures\Cards\back_red");
        CardData.LoadTextures(Game.Content, true);
        #endregion

        #region SimpleImages
        bg = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\bg"), new Vector2(0f, 800f), 0, anchor: Alignment.TopLeft);
        table = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\Table"), new Vector2(400f, 800f), 0, anchor: Alignment.TopCenter, scale: 1.5f);
        deck = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\deck_red"), new Vector2(400f, -200f), 8);
        gameBg = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\gameBg"), Vector2.Zero, 1, anchor: Alignment.TopLeft, visible: false, /*animation: Animation<Rectangle>.TextureAnimation(new Point(800, 800), new Point(1600, 800), true, 30),*/ opacity: 0f);
        map = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\map"), new Vector2(0f, 800f), 0, anchor: Alignment.TopLeft, visible: true, animation: Animation<Rectangle>.TextureAnimation(new Point(800, 600), new Point(800, 1200), true, 30));
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
        /*
        vo1 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo1");
        vo2 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo2");
        vo3 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo3");
        vo4 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo4");
        vo5 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo5");
        vo6 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo6");
        vo7 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo7");
        vo8 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo8");
        vo9 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo9");
        vo10 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo10");
        vo11 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo11");
        vo12 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo12");
        vo13 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo13");
        vo14 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo14");
        vo15 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo15");
        vo16 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo16");
        vo17 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo17");
        vo18 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo18");
        vo19 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo19");
        vo20 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo20");
        vo21 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo21");
        */
        MediaPlayer.Play(Game.Content.Load<Song>(@"Audio\Casino\street"));
        bgm = Game.Content.Load<Song>(@"Audio\Casino\bgm");
        #endregion

        #region TextComponents
        ccText = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\consolas"), "", new Vector2(400f, 750f), 10, anchor: Alignment.Center);
        _components.Add(ccText);
        #endregion
    }

    public override void UnloadContent()
    {
        // LA PODRIDAAAAAAAAAAAAAAAAA (2)
        throw new NotImplementedException();
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

        base.Update(gameTime);
    }

    private async void IntroSequence()
    {
        while (bg.Position.Y > 0)
        {
            bg.Position += new Vector2(0f, -5f);
            await Task.Delay(17);
        }

        map.Animation.Start();
        while (map.Position.Y > 100f)
        {
            map.Position += new Vector2(0f, -5f);
            await Task.Delay(17);
        }
        map.Animation.Paused = false;

        await Task.Delay(5000); //DEBUG

        while (map.Position.Y < 800f)
        {
            map.Position += new Vector2(0f, 5f);
            await Task.Delay(17);
        }
        map.Visible = false;

        while (table.Position.Y > 450)
        {
            table.Position += new Vector2(0f, -5f);
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
            await Task.Delay(17);
        }
        deck.Position = new Vector2(400f, 708f);

        MediaPlayer.Play(bgm);
        await Task.Delay(500);
        ccText.Position = new Vector2(400f, 50f);
        StateMachineHandler();
    }

    private void StateMachineHandler()
    {
        Game.Window.Title = $"La Podrida 2 - Hand {handCount+1}";
        if (handCount % 2 == 0)
        {
            YourHandOut();
        } else if (handCount == 3)
        {

        } else if (handCount == 5)
        {

        } else 
        {
            ElectroHandOut();
        }
    }

    private void YourHandOut()
    {
        for (int i = 0; i < 3; i++)
        {
            yourCardImages[i].Visible = true;
            yourCardImages[i].Position = new Vector2(400f, 700f);
            electroCardImages[i].Visible = true;
            electroCardImages[i].Position = new Vector2(400f, 700f);
            goldenCardImages[i].Visible = true;
            goldenCardImages[i].Position = new Vector2(400f, 700f);
        }
        deckButton.Enabled = true;
        //oscilatingOpacityImageReference = deckButton.Image;
        CreateCards();
    }

    private void Decker(object sender, EventArgs e)
    {
        deckButton.Enabled = false;
        deckButton.Image.Visible = false;
        if (!goldenCardImages[2].Visible)
        {
            deck.Visible = false;
            cardPlacerButtons[0].Enabled = true;
            cardPlacerButtons[0].Image.Visible = true;
            oscilatingOpacityImageReference = cardPlacerButtons[0].Image;
            return;
        }

        oscilatingOpacityImageReference = null;
        deck.Visible = true;
    }

    private void PlaceCard(object sender, EventArgs e)
    {
        int index = Array.IndexOf(cardPlacerButtons, sender);
        (sender as Button).Enabled = false;
        (sender as Button).Image.Visible = false;

        if (index is 8)
        {
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
            goldenCardImages[index-6].Visible = true;
            goldenCardImages[index-6].Position = new Vector2(265f + (index-6) * 135, 400f);
        } else if (index % 2 == 0)
        {
            electroCardImages[index/2].Visible = true;
            electroCardImages[index/2].Position = new Vector2(265f + (index/2) * 135, 250f);
        } else
        {
            yourCardImages[index/2].Visible = true;
            yourCardImages[index/2].Position = new Vector2(265f + (index/2) * 135, 550f);
        }
    }

    private async void ElectroHandOut()
    {
        for (int i = 0; i < 3; i++)
        {
            yourCardImages[i].Visible = true;
            while (yourCardImages[i].Position.Y < 550)
            {
                yourCardImages[i].Position += new Vector2(3f * (i-1), 10f);
                await Task.Delay(17);
            }
            electroCardImages[i].Visible = true;
            while (electroCardImages[i].Position.Y < 250) {
                electroCardImages[i].Position += new Vector2(9 * (i-1), 10f);
                await Task.Delay(17);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            goldenCardImages[i].Visible = true;
            while (goldenCardImages[i].Position.Y < 400)
            {
                goldenCardImages[i].Position += new Vector2(4.5f * (i-1), 10f);
                await Task.Delay(17);
            }
        }
        deck.Position = new Vector2(400f, 100f);
        deck.Visible = true;
        while (deck.Position.Y < 400)
        {
            deck.Position += new Vector2(-9f, 10f);
            await Task.Delay(17);
        }

        CreateCards();
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
        yourTurn = true;
    }

    private void CreateCards()
    {   
        for (int i = 0; i < 3; i++)
        {
            Y:
            var y = CardData.CreateRandom(false);
            if (yourCards.Any(x => x.Value == y.Value && x.Suit == y.Suit) || electroCards.Any(x => x?.Value == y.Value && x?.Suit == y.Suit))
                goto Y;
            yourCards[i] = y;
            E:
            var e = CardData.CreateRandom(false);
            if (yourCards.Any(x => x.Value == e.Value && x.Suit == e.Suit) || electroCards.Any(x => x?.Value == e.Value && x?.Suit == e.Suit))
                goto E;
            electroCards[i] = e;
        }
        electroCards = electroCards.OrderBy(x => x?.Value).ToArray();
        for (int i = 0; i < 3; i++)
        {
            G:
            var g = CardData.CreateRandom(true);
            if (yourCards.Any(x => x.Value == g.Value && x.Suit == g.Suit) || electroCards.Any(x => x?.Value == g.Value && x?.Suit == g.Suit) || goldenCards.Any(x => x.Value == g.Value && x.Suit == g.Suit))
                goto G;
            goldenCards[i] = g;
        }
    }

    private void PlayCard(object sender, EventArgs e)
    {
        (sender as Button).Enabled = false;
        int cardIndex = yourCardButtons.ToList().IndexOf(sender as Button);

        if (electroPlayedCard is null)
            ElectroPlayCard();

    }

    private void ElectroPlayCard()
    {
        CardData c = (CardData)electroCards.FirstOrDefault((x => x?.Suit == goldenCards[roundCount].Suit), electroCards.First(x => x is not null));
        int index = Array.IndexOf(electroCards, c as CardData?);
        c.IsFaceUp = true;
        electroCardImages[index].Position = new Vector2(265 + 135 * roundCount, 250f);
        electroCards[index] = null;
        electroPlayedCard = c;
    }

    private void EvaluateRound()
    {
        
    }
}