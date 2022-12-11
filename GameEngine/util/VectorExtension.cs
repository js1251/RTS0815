using Microsoft.Xna.Framework;

namespace GameEngine.util;

public static class VectorExtension {
    public static float[] ToArray(this Vector2 vector) {
        return new[] { vector.X, vector.Y };
    }

    public static float[] ToArray(this Vector3 vector) {
        return new[] { vector.X, vector.Y, vector.Z };
    }

    public static float[] ToArray(this Vector4 vector) {
        return new[] { vector.X, vector.Y, vector.Z, vector.W };
    }
}