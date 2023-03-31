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
    private SimpleImage bg;
    private SimpleImage table;
    private SimpleImage deck;
    private SimpleImage tutoBg;
    private TextComponent ccText;
    private SoundEffect vo1; // "What do you mean you don't know how to play?"
    private SoundEffect vo2; // "The tournament starts in an hour..."
    private SoundEffect vo3; // "I guess there's time, I'll give you a little tutorial."
    private SoundEffect vo4; // "Yes, this is my house and this is my table!"
    private SoundEffect vo5; // "If you don't like the stock logo you can leave!"
    private SoundEffect vo6; // "Well, it starts easy, each player gets 3 cards."
    private SoundEffect vo7; // "Then 3 cards are placed in the middle."
    private SoundEffect vo8; // "Now the first of the cards in the middle can be flipped."
    private SoundEffect vo9; // "Those cards, as you can see, are the golden cards."
    private SoundEffect vo10; // "The game consists of a best of 3, the biggest card wins the round."
    private SoundEffect vo11; // "Now, all cards from the suit of the golden card have higher values than the other suits."
    private SoundEffect vo12; // "Come on, for example, play a card."
    private SoundEffect vo13; // "Now, I'll play a card."
    private SoundEffect vo14; // "As you can see, the golden card has a higher value than the other suits."
    private SoundEffect vo15; // "And, as you might have discovered, there's a golden card per round."
    private SoundEffect vo16; // "Let's keep on playing."
    private SoundEffect vo17; // "Now that the hand's over, the winner gets the amount of points from the values of the golden cards."
    private SoundEffect vo18; // "An ace has a point value of 20."
    private SoundEffect vo19; // "Now, let's play another hand. Grab the deck and hand out the cards."
    private SoundEffect vo20; // "Well done, a game consists of 7 hands, the player with the most points after that wins."
    private SoundEffect vo21; // "Let's get going to the casino, else we're gonna be there late!"
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
    private int cardCount = 0;
    private int roundCount = 0;
    private Song bgm;
    private Button[] yourCardButtons = new Button[3];
    private SimpleImage oscilatingOpacityImageReference;
    private bool goingUp = false;


    public TutorialState(Game game) : base(game) {}

    public override void LoadContent()
    {
        #region Textures
        CardData.LoadTextures(Game.Content, false);
        #endregion

        #region SimpleImages
        bg = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Tutorial\bg"), new Vector2(0f, 800f), 0, anchor: Alignment.TopLeft);
        table = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Tutorial\table"), new Vector2(400f, 800f), 0, anchor: Alignment.TopCenter, scale: 1.2f);
        deck = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\deck_blue"), new Vector2(200f, -200f), 5);
        tutoBg = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Tutorial\tablebg"), Vector2.Zero, 1, anchor: Alignment.TopLeft, visible: false, animation: Animation<Rectangle>.TextureAnimation(new Point(800, 800), new Point(1600, 800), true, 30), opacity: 0f);
        for (int i = 0; i < 3; i++)
        {
            electroCardImages[i] = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\back_blue"), new Vector2(400f, 100f), 7, false);
            goldenCardImages[i] = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\back_blue"), new Vector2(400f, 100f), 7, false);
            yourCardImages[i] = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\back_blue"), new Vector2(400f, 100f), 7, false);
            _components.Add(electroCardImages[i]);
            _components.Add(goldenCardImages[i]);
            _components.Add(yourCardImages[i]);
        }
        
        _components.Add(bg);
        _components.Add(table);
        _components.Add(deck);
        _components.Add(tutoBg);
        #endregion

        #region Buttons
        yourCardButtons[0] = new Button(Game, new Rectangle(265, 700, 88, 124), enabled: false, hasHover: false);
        yourCardButtons[1] = new Button(Game, new Rectangle(400, 700, 88, 124), enabled: false, hasHover: false);
        yourCardButtons[2] = new Button(Game, new Rectangle(535, 700, 88, 124), enabled: false, hasHover: false);
        _components.Add(yourCardButtons[0]);
        _components.Add(yourCardButtons[1]);
        _components.Add(yourCardButtons[2]);
        #endregion

        #region Audios
        vo1 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo1");
        vo2 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo2");
        vo3 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo3");
        vo4 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo4");
        vo5 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo5");
        /*
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
        bgm = Game.Content.Load<Song>(@"Audio\Tutorial\bgm");
        #endregion

        #region TextComponents
        ccText = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\consolas"), "", new Vector2(400f, 750f), 10, anchor: Alignment.Center);
        _components.Add(ccText);
        #endregion
    }

    public override void Initialize()
    {
        base.Initialize();
        MediaPlayer.Stop();
        yourCardButtons[0].LeftClicked += PlayCard;
        yourCardButtons[1].LeftClicked += PlayCard;
        yourCardButtons[2].LeftClicked += PlayCard;
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

    public override void HandleInput(object s, InputKeyEventArgs e)
    {
        switch (e.Key)
        {
            case Microsoft.Xna.Framework.Input.Keys.Up:
                table.Position += new Vector2(0, -5f);
                ccText.Text = $"Y: {table.Position.Y}";
                break;
            case Microsoft.Xna.Framework.Input.Keys.Down:
                table.Position += new Vector2(0, 5f);
                ccText.Text = $"Y: {table.Position.Y}";
                break;
        }
        base.HandleInput(s, e);
    }

    public override void UnloadContent()
    {
        // LA PODRIDAAAAAAAAAAAAAAAAA (2)
        throw new NotImplementedException();
    }

    private async void IntroSequence()
    {
        while (bg.Position.Y > 0)
        {
            bg.Position += new Vector2(0f, -5f);
            await Task.Delay(17);
        }
        
        vo1.Play();
        ccText.Text = "What do you mean you don't know how to play?";
        await Task.Delay(vo1.Duration);
        vo2.Play();
        ccText.Text = "The tournament starts in an hour...";
        await Task.Delay(vo2.Duration);
        vo3.Play();
        ccText.Text = "I guess there's time, I'll give you a little tutorial.";
        await Task.Delay(vo3.Duration);
        ccText.Text = "";

        while (table.Position.Y > 450)
        {
            table.Position += new Vector2(0f, -5f);
            await Task.Delay(17);
        }

        vo4.Play();
        ccText.Text = "Yes, this is my house and this is my table!";
        Game.Window.Title = "La Podrida 2 - Electro20's House";
        await Task.Delay(vo4.Duration);
        vo5.Play();
        ccText.Text = "If you don't like the stock logo you can leave!";
        await Task.Delay(vo5.Duration);
        ccText.Text = "";

        while (deck.Position.Y < 490)
        {
            deck.Position += new Vector2(0f, 25f);
            await Task.Delay(17);
        }

        await Task.Delay(500);

        while (deck.Position.X < 400)
        {
            deck.Position += new Vector2(5f, -5f);
            await Task.Delay(17);
        }
        deck.Visible = false;
        tutoBg.Visible = true;
        while (tutoBg.Opacity < 1f)
        {
            tutoBg.Opacity += 0.02f;
            await Task.Delay(17);
        }

        MediaPlayer.Play(bgm);
        await Task.Delay(500);
        Game.Window.Title = "La Podrida 2 - Tutorial";
        ccText.Position = new Vector2(400f, 50f);
        ccText.Color = new Color(0x00, 0x97, 0x9D);
        ccText.Text = "Well, it starts easy, each player gets 3 cards.";
        //vo6.Play();

        ElectroHandOut();



        yourCardButtons[0].Enabled = true;
        yourCardButtons[1].Enabled = true;
        yourCardButtons[2].Enabled = true;
    }

    private async void ElectroHandOut()
    {
        for (int i = 0; i < 3; i++)
        {
            yourCardImages[i].Visible = true;
            while (yourCardImages[i].Position.Y < 550)
            {
                yourCardImages[i].Position += new Vector2(1.5f * (i-1), 20f);
                await Task.Delay(17);
            }
            electroCardImages[i].Visible = true;
            while (electroCardImages[i].Position.Y < 250) {
                electroCardImages[i].Position += new Vector2(9 * (i-1), 10f);
                await Task.Delay(17);
            }
        }
        ccText.Text = "Then 3 cards are placed in the middle.";
        //vo7.Play();
        await Task.Delay(3000);
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

        CardsCreate();
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

    private void CardsCreate()
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
        electroCards = electroCards.OrderBy(x => x.Value).ToArray();
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
        var cardIndex = yourCardButtons.ToList().IndexOf(sender as Button);

        if (electroPlayedCard is null)
            ElectroPlayCard();

    }

    private void ElectroPlayCard()
    {
        CardData c = (CardData)electroCards.FirstOrDefault((x => x?.Suit == goldenCards[roundCount].Suit), electroCards.First(x => x is not null));
        c.IsFaceUp = true;
        electroCardImages[Array.IndexOf(electroCards, c)].Position = new Vector2(265 + 135 * roundCount, 250f);
        electroCards[Array.IndexOf(electroCards, c)] = null;
        electroPlayedCard = c;
    }

    private void EvaluateRound()
    {

    }
}