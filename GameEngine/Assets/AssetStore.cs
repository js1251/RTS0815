using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameEngine.Assets;

public static class AssetStore {
    public static Dictionary<string, Texture2D> Textures { get; } = new();
    public static Dictionary<string, SpriteFont> Fonts { get; } = new();
    public static Dictionary<string, SoundEffect> Sounds { get; } = new();
    public static Dictionary<string, Song> Songs { get; } = new();
    public static Dictionary<string, Effect> Effects { get; } = new();

    public static void LoadAsset<T>(ContentManager content, string name, string path) {
        var tType = typeof(T);

        if (tType == typeof(Texture2D)) {
            AddAsset(name, content.Load<Texture2D>(path), Textures);
            return;
        }

        if (tType == typeof(SoundEffect)) {
            AddAsset(name, content.Load<SoundEffect>(path), Sounds);
            return;
        }

        if (tType == typeof(Song)) {
            AddAsset(name, content.Load<Song>(path), Songs);
            return;
        }

        if (tType == typeof(SpriteFont)) {
            AddAsset(name, content.Load<SpriteFont>(path), Fonts);
            return;
        }

        if (tType == typeof(Effect)) {
            AddAsset(name, content.Load<Effect>(path), Effects);
            return;
        }

        throw new Exception($"Unknown asset type {tType}");
    }

    private static void AddAsset<T>(string name, T content, IDictionary<string, T> dict) {
        if (dict.ContainsKey(name)) {
            throw new Exception($"Asset {name} already exists");
        }

        dict.Add(name, content);
    }
}