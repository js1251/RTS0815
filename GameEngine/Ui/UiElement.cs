using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Ui;

public abstract class UiElement {
    public bool IsVisible { get; set; } = true;
    public Vector4 Margin { get; set; }
    public Vector2 PositionRelative { get; set; }
    public Vector2 Size { get; set; }

    private UiElement Parent { get; set; }
    private List<UiElement> Children { get; }

    public UiElement() {
        Children = new List<UiElement>();
    }

    private void AddChild(UiElement child) {
        Children.Add(child);
        child.Parent = this;
    }

    private void RemoveChild(UiElement child) {
        Children.Remove(child);
        child.Parent = null;
    }

    public void Update(GameTime gameTime) {
        if (!IsVisible) {
            return;
        }

        UpdateElement(gameTime);

        foreach (var child in Children) {
            child.Update(gameTime);
        }
    }


    public void Draw(SpriteBatch spriteBatch) {
        if (!IsVisible) {
            return;
        }

        DrawElement(spriteBatch);

        foreach (var child in Children) {
            child.Draw(spriteBatch);
        }
    }

    public abstract void UpdateElement(GameTime gameTime);
    public abstract void DrawElement(SpriteBatch spriteBatch);
}