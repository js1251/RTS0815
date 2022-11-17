using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic;

public class Game1 : Game {
    private GraphicsDeviceManager mGraphics;
    private SpriteBatch mSpriteBatch;

    public Game1() {
        mGraphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
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
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
    }
}