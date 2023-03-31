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
    private Texture2D back;
    private SimpleImage bg;
    private SimpleImage table;
    private SimpleImage deck;
    private SimpleImage map;
    private SimpleImage gameBg;
    private TextComponent ccText;
    private CardData[] electroCards;
    private SimpleImage[] electroCardImages = new SimpleImage[3];
    private CardData[] goldenCards;
    private SimpleImage[] goldenCardImages = new SimpleImage[3];
    private CardData[] yourCards;
    private SimpleImage[] yourCardImages = new SimpleImage[3];
    private int yourPlayedCard = -1;
    private int electroPlayedCard = -1;
    private int yourPoints = 0;
    private int electroPoints = 0;
    private bool yourRounds = false;
    private bool electroRounds = false;
    private int roundCount = 0;
    private int handCount = 0;
    private Song streetShit;
    private Song bgm;
    private Button[] yourCardButtons = new Button[3];
    private Button deckButton;
    private Button[] cardPlacerButtons = new Button[9];
    private SimpleImage oscilatingOpacityImageReference;
    private SimpleImage plus1;
    private SimpleImage plus2;
    private SimpleImage equal;
    private TextComponent roundResult;
    private bool goingUp = false;


    public CasinoState(Game game) : base(game) {}

    public override void LoadContent()
    {
        #region Textures
        back = Game.Content.Load<Texture2D>(@"Textures\Cards\back_red");
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
        plus1 = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\plus"), new Vector2(333f, 400f), 10, visible: false, scale: .5f, color: Color.Black, animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30));
        plus2 = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Menu\plus"), new Vector2(467f, 400f), 10, visible: false, scale: .5f, color: Color.Black, animation: Animation<Rectangle>.TextureAnimation(new Point(100), new Point(100, 200), true, 30));
        equal = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Casino\equals"), new Vector2(602f, 400f), 10, visible: false, scale: .5f, animation: Animation<Rectangle>.TextureAnimation(new Point(135), new Point(270, 135), true, 30));
        _components.Add(plus1);
        _components.Add(plus2);
        _components.Add(equal);
        #endregion

        #region Audios
        streetShit = Game.Content.Load<Song>(@"Audio\Casino\street");
        bgm = Game.Content.Load<Song>(@"Audio\Casino\bgm");
        #endregion

        #region TextComponents
        roundResult = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\romanAlexander"), "", new Vector2(670f, 400f), 10, anchor: Alignment.Center, color: Color.Black, visible: false);
        ccText = new TextComponent(Game, Game.Content.Load<SpriteFont>(@"Other\consolas"), "", new Vector2(400f, 750f), 10, anchor: Alignment.Center);
        _components.Add(roundResult);
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
        MediaPlayer.Play(streetShit);

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
            if (deck.Position.Y >= 705 && deck.Position.Y < 708)
                deck.Position += new Vector2(0f, 1f);
            await Task.Delay(17);
        }
        bg.Visible = false;
        table.Visible = false;
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
            if (handCount is 0) Decker(null, null);
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

    private async void Decker(object sender, EventArgs e)
    {
        deckButton.Enabled = false;
        deckButton.Image.Visible = false;
        if (!goldenCardImages[2].Visible)
        {
            if (sender is not null)
                while (deck.Position.Y < 700)
                {
                    deck.Position += new Vector2(9f, 10f);
                    await Task.Delay(17);
                }
            
            deck.DrawOrder = 5;
            cardPlacerButtons[0].Enabled = true;
            cardPlacerButtons[0].Image.Visible = true;
            oscilatingOpacityImageReference = cardPlacerButtons[0].Image;
            return;
        }

        deck.DrawOrder = 9;

        oscilatingOpacityImageReference = null;
        while (deck.Position.Y > 400)
        {
            deck.Position += new Vector2(-9f, -10f);
            await Task.Delay(17);
        }

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
            goldenCardImages[index-6].Visible = true;
            while (goldenCardImages[index-6].Position.Y > 400f)
            {
                goldenCardImages[index-6].Position += new Vector2(4.5f * (index-7), -10f);
                await Task.Delay(17);
            }
        } else if (index % 2 == 0)
        {
            electroCardImages[index/2].Visible = true;
            while (electroCardImages[index/2].Position.Y > 250) {
                electroCardImages[index/2].Position += new Vector2(3f * (index/2-1), -10f);
                await Task.Delay(17);
            }
        } else
        {
            yourCardImages[(index-1)/2].Visible = true;
            while (yourCardImages[index/2].Position.Y > 550)
            {
                yourCardImages[index/2].Position += new Vector2(9f * ((index-1)/2-1), -10f);
                await Task.Delay(17);
            }
        }
    }

    private async void ElectroHandOut()
    {
        deck.DrawOrder = 5;
        while (deck.Position.Y > 100)
        {
            deck.Position += new Vector2(9f, -10f);
            await Task.Delay(17);
        }
        while (deck.Position.Y > 92)
        {
            deck.Position += new Vector2(0f, -1f);
            await Task.Delay(17);
        }
        for (int i = 0; i < 3; i++)
        {
            yourCardImages[i].Visible = true;
            yourCardImages[i].Position = new Vector2(400f, 100f);
            electroCardImages[i].Visible = true;
            electroCardImages[i].Position = new Vector2(400f, 100f);
            goldenCardImages[i].Visible = true;
            goldenCardImages[i].Position = new Vector2(400f, 100f);
        }
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
    }

    private void PlayCard(object sender, EventArgs e)
    {
        yourCardButtons[0].Enabled = false;
        yourCardButtons[1].Enabled = false;
        yourCardButtons[2].Enabled = false;
        yourPlayedCard = yourCardButtons.ToList().IndexOf(sender as Button);
        yourCards[yourPlayedCard].Used = true;
        yourCardImages[yourPlayedCard].Position = new Vector2(265 + 135 * roundCount, 550f);

        if (electroPlayedCard is -1)
            ElectroPlayCard();
        else
            EvaluateRound();
    }

    private void ElectroPlayCard()
    {
        electroPlayedCard = Array.IndexOf(electroCards, electroCards.FirstOrDefault((x => x.Suit == goldenCards[roundCount].Suit && !x.Used), electroCards.First(x => !x.Used)));
        electroCards[electroPlayedCard].IsFaceUp = true;
        electroCardImages[electroPlayedCard].ChangeAnimatedTexture(electroCards[electroPlayedCard].GetTexture().Key, electroCards[electroPlayedCard].GetTexture().Value);
        electroCardImages[electroPlayedCard].Position = new Vector2(265 + 135 * roundCount, 250f);
        electroCardImages[electroPlayedCard].Visible = true;
        electroCards[electroPlayedCard].Used = true;

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
        if (yourCards[yourPlayedCard].Value == electroCards[electroPlayedCard].Value)
            HandleScore(handCount % 2 == 1);

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
        handCount++;
        plus1.Visible = true;
        plus2.Visible = goldenCards[2].IsFaceUp;
        equal.Visible = true;
        roundResult.Text = $"{goldenCards[0].Points + goldenCards[1].Points + (goldenCards[2].IsFaceUp ? goldenCards[2].Points : 0)}";
        roundResult.Visible = true;

        await Task.Delay(3000);

        bg.Visible = true;
        table.Visible = true;
        while (gameBg.Opacity > 0f)
        {
            if (roundResult.Position.Y != 400f - 250 * (youWon ? 1 : -1))
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

        await Task.Delay(3000);

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
}