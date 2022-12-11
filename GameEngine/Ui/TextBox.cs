using GameEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Ui;

public class TextBox : UiElement {
    public string Placeholder {
        get => mPlaceholderLabel.Text;
        set => mPlaceholderLabel.Text = value;
    }

    public string Text {
        get => mTextLabel.Text;
        set => mTextLabel.Text = value;
    }

    private readonly Label mPlaceholderLabel;
    private readonly Label mTextLabel;
    private bool mIsFocused;

    public TextBox() {
        mPlaceholderLabel = new Label {
            DockType = UiDockType.Fill,
        };
        
        mTextLabel = new Label {
            DockType = UiDockType.Fill,
        };

        AddElement(mTextLabel);
        AddElement(mPlaceholderLabel);

        // default background
        Background = Color.White;
    }

    protected override void UpdateSelf(GameTime gameTime, InputManager inputManager) {
        // if the textbox does not contain text, show the placeholder
        mPlaceholderLabel.IsVisible = string.IsNullOrEmpty(Text);

        // toggle focus when the user clicks on the textbox
        if (inputManager.JustPressed(InputAction.LeftClick)) {
            mIsFocused = ContentBounds.Contains(inputManager.GlobalCursorPosition);

            if (mIsFocused) {
                inputManager.Consume(InputAction.LeftClick);
            }
        }

        if (!mIsFocused) {
            return;
        }

        if (inputManager.TextInput.Length <= 0) {
            return;
        }

        Text ??= "";

        // handle text input
        foreach (var character in inputManager.TextInput) {
            if (character == '\b') {
                // backspace
                if (Text.Length > 0) {
                    Text = Text[..^1];
                }
            } else if (character == '\r') {
                // enter
                mIsFocused = false;
            } else {
                // other chars
                Text += character;
            }
        }

        mTextLabel.Text = Text;

        // all inputs are considered consumed when the textbox is focused
        inputManager.ConsumeAll();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch) {
        // if the textbox is focused, show a carret
    }
}