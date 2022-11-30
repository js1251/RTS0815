using GameEngine.Input;
using GameEngine.Rendering;
using GameEngine.Screens;
using GameEngine.Ui;
using GameLogic.Screens.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Screens.Menu.MainMenu;

internal class MainMenuScreen : Screen {
    private readonly RootPane mMenuRootPane;

    public MainMenuScreen() {
        Camera = new Camera();

        UpdateScreen = true;
        DrawScreen = true;
        UpdateLower = false;
        DrawLower = false;

        mMenuRootPane = new RootPane();
        mMenuRootPane.AddElement(new Button("Test Button") {
            SizeRelative = new Vector2(0.1f, 0.05f),
            DockType = UiDockType.Center,
            Background = Color.Green,
            OnClick = () => ScreenStack.PushScreen(new GameScreen())
        });
    }

    public override void Update(GameTime gameTime, InputManager inputManager) {
        if (inputManager.JustPressed(InputAction.Quit)) {
            ScreenStack.PopScreen();
            inputManager.Consume(InputAction.Quit);
        }

        mMenuRootPane.Update(gameTime, inputManager);
    }

    public override void UpdateDebug(GameTime gameTime, InputManager inputManager) {
        throw new System.NotImplementedException();
    }

    public override void Draw(SpriteBatch spriteBatch) {
        mMenuRootPane.Draw(spriteBatch);
    }

    public override void DrawDebug(SpriteBatch spriteBatch) {
        throw new System.NotImplementedException();
    }
}