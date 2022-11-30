using System;
using GameEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Ui;

public class Label : UiElement {
    private readonly string mText;

    public Label(string text) {
        mText = text;
    }

    protected override void UpdateSelf(GameTime gameTime, InputManager inputManager) { }

    protected override void DrawSelf(SpriteBatch spriteBatch) { }
}