using GameEngine.Input;
using GameEngine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Screens.Pause;

internal class PauseScreen : IScreen {
    public bool DrawScreen => true;
    public bool UpdateScreen => true;
    public bool DrawLower => true;
    public bool UpdateLower => false;
    public ScreenManager ScreenManager { get; set; }

    public void Update(GameTime gameTime, InputManager inputManager) {
        if (inputManager.JustPressed(InputAction.Pause)) {
            ScreenManager.PopScreen();
            inputManager.Consume(InputAction.Pause);
        } else if (inputManager.JustPressed(InputAction.Quit)) {
            ScreenManager.PopScreen();
            inputManager.Consume(InputAction.Quit);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        System.Diagnostics.Debug.WriteLine("Drawing pause screen");
    }
}