using Microsoft.Xna.Framework;

namespace GameEngine.util;

public static class GameTimeExtension {
    public static float CurTime(this GameTime gameTime) {
        return (float)gameTime.TotalGameTime.TotalSeconds;
    }

    public static float DeltaTime(this GameTime gameTime) {
        return (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}