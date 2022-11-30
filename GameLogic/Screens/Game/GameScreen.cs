using GameEngine.Input;
using GameEngine.Rendering;
using GameEngine.Screens;
using GameLogic.Screens.Pause;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Screens.Game;

internal class GameScreen : Screen {
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

        var test = GetDebugInput("test", 0);
    }

    public override void UpdateDebug(GameTime gameTime, InputManager inputManager) { }

    public override void Draw(SpriteBatch spriteBatch) { }

    public override void DrawDebug(SpriteBatch spriteBatch) {
        // origin axis lines
        spriteBatch.DrawArrow(Vector2.Zero, Vector2.UnitX * 100, Color.Red);
        spriteBatch.DrawArrow(Vector2.Zero, Vector2.UnitY * 100, Color.Green);
    }
}