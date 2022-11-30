using System.Collections.Generic;
using GameEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Screens;

public sealed class ScreenStack {
    private readonly LinkedList<Screen> mScreens;
    private readonly InputManager mInputManager;

    public ScreenStack(InputManager inputManager) {
        mScreens = new LinkedList<Screen>();

        mInputManager = inputManager;
    }

    public void PushScreen(Screen screen) {
        mScreens.AddFirst(screen);
        screen.ScreenStack = this;
    }

    public void PopScreen() {
        mScreens.RemoveFirst();
    }

    public void RemoveScreen(Screen screen) {
        mScreens.Remove(screen);
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

    private void UpdateScreen(GameTime gameTime, LinkedListNode<Screen> screenNode) {
        // Note: Update is done top to bottom

        var screen = screenNode.Value;

        // if the screen itself needs to be updated, draw it
        if (screen.UpdateScreen) {
            screen.EarlyUpdate(gameTime, mInputManager);
            screen.Update(gameTime, mInputManager);
            screen.LateUpdate(gameTime, mInputManager);
        }

        // if the screen below needs to be updated, update it first
        if (screen.UpdateLower) {
            UpdateScreen(gameTime, screenNode.Next);
        }
    }

    private static void DrawScreen(SpriteBatch spriteBatch, LinkedListNode<Screen> screenNode) {
        // Note: Draw is done bottom to top

        var screen = screenNode.Value;

        // if the screen below needs to be drawn, draw it first
        if (screen.DrawLower) {
            DrawScreen(spriteBatch, screenNode.Next);
        }

        // if the screen itself needs to be drawn, draw it
        if (screen.DrawScreen) {
            screen.PreDraw(spriteBatch);
            screen.Draw(spriteBatch);
            screen.PostDraw(spriteBatch);
        }
    }
}