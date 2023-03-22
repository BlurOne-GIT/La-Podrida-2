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
    private bool isLooped;
    public bool Paused { get; set; } = false;
    public bool IsFinished { get => _frames.Length - 1 == pos; }
    #endregion

    public Animation(T[] frames, bool looped)
    {
        _frames = frames;
        isLooped = looped;
        pos = _frames.Length - 1;
    }

    public void Start() => pos = 0;

    public T NextFrame()
    {
        if (IsFinished)
        {
            if (isLooped)
                pos = 0;
            else
                return _frames[pos];
        }

        return _frames[Paused ? pos : pos++];
    }

    public static Animation<Rectangle> TextureAnimation(Point frameSize, Point bounds, bool looped)
    {
        var frames = new List<Rectangle>();
        for (int y = 0; y < bounds.Y; y += frameSize.Y)
        {
            for (int x = 0; x < bounds.X; x += frameSize.X)
            {
                frames.Add(new Rectangle(x, y, frameSize.X, frameSize.Y));
            }
        }

        return new Animation<Rectangle>(frames.ToArray(), looped);
    }
}