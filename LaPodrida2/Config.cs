using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace LaPodrida2;

public static class Configs
{
    #region Events
    public static event EventHandler MusicVolumeChanged;
    public static event EventHandler SfxVolumeChaged;
    #endregion

    #region Fields
    private const string file = ".\\configuraciondelapodrida2.porfavornotocar";
    /* 0 */ private static int _musicVolume;
    /* 1 */ private static int _sfxVolume;
    #endregion

    // Instances
    private static FileStream fileStream;

    #region Properties
    /* 0 */ public static int MusicVolume { get => _musicVolume; set {SetConfig(value); _musicVolume = value; MusicVolumeChanged?.Invoke(null, new EventArgs());}}
    /* 1 */ public static int SfxVolume { get => _sfxVolume; set {SetConfig(value); _sfxVolume = value; SfxVolumeChaged?.Invoke(null, new EventArgs());}}
    #endregion

    // Constructor
    public static void Initialize()
    {
        try
        {
            fileStream = File.Open(file, FileMode.Open);
        } catch
        {
            fileStream = File.Create(file);
            /* 0 */ MusicVolume = 10;
            /* 1 */ SfxVolume = 10;
            return;
        }
        /* 0 */ _musicVolume = (byte)fileStream.ReadByte();
        /* 1 */ _sfxVolume = (byte)fileStream.ReadByte();
        Fixer();
    }

    #region Methods
    private static void SetConfig(object value, [CallerMemberName] string name = null)
    {
        switch (name)
        {
            case "MusicVolume": fileStream.Position = 0; break;
            case "SfxVolume": fileStream.Position = 1; break;
        }

        fileStream.WriteByte(Convert.ToByte(value));
    }

    public static void CloseFile() => fileStream.Close();

    private static void Fixer()
    {
        // 0
        if (_musicVolume > 10)
            MusicVolume = 10;
        // 1
        if (_sfxVolume > 10)
            SfxVolume = 10;
    }
    #endregion
}