using GameEngine.Input;
using GameEngine.Screens;
using GameLogic.Screens.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Screens.Menu.MainMenu;

internal class MainMenuScreen : IScreen {
    public bool DrawScreen => true;
    public bool UpdateScreen => true;
    public bool DrawLower => false;
    public bool UpdateLower => false;
    public ScreenManager ScreenManager { get; set; }

    public void Update(GameTime gameTime, InputManager inputManager) {
        if (inputManager.JustPressed(InputAction.Enter)) {
            ScreenManager.PushScreen(new GameScreen());
            inputManager.Consume(InputAction.Enter);
        } else if (inputManager.JustPressed(InputAction.Quit)) {
            ScreenManager.PopScreen();
            inputManager.Consume(InputAction.Quit);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        System.Diagnostics.Debug.WriteLine("Drawing main menu screen");
    }
}