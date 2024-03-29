﻿using System.Collections.Generic;
using GameEngine.Input;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Ui;

public enum UiDockType {
    Absolute,
    Fill,
    Center,
    Left,
    Right,
    Top,
    Bottom
}

public abstract class UiElement {
    /// <summary>
    /// 
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool IsEnabled {
        get => IsVisible && mIsEnabled;
        set => mIsEnabled = value;
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector4 Padding {
        get => mPadding;
        set {
            mPadding = value;
            mIsDirty = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector2 PositionRelative {
        get => mPositionRelative;
        set {
            mPositionRelative = value;
            mIsDirty = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector2 SizeRelative {
        get => mSizeRelative;
        set {
            mSizeRelative = value;
            mIsDirty = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Color Background { get; set; } = Color.Transparent;

    /// <summary>
    /// 
    /// </summary>
    public UiDockType DockType {
        get => mDockType;
        set {
            mDockType = value;
            mIsDirty = true;
        }
    }

    protected Rectangle BorderBounds { get; set; }
    protected Rectangle ContentBounds { get; set; }
    protected UiElement Parent { get; set; }
    protected List<UiElement> Children { get; }

    private Vector2 mSizeRelative = Vector2.One;
    private Vector2 mPositionRelative = Vector2.Zero;
    private bool mIsEnabled = true;
    private UiDockType mDockType;
    private bool mIsDirty;
    private Vector4 mPadding;

    protected UiElement() {
        Children = new List<UiElement>();
    }

    public void AddElement(UiElement child) {
        Children.Add(child);
        child.Parent = this;
        OnElementAdded(child);
    }

    protected virtual void OnElementAdded(UiElement child) { }

    public void RemoveElement(UiElement child) {
        if (Children.Remove(child)) {
            child.Parent = null;
        }
    }

    public void Update(GameTime gameTime, InputManager inputManager) {
        if (mIsDirty) {
            RedoDocking();
            RedoBounds();
        }

        if (!IsVisible || !IsEnabled) {
            return;
        }

        // update yourself first
        UpdateSelf(gameTime, inputManager);

        // update all children
        foreach (var child in Children) {
            child.Update(gameTime, inputManager);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        if (!IsVisible) {
            return;
        }

        // draw the background
        if (Background != Color.Transparent) {
            spriteBatch.DrawFilledRectangle(ContentBounds, Background);
        }

        // draw itself
        DrawSelf(spriteBatch);

        // draw all children
        foreach (var child in Children) {
            child.Draw(spriteBatch);
        }
    }

    protected abstract void UpdateSelf(GameTime gameTime, InputManager inputManager);
    protected abstract void DrawSelf(SpriteBatch spriteBatch);

    protected bool MouseOver(InputManager inputManager) {
        return ContentBounds.Contains(inputManager.GlobalCursorPosition);
    }

    private void RedoDocking() {
        // no special calculations are required for absolute docking
        if (DockType == UiDockType.Absolute) {
            return;
        }

        // fill docks the UiElement to the top left and makes it as big as its parents bounds
        if (DockType == UiDockType.Fill) {
            mPositionRelative = Vector2.Zero;
            mSizeRelative = Vector2.One;
            return;
        }

        // center, centers the UiElement relative to its parents bounds
        if (DockType == UiDockType.Center) {
            mPositionRelative = new Vector2 {
                X = 0.5f - SizeRelative.X / 2f,
                Y = 0.5f - SizeRelative.Y / 2f
            };

            return;
        }

        // the position is either left or right...
        if (DockType is UiDockType.Left or UiDockType.Right) {
            mPositionRelative.Y = 0.5f - SizeRelative.Y / 2f;
            mPositionRelative.X = DockType is UiDockType.Left
                ? 0f
                : 1f - SizeRelative.X;
        }

        // ...and top or bottom
        if (DockType is UiDockType.Top or UiDockType.Bottom) {
            mPositionRelative.X = 0.5f - SizeRelative.X / 2f;
            mPositionRelative.Y = DockType is UiDockType.Top
                ? 0f
                : 1f - SizeRelative.Y;
        }
    }

    private void RedoBounds() {
        var (xRelative, yRelative) = PositionRelative;
        var x = Parent.ContentBounds.X + Parent.ContentBounds.Width * xRelative;
        var y = Parent.ContentBounds.Y + Parent.ContentBounds.Height * yRelative;

        var width = Parent.ContentBounds.Width * mSizeRelative.X;
        var height = Parent.ContentBounds.Height * mSizeRelative.Y;

        BorderBounds = new Rectangle {
            X = (int)x,
            Y = (int)y,
            Width = (int)width,
            Height = (int)height
        };

        ContentBounds = new Rectangle {
            X = (int)(x + Padding.X),
            Y = (int)(y + Padding.Y),
            Width = (int)(width - Padding.X - Padding.W),
            Height = (int)(height - Padding.Y - Padding.Z)
        };
    }
}