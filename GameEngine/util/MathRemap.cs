using Microsoft.Xna.Framework;

namespace GameEngine.util {
    public static class MathRemap {
        public static float Remap(this float value, float low1, float high1, float low2, float high2) {
            return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
        }

        public static double Remap(this double value, double low1, double high1, double low2, double high2) {
            return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
        }

        public static Color Remap(this float value, float low1, float high1, Color low2, Color high2) {
            var proportion = (value - low1) / (high1 - low1);
            var redDiff = (high2.R - low2.R);
            var greenDiff = (high2.G - low2.G);
            var blueDiff = (high2.B - low2.B);

            var r = (byte)(low2.R + proportion * redDiff);
            var g = (byte)(low2.G + proportion * greenDiff);
            var b = (byte)(low2.B + proportion * blueDiff);

            return new Color(r, g, b);
        }

        public static Color Remap(this double value, double low1, double high1, Color low2, Color high2) {
            return ((float)value).Remap((float)low1, (float)high1, low2, high2);
        }

        public static Vector2 Remap(this Vector2 value, Vector2 low1, Vector2 high1, Vector2 low2, Vector2 high2) {
            return new Vector2 {
                X = value.X.Remap(low1.X, high1.X, low2.X, high2.X),
                Y = value.Y.Remap(low1.Y, high1.Y, low2.Y, high2.Y)
            };
        }
    }
}