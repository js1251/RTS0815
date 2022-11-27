using GameEngine.Input;
using Microsoft.Xna.Framework;

namespace GameEngine.Rendering;

public class Camera {
    /// <summary>
    /// The cameras transform matrix.
    /// </summary>
    public Matrix Transform { get; protected set; } = Matrix.Identity;

    /// <summary>
    /// Transforms a given point from global coordinates (literal application window) to local coordinates (the screen the camera is in).
    /// </summary>
    /// <param name="global">The point to transform.</param>
    /// <returns>A point relative to the cameras screen.</returns>
    public Vector2 GlobalToLocal(Vector2 global) {
        return Vector2.Transform(global, Transform);
    }

    /// <summary>
    /// Transforms a given point from local coordinates (the screen the camera is in) to global coordinates (literal application window).
    /// </summary>
    /// <param name="local">The point to transform.</param>
    /// <returns>A point relative to the applications window.</returns>
    public Vector2 LocalToGlobal(Vector2 local) {
        return Vector2.Transform(local, Matrix.Invert(Transform));
    }

    public virtual void Update(GameTime gameTime, InputManager inputManager) { }
}