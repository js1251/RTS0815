using GameEngine.Input;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Screens;

public abstract class Screen {
    public bool DrawScreen { get; protected set; } = true;
    public bool UpdateScreen { get; protected set; } = true;
    public bool DrawLower { get; protected set; } = false;
    public bool UpdateLower { get; protected set; } = false;
    public ScreenStack ScreenStack { protected get; set; }
    public Camera Camera { protected internal get; set; }

    protected Screen() {
        Camera = new Camera();
    }

    public void EarlyUpdate(GameTime gameTime, InputManager inputManager) {
        Camera.Update(gameTime, inputManager);
    }

    public abstract void Update(GameTime gameTime, InputManager inputManager);

    public void LateUpdate(GameTime gameTime, InputManager inputManager) { }

    public void PreDraw(SpriteBatch spriteBatch) {
        spriteBatch.Begin(transformMatrix: Camera.Transform);
    }

    public abstract void Draw(SpriteBatch spriteBatch);

    public void PostDraw(SpriteBatch spriteBatch) {
        spriteBatch.End();
    }
}