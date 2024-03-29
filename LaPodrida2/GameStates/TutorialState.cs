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

public class TutorialState : GameState
{
    #region Fields
    private Texture2D back;
    private Texture2D angry;
    private Texture2D clock;
    private Texture2D idle;
    private Texture2D musculito;
    private Texture2D sus;
    private SimpleImage bg;
    private SimpleImage table;
    private SimpleImage deck;
    private SimpleImage tutoBg;
    private SimpleImage electro20;
    private SimpleImage circle;
    private TextComponent ccText;
    private CardData[] electroCards = new CardData[3];
    private SimpleImage[] electroCardImages = new SimpleImage[3];
    private CardData[] goldenCards = new CardData[3];
    private SimpleImage[] goldenCardImages = new SimpleImage[3];
    private CardData[] yourCards = new CardData[3];
    private SimpleImage[] yourCardImages = new SimpleImage[3];
    private int yourPlayedCard = -1;
    private int electroPlayedCard = -1;
    private bool yourRounds = false;
    private bool electroRounds = false;
    private int roundCount = 0;
    private int handCount = 0;
    private SimpleImage plus1;
    private SimpleImage plus2;
    private SimpleImage equal;
    private SoundEffect cardGrab;
    private SoundEffect cardPlace;
    private SoundEffect deckGrab;
    private SoundEffect deckGrabShuffle;
    private SoundEffect deckPlace;
    private TextComponent roundResult;
    private Button[] yourCardButtons = new Button[3];
    private Button deckButton;
    private Button[] cardPlacerButtons = new Button[9];
    private Song bgm;
    private SimpleImage oscilatingOpacityImageReference;
    private bool goingUp = false;
    private SoundEffect[] vo = new SoundEffect[21];
    /*
    00: "What do you mean you don't know how to play?"
    01: "The tournament starts in an hour..."
    02: "I guess there's time, I'll give you a little tutorial."
    03: "Yes, this is my house and this is my table!"
    04: "If you don't like the stock logo you can leave!"
    05: "Well, it starts easy, each player gets 3 cards."
    06: "Then 3 cards are placed in the middle."
    07: "Now the first of the cards in the middle can be flipped."
    08: "Those cards, as you can see, are the golden cards."
    09: "The game consists of a best of 3, the biggest card wins the round."
    10: "Now, all cards from the suit of the golden card\nhave higher values than the other suits."
    11: "Come on, for example, play a card."
    12: "Now, I'll play a card."
    13: "As you can see, the golden card has a higher value than the other suits."
    14: "And, as you might have discovered, there's a golden card per round."
    15: "Let's keep on playing."
    16: "Now that the hand's over, the winner gets the amount\nof points from the values of the golden cards."
    17: "An ace has a point value of 20."
    18: "Now, let's play another hand.\nGrab the deck and hand out the cards."
    19: "Well done, a game consists of 7 hands,\nthe player with the most points after that wins."
    20: "Let's get going to the casino,\nelse we're gonna be there late!"
    */
    #endregion

    public TutorialState(Game game) : base(game) {}

