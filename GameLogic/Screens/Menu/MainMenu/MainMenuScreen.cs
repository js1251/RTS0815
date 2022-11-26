using GameEngine.Input;
using GameEngine.Rendering;
using GameEngine.Screens;
using GameLogic.Screens.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Screens.Menu.MainMenu;

internal class MainMenuScreen : Screen {
    public MainMenuScreen() {
        Camera = new Camera();

        UpdateScreen = true;
        DrawScreen = true;
        UpdateLower = false;
        DrawLower = false;
    }

    public override void Update(GameTime gameTime, InputManager inputManager) {
        base.Update(gameTime, inputManager);

        if (inputManager.JustPressed(InputAction.Enter)) {
            ScreenStack.PushScreen(new GameScreen());
            inputManager.Consume(InputAction.Enter);
        } else if (inputManager.JustPressed(InputAction.Quit)) {
            ScreenStack.PopScreen();
            inputManager.Consume(InputAction.Quit);
        }
    }

    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.DrawFilledSquare(new Vector2(100, 100), 100, Color.Blue);
    }
}