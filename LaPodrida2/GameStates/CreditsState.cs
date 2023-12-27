using System;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace LaPodrida2;

public class CreditsState : GameState
{
    private bool done = false;
    private Texture2D electro20;
    private Song pokemongo;
    private Song bgm;
    public CreditsState(Game game) : base(game) {}

    public override void LoadContent()
    {
        electro20 = Game.Content.Load<Texture2D>(@"Textures\Credits\electro20");
        _components.Add(new SimpleImage(Game, Game.Content.Load<Texture2D>(@"Textures\Credits\splash"), new Vector2(0, 0), 1, anchor: Alignment.TopLeft));
        bgm = Game.Content.Load<Song>(@"Audio\Credits\bgm");
        pokemongo = Game.Content.Load<Song>(@"Audio\Credits\Welcome_to_la_podrida");
    }

    public override void Initialize()
    {
        base.Initialize();
        MediaPlayer.Play(bgm);
        MediaPlayer.IsRepeating = false;
        MediaPlayer.MediaStateChanged += TheLastJoke;
    }

    public override void Dispose()
    {   
        MediaPlayer.MediaStateChanged -= TheLastJoke;
        base.Dispose();
    }

    private void TheLastJoke(object sender, EventArgs e)
    {
        if (done) Game.Exit();
        MediaPlayer.MediaStateChanged -= TheLastJoke;
        done = true;
        MediaPlayer.Play(pokemongo);
        (_components[0] as SimpleImage).ChangeTexture(electro20);
        MediaPlayer.MediaStateChanged += TheLastJoke;
    }

    public override void UnloadContent()
    {
        Game.Content.UnloadAsset(@"Textures\Credits\splash");
        Game.Content.UnloadAsset(@"Textures\Credits\electro20");
        Game.Content.UnloadAsset(@"Audio\Credits\bgm");
        Game.Content.UnloadAsset(@"Audio\Credits\Welcome_to_la_podrida");
    }
}