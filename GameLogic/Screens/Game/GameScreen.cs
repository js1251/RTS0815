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
        base.Update(gameTime, inputManager);

        if (inputManager.JustPressed(InputAction.Quit)) {
            ScreenStack.PopScreen();
            inputManager.Consume(InputAction.Quit);
        } else if (inputManager.JustPressed(InputAction.Pause)) {
            ScreenStack.PushScreen(new PauseScreen());
            inputManager.Consume(InputAction.Pause);
        }
    }

    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.DrawFilledSquare(new Vector2(100, 100), 100, Color.Red);
    }
}