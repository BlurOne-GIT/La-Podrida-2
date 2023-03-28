using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Engine;

namespace LaPodrida2;

public class TutorialState : GameState
{
    private SimpleImage bg;
    private SimpleImage table;
    private SimpleImage deck;
    private TextComponent ccText;
    private SoundEffect vo1; // "What do you mean you don't know how to play?"
    private SoundEffect vo2; // "The tournament starts in an hour..."
    private SoundEffect vo3; // "I guess there's time, I'll give you a little tutorial."
    private SoundEffect vo4; // "Yes, this is my house and this is my table!"
    private SoundEffect vo5; // "If you don't like the stock logo you can leave!"
    private CardData[] electroCards = new CardData[3];
    private SimpleImage[] electroCardImages = new SimpleImage[3];
    private CardData[] goldenCards = new CardData[3];
    private SimpleImage[] goldenCardImages = new SimpleImage[3];
    private CardData[] yourCards = new CardData[3];
    private SimpleImage[] yourCardImages = new SimpleImage[3];
    private int roundCount = 0;
    private Song bgm;
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
        for (int i = 0; i < 3; i++)
        {
            electroCardImages[i] = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\back_blue"), new Vector2(400f, 200f), 7, false);
            goldenCardImages[i] = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\back_blue"), new Vector2(400f, 200f), 7, false);
            yourCardImages[i] = new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Cards\back_blue"), new Vector2(400f, 200f), 7, false);
            _components.Add(electroCardImages[i]);
            _components.Add(goldenCardImages[i]);
            _components.Add(yourCardImages[i]);
        }
        
        _components.Add(bg);
        _components.Add(table);
        _components.Add(deck);
        #endregion

        #region Audios
        vo1 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo1");
        vo2 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo2");
        vo3 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo3");
        vo4 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo4");
        vo5 = Game.Content.Load<SoundEffect>(@"Audio\Tutorial\vo5");
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
            table.Scale += 0.035f;
            table.Position += new Vector2(0f, -2f);
            await Task.Delay(17);
        }
        deck.Visible = false;

        while (table.Position.Y > 250)
        {
            table.Position += new Vector2(0f, -2f);
            await Task.Delay(17);
        }

        await Task.Delay(500);
        MediaPlayer.Play(bgm);
        Game.Window.Title = "La Podrida 2 - Tutorial";
        ccText.Position = new Vector2(400f, 50f);
        ElectroHandOut();
    }

    private async void ElectroHandOut()
    {
        for (int i = 0; i < 3; i++)
        {
            yourCardImages[i].Visible = true;
            while (yourCardImages[i].Position.Y < 610)
            {
                yourCardImages[i].Position += new Vector2(3 * (i-1), 10f);
                await Task.Delay(17);
            }
            electroCardImages[i].Visible = true;
            while (electroCardImages[i].Position.Y < 330) {
                electroCardImages[i].Position += new Vector2(9 * (i-1), 10f);
                await Task.Delay(17);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            goldenCardImages[i].Visible = true;
            while (goldenCardImages[i].Position.Y < 470)
            {
                goldenCardImages[i].Position += new Vector2(4.5f * (i-1), 10f);
                await Task.Delay(17);
            }
        }

        CardsCreate();
        while (yourCardImages[0].Position.Y < 700)
        {
            yourCardImages[0].Position += new Vector2(0f, 5f);
            yourCardImages[1].Position += new Vector2(0f, 5f);
            yourCardImages[2].Position += new Vector2(0f, 5f);
            await Task.Delay(17);
        }

        yourCards[0].IsFaceUp = true;
        yourCards[1].IsFaceUp = true;
        yourCards[2].IsFaceUp = true;
        goldenCards[0].IsFaceUp = true;
        yourCardImages[0].ChangeAnimatedTexture(yourCards[0].GetTexture().Key, yourCards[0].GetTexture().Value);
        yourCardImages[1].ChangeAnimatedTexture(yourCards[1].GetTexture().Key, yourCards[1].GetTexture().Value);
        yourCardImages[2].ChangeAnimatedTexture(yourCards[2].GetTexture().Key, yourCards[2].GetTexture().Value);
        goldenCardImages[0].ChangeAnimatedTexture(goldenCards[0].GetTexture().Key, goldenCards[0].GetTexture().Value);
    }

    private void CardsCreate()
    {   
        for (int i = 0; i < 3; i++)
        {
            G:
            var r = CardData.CreateRandom(true);
            if (goldenCards.Where(x => x.Value == r.Value && x.Suit == r.Suit).Count() != 0)
                goto G;
            goldenCards[i] = r;
        }
        for (int i = 0; i < 3; i++)
        {
            Y:
            var r = CardData.CreateRandom(false);
            if (yourCards.Where(x => x.Value == r.Value && x.Suit == r.Suit).Count() != 0 && goldenCards.Where(x => x.Value == r.Value && x.Suit == r.Suit).Count() != 0)
                goto Y;
            yourCards[i] = r;
        }
        for (int i = 0; i < 3; i++)
        {
            E:
            var r = CardData.CreateRandom(false);
            if (electroCards.Where(x => x.Value == r.Value && x.Suit == r.Suit).Count() != 0 && goldenCards.Where(x => x.Value == r.Value && x.Suit == r.Suit).Count() != 0 && yourCards.Where(x => x.Value == r.Value && x.Suit == r.Suit).Count() != 0)
                goto E;
            electroCards[i] = r;
        }
    }
}