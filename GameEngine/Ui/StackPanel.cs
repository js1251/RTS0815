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

    protected override void OnElementAdded(UiElement child) {
        base.OnElementAdded(child);

        // no adjustments needed for first child
        if (Children.Count == 1) {
            return;
        }

        if (Orientation is StackPanelOrientation.Vertical) {
            DockVertical(child);
        } else {
            DockHorizontal(child);
        }
    }

    private void DockVertical(UiElement child) {
        var above = Children[^2];
        var abovePos = above.PositionRelative;
        var aboveSize = above.SizeRelative;

        child.PositionRelative = new Vector2(child.PositionRelative.X, abovePos.Y + aboveSize.Y);
    }

    private void DockHorizontal(UiElement child) {
        var left = Children[^2];
        var leftPos = left.PositionRelative;
        var leftSize = left.SizeRelative;

        child.PositionRelative = new Vector2(leftPos.X + leftSize.X, child.PositionRelative.Y);
    }

    protected override void UpdateSelf(GameTime gameTime, InputManager inputManager) { }

    protected override void DrawSelf(SpriteBatch spriteBatch) { }
}