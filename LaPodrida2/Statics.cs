using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Engine;

namespace LaPodrida2;

public static class Statics
{
    #region Fields
    #endregion

    #region Properties
    public static SpriteFont TextureFont { get; private set; }
    public static SpriteFont BoldFont { get; private set; }
    public static SpriteFont LightFont { get; private set; }
    public static bool WindowFocused { get; set; }
    public static Vector2 DetectionPoint { get; } = new Vector2(13f);
    #endregion

    #region Methods
    public static void LoadFonts(ContentManager content)
    {
        Texture2D fontTexture = content.Load<Texture2D>("Fonts");
        List<Vector3> kernings = new List<Vector3>();
        List<Rectangle> glyphRectangles = new List<Rectangle>();
        List<Rectangle> fontRectangles = new List<Rectangle>();
        List<char> characters = new List<char>{
            ' ',
            '!',
            '%',
            '*',
            '-',
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            ':',
            '=',
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            '_'
        };
        for (int i = 0; i < characters.Count; i++)
        {
            glyphRectangles.Add(new Rectangle(new Point(i*8, 0), new Point(8, 7)));
            fontRectangles.Add(new Rectangle(new Point(0, 0), new Point(8, 7)));
            kernings.Add(new Vector3(0, 8, 0));
        }
        LightFont = new SpriteFont(fontTexture, glyphRectangles, fontRectangles, characters, 0, 0, kernings, ' ');
        characters = new List<char>{
            ' ',
            '/',
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            '_'
        };
        glyphRectangles = new List<Rectangle>();
        fontRectangles = new List<Rectangle>();
        kernings = new List<Vector3>();
        for (int i = 0; i < characters.Count; i++)
        {
            Point p = new Point(i * 8, 7);
            glyphRectangles.Add(new Rectangle(p, new Point(8, 8)));
            fontRectangles.Add(new Rectangle(new Point(0, 0), new Point(8, 7)));
            kernings.Add(new Vector3(0, 8, 0));
        }
        BoldFont = new SpriteFont(fontTexture, glyphRectangles, fontRectangles, characters, 0, 0, kernings, ' ');
    }
    public static void Focus(object s, EventArgs e) => WindowFocused = true;
    public static void UnFocus(object s, EventArgs e) => WindowFocused = false;
    #endregion
}