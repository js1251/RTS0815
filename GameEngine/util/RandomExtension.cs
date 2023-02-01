using System;
using Microsoft.Xna.Framework;

namespace GameEngine.util;

public static class RandomExtension {
    // caching a Random instance to avoid creating a new one every time
    private static readonly Random sRandom = new();

    public static float NextFloat(float min, float max) {
        return (float)sRandom.NextDouble() * (max - min) + min;
    }

    public static double NextDouble(double min, double max) {
        return sRandom.NextDouble() * (max - min) + min;
    }

    public static Vector2 NextVector2(Vector2 min, Vector2 max) {
        var (xMin, yMin) = min;
        var (xMax, yMax) = max;
        return new Vector2(NextFloat(xMin, xMax), NextFloat(yMin, yMax));
    }

    public static Color NextColor(Color min, Color max) {
        return new Color(
            (int)NextFloat(min.R, max.R),
            (int)NextFloat(min.G, max.G),
            (int)NextFloat(min.B, max.B)
        );
    }
}