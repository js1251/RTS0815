using GameEngine.Input;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Screens;

public abstract class Screen {
    public bool DrawScreen { get; protected set; }
    public bool UpdateScreen { get; protected set; }
    public bool DrawLower { get; protected set; }
    public bool UpdateLower { get; protected set; }
    public ScreenStack ScreenStack { protected get; set; }
    public Camera Camera { protected get; set; }

    protected Vector2 GlobalToLocal(Vector2 global) {
        return Camera.GlobalToLocal(global);
    }

    protected Vector2 LocalToGlobal(Vector2 local) {
        return Camera.LocalToGlobal(local);
    }


    public virtual void Update(GameTime gameTime, InputManager inputManager) {
        Camera.Update(gameTime, inputManager);
        inputManager.LocalCursorPosition = GlobalToLocal(inputManager.GlobalCursorPosition);
    }

    public void PreDraw(SpriteBatch spriteBatch) {
        spriteBatch.Begin(transformMatrix: Camera.Transform);
    }

    public abstract void Draw(SpriteBatch spriteBatch);

    public void PostDraw(SpriteBatch spriteBatch) {
        spriteBatch.End();
    }
}