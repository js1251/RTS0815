using GameEngine.Input;
using GameEngine.Screens;
using GameLogic.Screens.Menu.MainMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLogic;

public class RTS0815 : Game {
    private GraphicsDeviceManager mGraphics;
    private SpriteBatch mSpriteBatch;

    private readonly ScreenManager mScreenManager;
    private readonly InputManager mInputManager;

    public RTS0815() {
        mGraphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        mInputManager = new InputManager();

        mScreenManager = new ScreenManager(mInputManager);
        mScreenManager.PushScreen(new MainMenuScreen());
    }

    protected override void Initialize() {
        // Initialize() is not used anywhere else as a design decision.
        // it is required here for a couple things however.
        // Namely: Setting the window title
        base.Initialize();
        Window.Title = "RTS 0815";
    }

    protected override void LoadContent() {
        mSpriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime) {
        base.Update(gameTime);

        // inputmanager needs to be updated as soon as possible
        mInputManager.Update(gameTime, Keyboard.GetState(), Mouse.GetState());

        // update all screens
        mScreenManager.Update(gameTime);

        // quit game if no screens are left
        if (mScreenManager.IsEmpty()) {
            // TODO: do stuff on exit?
            Exit();
        }
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);

        mScreenManager.Draw(mSpriteBatch);
    }
}