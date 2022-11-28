using GameEngine.Input;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Ui;

public sealed class RootPane : UiElement {
    public RootPane() {
        SetBoundsToScreen();
    }

    protected override void UpdateSelf(GameTime gameTime, InputManager inputManager) {
        // update bounds if ScrW, ScrH changes
        if (ContentBounds.Width != Application.ScrW || ContentBounds.Height != Application.ScrH) {
            SetBoundsToScreen();
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch) { }

    private void SetBoundsToScreen() {
        ContentBounds = new Rectangle(0, 0, Application.ScrW, Application.ScrH);
    }
}