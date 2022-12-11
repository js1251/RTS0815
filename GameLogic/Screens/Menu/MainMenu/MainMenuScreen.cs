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

        var stackPanel = new StackPanel {
            SizeRelative = new Vector2(0.4f, 0.8f),
            DockType = UiDockType.Center
        };
        
        mMenuRootPane.AddElement(stackPanel);

        stackPanel.AddElement(new Button("Start Game") {
            SizeRelative = new Vector2(1f, 0.1f),
            Padding = Vector4.One * 4,
            Background = Color.Blue,
            TextColor = Color.White,
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

    public override void Draw(SpriteBatch spriteBatch) {
        mMenuRootPane.Draw(spriteBatch);
    }
}