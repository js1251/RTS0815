using System.Collections.Generic;
using GameEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Screens;

public sealed class ScreenStack {
    private readonly LinkedList<Screen> mScreens;
    private readonly DebugScreen mDebugScreen;
    private readonly InputManager mInputManager;

    public ScreenStack(InputManager inputManager) {
        mScreens = new LinkedList<Screen>();

        mDebugScreen = new DebugScreen();
        mInputManager = inputManager;
    }

    public void PushScreen(Screen screen) {
        mScreens.AddFirst(screen);
        screen.ScreenStack = this;
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

        // setting every update in case there are multiple ScreenStacks
        ScreenContext.DebugScreen = mDebugScreen;

        if (mInputManager.JustPressed(InputAction.Enter)) {
            ScreenContext.ToggleDebug();
            mInputManager.Consume(InputAction.Enter);
        }

        // update all screens
        UpdateScreen(gameTime, mScreens.First);

        // update debug screen if enabled
        if (ScreenContext.DebugEnabled) {
            mDebugScreen.EarlyUpdate(gameTime, mInputManager);
            mDebugScreen.Update(gameTime, mInputManager);
            mDebugScreen.LateUpdate(gameTime, mInputManager);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        if (IsEmpty()) {
            return;
        }

        // draw all screens
        DrawScreen(spriteBatch, mScreens.First);

        // draw debug screen if enabled
        if (ScreenContext.DebugEnabled) {
            mDebugScreen.PreDraw(spriteBatch);
            mDebugScreen.Draw(spriteBatch);
            mDebugScreen.PostDraw(spriteBatch);
        }
    }

    private void UpdateScreen(GameTime gameTime, LinkedListNode<Screen> screenNode) {
        // Note: Update is done top to bottom

        var screen = screenNode.Value;

        // update screen context
        ScreenContext.CurrentScreen = screen;

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

        // update screen context
        ScreenContext.CurrentScreen = screen;

        // if the screen below needs to be drawn, draw it first
        if (screen.DrawLower) {
            DrawScreen(spriteBatch, screenNode.Next);
        }

        // if the screen itself needs to be drawn, draw it
        if (screen.DrawScreen) {
            screen.PreDraw(spriteBatch);
            screen.Draw(spriteBatch);
            ScreenContext.DrawDebug();
            screen.PostDraw(spriteBatch);
        }
    }
}