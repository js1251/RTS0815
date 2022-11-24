namespace GameEngine.Screens;

public sealed class ScreenManager {
    private LinkedList<IScreen> mScreens;

    public ScreenManager() {
        mScreens = new LinkedList<IScreen>();
    }

    public void PushScreen(IScreen screen) {
        mScreens.AddFirst(screen);
    }

    public void PopScreen() {
        mScreens.RemoveFirst();
    }

    public void Update(GameTime gameTime) {
        UpdateScreen(gameTime, mScreens.First);
    }

    public void Draw(SpriteBatch spriteBatch) {
        DrawScreen(spriteBatch, mScreens.First);
    }

    private static void UpdateScreen(GameTime gameTime, LinkedListNode<IScreen> screenNode) {
        var screen = screenNode.Value;

        // if the screen below needs to be updated, update it first
        if (screen.UpdateLower) {
            UpdateScreen(gameTime, screenNode.Next);
        }

        // if the screen itself needs to be updated, draw it
        if (screen.UpdateScreen) {
            screen.Update(gameTime);
        }
    }

    private static void DrawScreen(SpriteBatch spriteBatch, LinkedListNode<IScreen> screenNode) {
        var screen = screenNode.Value;

        // if the screen below needs to be drawn, draw it first
        if (screen.DrawLower) {
            DrawScreen(spriteBatch, screenNode.Next);
        }

        // if the screen itself needs to be drawn, draw it
        if (screen.DrawScreen) {
            screen.Draw(spriteBatch);
        }
    }
}