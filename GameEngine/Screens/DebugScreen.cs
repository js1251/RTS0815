using System.Collections.Generic;
using GameEngine.Debug;
using GameEngine.Input;
using GameEngine.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Screens;

internal class DebugScreen : Screen {
    private readonly Screen mParentScreen;
    private readonly Dictionary<string, StringValueParser> mDebugValues;
    private readonly Dictionary<string, TextBox> mDebugInputs;
    private readonly RootPane mDebugInputPanel;
    private readonly StackPanel mDebugStackPanel;

    internal DebugScreen(Screen parent) {
        UpdateLower = true;
        DrawLower = true;

        mParentScreen = parent;
        mDebugValues = new Dictionary<string, StringValueParser>();
        mDebugInputs = new Dictionary<string, TextBox>();

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

    internal T GetDebugValue<T>(string name, T defaultValue) {
        if (mDebugValues.ContainsKey(name)) {
            mDebugValues[name].Input = mDebugInputs[name].Text;
            return mDebugValues[name].ParseAs<T>();
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
        var input = new TextBox {
            DockType = UiDockType.Right,
            Text = defaultValue.ToString(),
            SizeRelative = new Vector2(0.5f, 1),
        };
        horizontalStackPanel.AddElement(input);

        // add the horizontal stackpanel to the vertical stackpanel of debug input fields
        mDebugStackPanel.AddElement(horizontalStackPanel);

        // save the reference
        mDebugInputs.Add(name, input);
        mDebugValues.Add(name, new StringValueParser(defaultValue));

        // if the input field was just created its value cannot have changed from the default value
        return defaultValue;
    }

    public override void Update(GameTime gameTime, InputManager inputManager) {
        mDebugInputPanel.Update(gameTime, inputManager);
        mParentScreen.UpdateDebug(gameTime, inputManager);
    }

    public override void UpdateDebug(GameTime gameTime, InputManager inputManager) { }

    public override void Draw(SpriteBatch spriteBatch) {
        mDebugInputPanel.Draw(spriteBatch);
    }

    public override void DrawDebug(SpriteBatch spriteBatch) { }
}