    public override void LoadContent()
    {
        #region Textures
        back = Game.Content.Load<Texture2D>(@"Textures\Cards\back_blue");
        CardData.LoadTextures(Game.Content, false);
        angry = Game.Content.Load<Texture2D>(@"Textures\Electro20\angry");
        clock = Game.Content.Load<Texture2D>(@"Textures\Electro20\clock");
        idle = Game.Content.Load<Texture2D>(@"Textures\Electro20\idle");
        musculito = Game.Content.Load<Texture2D>(@"Textures\Electro20\musculito");
        sus = Game.Content.Load<Texture2D>(@"Textures\Electro20\sus");
        #endregion

        #region SimpleImages
        bg = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Tutorial\bg"), new Vector2(0f, 800f), 0, anchor: Alignment.TopLeft);
        table = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Tutorial\Table"), new Vector2(400f, 800f), 2, anchor: Alignment.TopCenter, scale: 1.2f);
        deck = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\deck_blue"), new Vector2(400f, -200f), 8);
        tutoBg = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Tutorial\tablebg"), Vector2.Zero, 3, anchor: Alignment.TopLeft, visible: false, animation: Animation<Rectangle>.TextureAnimation(new Point(800, 800), new Point(1600, 800), true, 30), opacity: 0f);
        electro20 = new SimpleImage(Game, idle, new Vector2(400f, 1300f), 1, animation: Animation<Rectangle>.TextureAnimation(new Point(500), new Point(1000, 500), true, 30));
        circle = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\circle"), new Vector2(130f, 400f), 9, visible: false);
        plus1 = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\plus"), new Vector2(333f, 400f), 10, visible: false, scale: .5f, color: new Color(0x00, 0x97, 0x9D), animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30));
        plus2 = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\plus"), new Vector2(467f, 400f), 10, visible: false, scale: .5f, color: new Color(0x00, 0x97, 0x9D), animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30));
        equal = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\equals"), new Vector2(602f, 400f), 10, visible: false, scale: .5f, color: new Color(0x00, 0x97, 0x9D), animation: Animation<Rectangle>.TextureAnimation(new Point(135), new Point(270, 135), true, 30));
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
        _components.Add(tutoBg);
        _components.Add(electro20);
        _components.Add(circle);
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
        deckButton = new Button(Game, new Rectangle(130, 400, 88, 140), enabled: false, hasHover: false, texture: new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\deck_blue"), new Vector2(130f, 400f), 4, false));
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
        for (int i = 0; i < 21; i++)
            vo[i] = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo" + (i + 1));
        bgm = Game.Content.Load<Song>(@"Audio\Tutorial\bgm");
        #endregion

        #region TextComponents
        roundResult = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\romanAlexander"), "", new Vector2(670f, 400f), 10, anchor: Alignment.Center, color: new Color(0x00, 0x97, 0x9D), visible: false);
        ccText = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\consolas"), "", new Vector2(400f, 750f), 10, anchor: Alignment.Center);
        _components.Add(ccText);
        _components.Add(roundResult);
        #endregion
    }

    public override void UnloadContent()
    {
        // LA PODRIDAAAAAAAAAAAAAAAAA (2)
        #region Textures
        Game.Content.UnloadAsset(@"Textures\Cards\back_blue");
        CardData.UnloadTextures(Game.Content);
        Game.Content.UnloadAsset(@"Texture\Electro20\angry");
        Game.Content.UnloadAsset(@"Textures\Electro20\clock");
        Game.Content.UnloadAsset(@"Textures\Electro20\idle");
        Game.Content.UnloadAsset(@"Textures\Electro20\musculito");
        Game.Content.UnloadAsset(@"Textures\Electro20\sus");
        #endregion
    
        #region SimpleImages
        Game.Content.UnloadAsset(@"Textures\Tutorial\bg");
        Game.Content.UnloadAsset(@"Textures\Tutorial\table");
        Game.Content.UnloadAsset(@"Textures\Tutorial\deck_blue");
        Game.Content.UnloadAsset(@"Textures\Tutorial\tablebg");
        Game.Content.UnloadAsset(@"Textures\Cards\circle");
        Game.Content.UnloadAsset(@"Textures\Cards\plus");
        Game.Content.UnloadAsset(@"Textures\Cards\equals");
        #endregion
    
        #region Audios
        Game.Content.UnloadAsset(@"Audio\cardGrab");
        Game.Content.UnloadAsset(@"Audio\cardPlace");
        Game.Content.UnloadAsset(@"Audio\deckGrab");
        Game.Content.UnloadAsset(@"Audio\deckGrabShuffle");
        Game.Content.UnloadAsset(@"Audio\deckPlace");
        for (int i = 0; i < 21; i++)
            Game.Content.UnloadAsset(@"Audio\Tutorial\vo" + (i + 1));
        Game.Content.UnloadAsset(@"Audio\Tutorial\bgm");
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
        
        vo[0].Play();
        ccText.Text = "What do you mean you don't know how to play?";
        electro20.ChangeAnimatedTexture(sus, null);
        await Task.Delay(vo[0].Duration);
        vo[1].Play();
        ccText.Text = "The tournament starts in an hour...";
        electro20.ChangeAnimatedTexture(clock, null);
        await Task.Delay(vo[1].Duration);
        vo[2].Play();
        ccText.Text = "I guess there's time, I'll give you a little tutorial.";
        electro20.ChangeAnimatedTexture(musculito, null);
        await Task.Delay(vo[2].Duration);
        ccText.Text = "";

        while (table.Position.Y > 450)
        {
            table.Position += new Vector2(0f, -5f);
            electro20.Position += new Vector2(0f, -2f);
            await Task.Delay(17);
        }

        vo[3].Play();
        ccText.Text = "Yes, this is my house and this is my table!";
        Game.Window.Title = "La Podrida 2 - Electro20's House";
        electro20.ChangeAnimatedTexture(angry, null);
        await Task.Delay(vo[3].Duration);
        vo[4].Play();
        ccText.Text = "If you don't like the stock logo you can leave!";
        await Task.Delay(vo[4].Duration);
        ccText.Text = "";

        while (deck.Position.Y < 500)
        {
            deck.Position += new Vector2(0f, 50f);
            await Task.Delay(17);
        }

        await Task.Delay(500);

        tutoBg.Visible = true;
        while (tutoBg.Opacity < 1f)
        {
            if (deck.Position.Y > 105)
                deck.Position += new Vector2(0f, -5f);
            tutoBg.Opacity += 0.02f;
            await Task.Delay(17);
        }
        while (deck.Position.Y > 105)
        {
            deck.Position += new Vector2(0f, -5f);
            await Task.Delay(17);
        }
        while (deck.Position.Y < 108)
        {
            deck.Position += new Vector2(0f, 1f);
            await Task.Delay(17);
        }
        electro20.Visible = false;
        table.Visible = false;
        bg.Visible = false;

        MediaPlayer.Play(bgm);
        await Task.Delay(500);
        Game.Window.Title = "La Podrida 2 - Tutorial";
        ccText.Position = new Vector2(400f, 50f);
        ccText.Color = new Color(0x00, 0x97, 0x9D);
        vo[5].Play();
        ccText.Text = "Well, it starts easy, each player gets 3 cards.";
        await Task.Delay(vo[5].Duration);
        ccText.Text = "";
        ElectroHandOut();
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
        if (roundCount is 0 && handCount is 0)
        {
            vo[12].Play();
            ccText.Text = "Now, I'll play a card.";
            await Task.Delay(vo[12].Duration);
            ccText.Text = "";
        }

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

    private async void ElectroHandOut()
    {
        deck.DrawOrder = 5;
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
        vo[6].Play();
        ccText.Text = "Then 3 cards are placed in the middle.";
        await Task.Delay(vo[6].Duration);
        ccText.Text = "";
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
        while (deck.Position.Y < 400)
        {
            deck.Position += new Vector2(-9f, 10f);
            await Task.Delay(17);
        }
        deckPlace.Play();
        deck.DrawOrder = 8;

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
        
        vo[7].Play();
        ccText.Text = "Now the first of the cards in the middle can be flipped.";
        await Task.Delay(vo[7].Duration + TimeSpan.FromMilliseconds(500));

        goldenCards[0].IsFaceUp = true;
        goldenCardImages[0].ChangeAnimatedTexture(goldenCards[0].GetTexture().Key, goldenCards[0].GetTexture().Value);

        vo[8].Play();
        ccText.Text = "Those cards, as you can see, are the golden cards.";
        await Task.Delay(vo[8].Duration);
        vo[9].Play();
        ccText.Text = "The game consists of a best of 3,\nthe biggest card wins the round.";
        await Task.Delay(vo[9].Duration);
        vo[10].Play();
        ccText.Text = "Now, all cards from the suit of the golden card\nhave higher values than the other suits.";
        await Task.Delay(vo[10].Duration);
        vo[11].Play();
        ccText.Text = "Come on, for example, play a card.";
        await Task.Delay(vo[11].Duration);
        ccText.Text = "";
    
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
        for (int i = 0; i < 3; i++)
        {
            G:
            var g = CardData.CreateRandom(true);
            if (yourCards.Any(x => x.Value == g.Value && x.Suit == g.Suit) || electroCards.Any(x => x.Value == g.Value && x.Suit == g.Suit) || goldenCards.Any(x => x.Value == g.Value && x.Suit == g.Suit))
                goto G;
            goldenCards[i] = g;
        }
        while (!electroCards.Any(x => x.Suit == goldenCards[0].Suit))
        {
            F:
            var e = CardData.CreateRandom(false);
            if (yourCards.Any(x => x.Value == e.Value && x.Suit == e.Suit) || electroCards.Any(x => x.Value == e.Value && x.Suit == e.Suit) || goldenCards.Any(x => x.Value == e.Value && x.Suit == e.Suit))
                goto F;
            electroCards[0] = e;
        }
        electroCards = electroCards.OrderBy(x => x.Value).ToArray();
    }

    private void EvaluateRound()
    {
        if (yourCards[yourPlayedCard].Value == electroCards[electroPlayedCard].Value && yourCards[yourPlayedCard].Suit != goldenCards[roundCount].Suit && electroCards[electroPlayedCard].Suit != goldenCards[roundCount].Suit)
        {
            HandleScore(handCount % 2 == 0);
            return;
        }    

        int yourValue = yourCards[yourPlayedCard].Points + (yourCards[yourPlayedCard].Suit == goldenCards[roundCount].Suit ? 100 : 0);
        int electroValue = electroCards[electroPlayedCard].Points + (electroCards[electroPlayedCard].Suit == goldenCards[roundCount].Suit ? 100 : 0);
        
        HandleScore(yourValue > electroValue);
    }

    private async void HandleScore(bool youWon)
    {
        if (youWon)
        {
            yourCards[yourPlayedCard].Won = true;
            yourCardImages[yourPlayedCard].ChangeAnimatedTexture(yourCards[yourPlayedCard].GetTexture().Key, yourCards[yourPlayedCard].GetTexture().Value);
            if (yourRounds)
            {
                if (handCount is 0)
                    MidMatch();
                else
                    OutroSequence();
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
                if (handCount is 0)
                    MidMatch();
                else
                    OutroSequence();
                return;
            }
            else
                electroRounds = true;
        }

        if (roundCount is 0 && handCount is 0)
        {
            vo[13].Play();
            ccText.Text = "As you can see, the golden card has\na higher value than the other suits.";
            await Task.Delay(vo[13].Duration);
            ccText.Text = "";
        }
        
        roundCount++;
        yourPlayedCard = -1;
        electroPlayedCard = -1;
        goldenCards[roundCount].IsFaceUp = true;
        goldenCardImages[roundCount].ChangeAnimatedTexture(goldenCards[roundCount].GetTexture().Key, goldenCards[roundCount].GetTexture().Value);

        if (roundCount is 1 && handCount is 0)
        {
            vo[14].Play();
            ccText.Text = "And, as you might have discovered,\nthere's a golden card per round.";
            await Task.Delay(vo[14].Duration);
            vo[15].Play();
            ccText.Text = "Let's keep on playing.";
            await Task.Delay(vo[15].Duration);
            ccText.Text = "";
        }

        if (youWon)
        {
            for (int i = 0; i < 3; i++)
                if (!yourCards[i].Used)
                    yourCardButtons[i].Enabled = true;
        }
        else
            ElectroPlayCard();
    }

    private async void MidMatch()
    {
        handCount++;
        vo[16].Play();
        ccText.Text = "Now that the hand's over, the winner gets the amount\nof points from the values of the golden cards.";
        await Task.Delay(vo[16].Duration + TimeSpan.FromMilliseconds(500));

        plus1.Visible = true;
        plus2.Visible = goldenCards[2].IsFaceUp;
        equal.Visible = true;
        roundResult.Text = $"{goldenCards[0].Points + goldenCards[1].Points + (goldenCards[2].IsFaceUp ? goldenCards[2].Points : 0)}";
        roundResult.Visible = true;

        vo[17].Play();
        ccText.Text = "An ace has a point value of 20.";
        await Task.Delay(vo[17].Duration);
        ccText.Text = "";

        plus1.Visible = false;
        plus2.Visible = false;
        equal.Visible = false;
        roundResult.Visible = false;

        while (goldenCardImages[0].Position.Y > -400)
        {
            goldenCardImages[0].Position += new Vector2(0f, -10f);
            goldenCardImages[1].Position += new Vector2(0f, -10f);
            goldenCardImages[2].Position += new Vector2(0f, -10f);
            electroCardImages[0].Position += new Vector2(0f, -10f);
            electroCardImages[1].Position += new Vector2(0f, -10f);
            electroCardImages[2].Position += new Vector2(0f, -10f);
            yourCardImages[0].Position += new Vector2(0f, -10f);
            yourCardImages[1].Position += new Vector2(0f, -10f);
            yourCardImages[2].Position += new Vector2(0f, -10f);
            await Task.Delay(17);
        }

        ResetTable();

        vo[18].Play();
        ccText.Text = "Now, let's play another hand.\nGrab the deck and hand out the cards.";
        await Task.Delay(vo[18].Duration);
        ccText.Text = "";

        YourHandOut();
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

    private void YourHandOut()
    {
        for (int i = 0; i < 3; i++)
        {
            yourCardImages[i].Position = new Vector2(400f, 700f);
            yourCardImages[i].Visible = false;
            electroCardImages[i].Position = new Vector2(400f, 700f);
            electroCardImages[i].Visible = false;
            goldenCardImages[i].Position = new Vector2(400f, 700f);
            goldenCardImages[i].Visible = false;
        }
        deckButton.Enabled = true;
        circle.Visible = true;
        oscilatingOpacityImageReference = circle;
        CreateCards();
    }

    private async void OutroSequence()
    {
        plus1.Visible = true;
        plus2.Visible = goldenCards[2].IsFaceUp;
        equal.Visible = true;
        roundResult.Text = $"{goldenCards[0].Points + goldenCards[1].Points + (goldenCards[2].IsFaceUp ? goldenCards[2].Points : 0)}";
        roundResult.Visible = true;

        await Task.Delay(3000);

        bg.Visible = true;
        electro20.Position = new Vector2(400f, 500f);
        electro20.Visible = true;
        electro20.ChangeAnimatedTexture(idle, null);
        while (tutoBg.Opacity > 0f)
        {
            tutoBg.Opacity -= 0.02f;
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
            roundResult.Opacity -= 0.02f;
            await Task.Delay(17);
        }

        tutoBg.Visible = false;
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

        ccText.Position = new Vector2(400f, 750f);
        ccText.Color = Color.White;
        vo[19].Play();
        ccText.Text = "Well done, a game consists of 7 hands,\nthe player with the most points after that wins.";
        await Task.Delay(vo[19].Duration);
        MediaPlayer.Stop();
        vo[20].Play();
        ccText.Text = "Let's get going to the casino,\nelse we're gonna be there late!";
        await Task.Delay(vo[20].Duration);
        Game.Window.Title = "La Podrida 2 - Street";
        while (bg.Position.Y > -800f)
        {
            bg.Position += new Vector2(0f, -5f);
            electro20.Position += new Vector2(0f, -5f);
            ccText.Position += new Vector2(0f, -5f);
            await Task.Delay(17);
        }
        
        SwitchState(new CasinoState(Game));
    }
}