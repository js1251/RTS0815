using GameEngine.Input;
using Microsoft.Xna.Framework;

namespace GameEngine.Rendering;

public class Camera {
    public Matrix Transform { get; protected set; } = Matrix.Identity;

    public Vector2 GlobalToLocal(Vector2 global) {
        return Vector2.Transform(global, Transform);
    }

    public Vector2 LocalToGlobal(Vector2 local) {
        return Vector2.Transform(local, Matrix.Invert(Transform));
    }

    public virtual void Update(GameTime gameTime, InputManager inputManager) { }
}