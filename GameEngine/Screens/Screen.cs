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
    public Camera Camera { protected get; set; }
    private DebugScreen DebugScreen { get; set; }

    protected Screen() {
        Camera = new Camera();
    }

    protected Vector2 GlobalToLocal(Vector2 global) {
        return Camera.GlobalToLocal(global);
    }

    protected Vector2 LocalToGlobal(Vector2 local) {
        return Camera.LocalToGlobal(local);
    }

    protected void EnableDebug() {
        DebugScreen = new DebugScreen(this);
        ScreenStack.PushScreen(DebugScreen);
    }

    protected void DisableDebug() {
        ScreenStack.RemoveScreen(DebugScreen);
        DebugScreen = null;
    }

    protected void ToggleDebug() {
        if (DebugScreen is null) {
            EnableDebug();
        } else {
            DisableDebug();
        }
    }

    protected T GetDebugInput<T>(string name, T defaultValue) {
        return DebugScreen is not null ? DebugScreen.GetDebugValue(name, defaultValue) : defaultValue;
    }

    public void EarlyUpdate(GameTime gameTime, InputManager inputManager) {
        Camera.Update(gameTime, inputManager);
        inputManager.LocalCursorPosition = GlobalToLocal(inputManager.GlobalCursorPosition);
    }

    public abstract void Update(GameTime gameTime, InputManager inputManager);
    public abstract void UpdateDebug(GameTime gameTime, InputManager inputManager);

    public void LateUpdate(GameTime gameTime, InputManager inputManager) { }

    public void PreDraw(SpriteBatch spriteBatch) {
        spriteBatch.Begin(transformMatrix: Camera.Transform);
    }

    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void DrawDebug(SpriteBatch spriteBatch);

    public void PostDraw(SpriteBatch spriteBatch) {
        if (DebugScreen is not null) {
            DrawDebug(spriteBatch);
        }

        spriteBatch.End();
    }
}