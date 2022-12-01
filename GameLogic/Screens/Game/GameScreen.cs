using GameEngine.Assets;
using GameEngine.Input;
using GameEngine.Rendering;
using GameEngine.Screens;
using GameLogic.Screens.Pause;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Screens.Game;

internal class GameScreen : Screen {
    private float mDebugValue;

    internal GameScreen() {
        Camera = new TransformCamera();
        DrawScreen = true;
        UpdateScreen = true;
        DrawLower = false;
        UpdateLower = false;
    }

    public override void Update(GameTime gameTime, InputManager inputManager) {
        if (inputManager.JustPressed(InputAction.Quit)) {
            ScreenStack.PopScreen();
            inputManager.Consume(InputAction.Quit);
        } else if (inputManager.JustPressed(InputAction.Pause)) {
            ScreenStack.PushScreen(new PauseScreen());
            inputManager.Consume(InputAction.Pause);
        } else if (inputManager.JustPressed(InputAction.Enter)) {
            ToggleDebug();
            inputManager.Consume(InputAction.Enter);
        }

        mDebugValue = GetDebugInput<float>("float", 0.15f);
        GetDebugInput<int>("int", 0);
        GetDebugInput<double>("double", 0d);
        GetDebugInput<Vector2>("vector2", Vector2.Zero);
    }

    public override void UpdateDebug(GameTime gameTime, InputManager inputManager) { }

    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.DrawString(AssetStore.Fonts["Calibri"], "" + mDebugValue, new Vector2(0, 0), 48, Color.White);
    }

    public override void DrawDebug(SpriteBatch spriteBatch) {
        // origin axis lines
        spriteBatch.DrawArrow(Vector2.Zero, Vector2.UnitX * 100, Color.Red);
        spriteBatch.DrawArrow(Vector2.Zero, Vector2.UnitY * 100, Color.Green);
    }
}