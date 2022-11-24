using System.Collections.Generic;
using GameEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Screens;

public sealed class ScreenManager {
    private readonly LinkedList<IScreen> mScreens;
    private readonly InputManager mInputManager;

    public ScreenManager(InputManager inputManager) {
        mScreens = new LinkedList<IScreen>();

        mInputManager = inputManager;
    }

    public void PushScreen(IScreen screen) {
        mScreens.AddFirst(screen);
        screen.ScreenManager = this;
    }

    public void PopScreen() {
        mScreens.RemoveFirst();
    }

    public bool IsEmpty() {
        return mScreens.Count <= 0;
    }

    public void Update(GameTime gameTime) {
        if (IsEmpty()) {
            return;
        }

        UpdateScreen(gameTime, mScreens.First);
    }

    public void Draw(SpriteBatch spriteBatch) {
        if (IsEmpty()) {
            return;
        }

        DrawScreen(spriteBatch, mScreens.First);
    }

    private void UpdateScreen(GameTime gameTime, LinkedListNode<IScreen> screenNode) {
        // Note: Update is done top to bottom

        var screen = screenNode.Value;

        // if the screen itself needs to be updated, draw it
        if (screen.UpdateScreen) {
            screen.Update(gameTime, mInputManager);
        }

        // if the screen below needs to be updated, update it first
        if (screen.UpdateLower) {
            UpdateScreen(gameTime, screenNode.Next);
        }
    }

    private static void DrawScreen(SpriteBatch spriteBatch, LinkedListNode<IScreen> screenNode) {
        // Note: Draw is done bottom to top

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