using System;
using GameEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Ui;

public enum StackPanelOrientation {
    Vertical,
    Horizontal
}

public class StackPanel : UiElement {
    public StackPanelOrientation Orientation { get; set; }

    protected override void UpdateSelf(GameTime gameTime, InputManager inputManager) { }

    protected override void DrawSelf(SpriteBatch spriteBatch) { }
}