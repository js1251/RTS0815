﻿using System;
using GameEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Ui;

public sealed class Button : UiElement {
    public Action OnClick { get; set; }

    public Color TextColor {
        get => mLabel.TextColor;
        set => mLabel.TextColor = value;
    }

    private readonly Label mLabel;

    public Button(string text = "") {
        mLabel = new Label(text) {
            DockType = UiDockType.Center
        };

        AddElement(mLabel);
    }

    protected override void UpdateSelf(GameTime gameTime, InputManager inputManager) {
        // check if cursor is over button
        if (!MouseOver(inputManager)) {
            return;
        }

        // check if button is clicked
        if (!inputManager.JustPressed(InputAction.LeftClick)) {
            return;
        }

        // trigger click event
        OnClick?.Invoke();

        // consume input
        inputManager.Consume(InputAction.LeftClick);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch) { }
}