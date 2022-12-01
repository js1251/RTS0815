using GameEngine.Assets;
using GameEngine.Input;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Ui;

public class Label : UiElement {
    public string Text { get; set; }
    public Color TextColor { get; set; } = Color.Black;

    public Label(string text = "") {
        Text = text;
    }

    protected override void UpdateSelf(GameTime gameTime, InputManager inputManager) { }

    protected override void DrawSelf(SpriteBatch spriteBatch) {
        spriteBatch.DrawString(AssetStore.Fonts["Calibri"], Text, ContentBounds.Location.ToVector2(), ContentBounds.Height, TextColor);
    }
}