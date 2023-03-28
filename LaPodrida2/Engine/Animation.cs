using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Engine;

public class Animation<T>
{
    #region Fields
    private T[] _frames;
    private int pos;
    private int frameDelay;
    private bool isLooped;
    private readonly int frameDuration;
    public bool Paused { get; set; } = false;
    #endregion

    public Animation(T[] frames, bool looped, int frameDuration = 1)
    {
        _frames = frames;
        isLooped = looped;
        pos = _frames.Length - 1;
        this.frameDuration = frameDuration - 1;
        frameDelay = this.frameDuration;
    }

    public void Start() => pos = 0;

    public T NextFrame()
    {
        if (Paused)
            return CurrentFrame();

        if (pos > _frames.Length - 1)
        {
            if (isLooped)
                pos = 0;
            else
                return _frames[pos-1];
        }

        if (frameDelay-- > 0)
            return _frames[pos];

        frameDelay = frameDuration;

        return _frames[Paused ? pos : pos++];
    }

    public T CurrentFrame() 
    {   
        if (pos < _frames.Length)
            return _frames[pos];
        else
            return _frames[pos-1];
    }

    public T[] GetFrames() => _frames;

    public static Animation<Rectangle> TextureAnimation(Point frameSize, Point bounds, bool looped, int frameDuration)
    {
        var frames = new List<Rectangle>();
        for (int y = 0; y < bounds.Y; y += frameSize.Y)
        {
            for (int x = 0; x < bounds.X; x += frameSize.X)
            {
                frames.Add(new Rectangle(x, y, frameSize.X, frameSize.Y));
            }
        }

        return new Animation<Rectangle>(frames.ToArray(), looped, frameDuration);
    }
}