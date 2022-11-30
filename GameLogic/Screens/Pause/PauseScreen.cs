using GameEngine.Input;
using GameEngine.Rendering;
using GameEngine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Screens.Pause;

internal class PauseScreen : Screen {
    public PauseScreen() {
        Camera = new Camera();

        UpdateScreen = true;
        DrawScreen = true;
        UpdateLower = false;
        DrawLower = true;
    }

    public override void Update(GameTime gameTime, InputManager inputManager) {
        if (inputManager.JustPressed(InputAction.Pause)) {
            ScreenStack.PopScreen();
            inputManager.Consume(InputAction.Pause);
        } else if (inputManager.JustPressed(InputAction.Quit)) {
            ScreenStack.PopScreen();
            inputManager.Consume(InputAction.Quit);
        }
    }

    public override void UpdateDebug(GameTime gameTime, InputManager inputManager) { }

    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.DrawFilledSquare(new Vector2(100, 100), 100, Color.Green);
    }

    public override void DrawDebug(SpriteBatch spriteBatch) { }
}