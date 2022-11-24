using GameEngine.Input;
using GameEngine.Screens;
using GameLogic.Screens.Pause;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Screens.Game;

internal class GameScreen : IScreen {
    public bool DrawScreen { get; set; }
    public bool UpdateScreen { get; set; }
    public bool DrawLower { get; set; }
    public bool UpdateLower { get; set; }
    public ScreenManager ScreenManager { get; set; }

    internal GameScreen() {
        DrawScreen = true;
        UpdateScreen = true;
        DrawLower = false;
        UpdateLower = false;
    }

    public void Update(GameTime gameTime, InputManager inputManager) {
        if (inputManager.JustPressed(InputAction.Quit)) {
            ScreenManager.PopScreen();
            inputManager.Consume(InputAction.Quit);
        } else if (inputManager.JustPressed(InputAction.Pause)) {
            ScreenManager.PushScreen(new PauseScreen());
            inputManager.Consume(InputAction.Pause);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        System.Diagnostics.Debug.WriteLine("Drawing game screen");
    }
}