using System.Collections.Generic;
using GameEngine.Debugging;
using GameEngine.Input;
using GameEngine.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Screens;

internal class DebugScreen : Screen {
    private class DebugInput<T> {
        private readonly StringValueParser<T> mParser;
        private readonly TextBox mTextBox;
        private T mValue;
        private string mPreviousInput = string.Empty;

        internal T Value {
            get {
                if (mPreviousInput.Equals(mTextBox.Text)) {
                    return mValue;
                }

                mParser.Input = mTextBox.Text;
                mParser.Parse();
                mValue = mParser.Value;
                mPreviousInput = mTextBox.Text;

                return mValue;
            }
        }

        internal DebugInput(T defaultValue, TextBox textBox) {
            mParser = new StringValueParser<T>(defaultValue);
            mTextBox = textBox;
        }
    }

    private readonly Dictionary<string, object> mDebugInputs;
    private readonly RootPane mDebugInputPanel;
    private readonly StackPanel mDebugStackPanel;

    internal DebugScreen() {
        UpdateLower = true;
        DrawLower = true;

        mDebugInputs = new Dictionary<string, object>();

        mDebugInputPanel = new RootPane();
        mDebugStackPanel = new StackPanel {
            Background = Color.White * 0.5f,
            PositionRelative = Vector2.Zero,
            SizeRelative = new Vector2(0.2f, 0.5f)
        };

        mDebugStackPanel.AddElement(new Label("Debug Inputs") {
            SizeRelative = new Vector2(1, 0.1f),
        });

        mDebugInputPanel.AddElement(mDebugStackPanel);
    }

    internal T DebugValue<T>(string name, T defaultValue) {
        if (mDebugInputs.ContainsKey(name)) {
            var debugInput = (DebugInput<T>)mDebugInputs[name];

            // TODO: check that name is not already used
            // --> check type of T of already existing input
            return debugInput.Value;
        }

        // create a horizontal stack panel for the label and input
        var horizontalStackPanel = new StackPanel {
            Orientation = StackPanelOrientation.Horizontal,
            SizeRelative = new Vector2(1, 0.1f),
            Padding = Vector4.One * 4,
        };

        // add the name of the input as a label
        horizontalStackPanel.AddElement(new Label(name + " (" + typeof(T).Name + "):") {
            DockType = UiDockType.Left,
            SizeRelative = new Vector2(0.5f, 1),
        });

        // add textbox next to label
        var textBox = new TextBox {
            DockType = UiDockType.Right,
            Text = defaultValue.ToString(),
            SizeRelative = new Vector2(0.5f, 1),
        };
        horizontalStackPanel.AddElement(textBox);

        // add the horizontal stackpanel to the vertical stackpanel of debug input fields
        mDebugStackPanel.AddElement(horizontalStackPanel);

        // save the reference
        mDebugInputs.Add(name, new DebugInput<T>(defaultValue, textBox));

        // if the input field was just created its value cannot have changed from the default value
        return defaultValue;
    }

    public override void Update(GameTime gameTime, InputManager inputManager) {
        mDebugInputPanel.Update(gameTime, inputManager);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        mDebugInputPanel.Draw(spriteBatch);
    }
}