using System;
using System.Collections.Generic;
using GameEngine.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Input;

public enum InputAction {
    Up,
    Down,
    Left,
    Right,
    Enter,
    Quit,
    Pause,

    LeftClick,
    RightClick,
    MiddleClick,
}

public enum MouseButtons {
    Left,
    Right,
    Middle
}

public sealed class InputManager {
    /// <summary>
    /// The current position of the cursor on the application window.
    /// </summary>
    public Vector2 GlobalCursorPosition { get; private set; }

    /// <summary>
    /// The current position of the cursor on the local screen.
    /// </summary>
    // public Vector2 LocalCursorPosition { get; set; }

    /// <summary>
    /// The current scroll wheel value.
    /// </summary>
    public int CursorScrollValue { get; private set; }

    /// <summary>
    /// The difference between the current and the previous scroll wheel value.
    /// </summary>
    public int CursorScrollValueDelta { get; private set; }

    /// <summary>
    /// The raw text input of the keyboard.
    /// </summary>
    public string TextInput { get; private set; } = string.Empty;

    private Dictionary<Keys, InputAction> mKeyMapping;
    private Dictionary<MouseButtons, InputAction> mMouseMapping;

    private readonly HashSet<InputAction> mCurrentActions;
    private HashSet<InputAction> mPreviousActions;
    private HashSet<InputAction> mConsumedActions;
    private readonly Dictionary<InputAction, double> mLastActionTimes;
    private readonly Dictionary<InputAction, double> mHoldTimes;
    private double mCurrentTime;
    private double mDeltaTime;
    private Keys mLastCharInput;

    /// <summary>
    /// The InputManager abstracts hardware inputs into <see cref="InputAction"/>.
    /// Provides methods to check if certain input criteria are met.
    /// </summary>
    public InputManager() {
        mCurrentActions = new HashSet<InputAction>();
        mPreviousActions = new HashSet<InputAction>();
        mConsumedActions = new HashSet<InputAction>();

        (mKeyMapping, mMouseMapping) = LoadInputMapping();
        (mLastActionTimes, mHoldTimes) = InitializeActionTimes();
    }

    /// <summary>
    /// Sets the <see cref="Keys"/> mapping for the given <see cref="InputAction"/>.
    /// </summary>
    /// <param name="action">The action that should be triggered if the given <see cref="Keys"/> is pressed</param>
    /// <param name="input">The <see cref="Keys"/> that needs to be pressed to trigger the <see cref="InputAction"/>.</param>
    public void SetMapping(InputAction action, Keys input) {
        if (mKeyMapping.ContainsKey(input)) {
            mKeyMapping[input] = action;
            return;
        }

        mKeyMapping.Add(input, action);
    }

    /// <summary>
    /// Sets the mouse button mapping for the given <see cref="InputAction"/>.
    /// </summary>
    /// <param name="action">The <see cref="InputAction"/> that should be triggered if the given <see cref="MouseButtons"/> is pressed</param>
    /// <param name="input">The <see cref="MouseButtons"/> that needs to be pressed to trigger the <see cref="InputAction"/>.</param>
    public void SetMapping(InputAction action, MouseButtons input) {
        if (mMouseMapping.ContainsKey(input)) {
            mMouseMapping[input] = action;
            return;
        }

        mMouseMapping.Add(input, action);
    }

    /// <summary>
    /// Removes the input mapping for the given <see cref="InputAction"/>.
    /// </summary>
    /// <param name="action">The <see cref="InputAction"/> to remove mappings for.</param>
    public void RemoveMapping(InputAction action) {
        if (mKeyMapping.ContainsValue(action)) {
            foreach (var key in mKeyMapping.Keys) {
                if (mKeyMapping[key] != action) {
                    continue;
                }

                mKeyMapping.Remove(key);
                break;
            }
        }

        if (mMouseMapping.ContainsValue(action)) {
            foreach (var key in mMouseMapping.Keys) {
                if (mMouseMapping[key] != action) {
                    continue;
                }

                mMouseMapping.Remove(key);
                break;
            }
        }
    }

    /// <summary>
    /// Resets the key/ mouse to <see cref="InputAction"/> mapping to the default values.
    /// </summary>
    public void ResetMapping() {
        (mKeyMapping, mMouseMapping) = LoadDefaultMapping();
    }

    /// <summary>
    /// Checks if a given <see cref="InputAction"/> is currently pressed.
    /// </summary>
    /// <param name="action">The <see cref="InputAction"/> that should be checked.</param>
    /// <returns>True if the <see cref="InputAction"/> is currently pressed.</returns>
    public bool IsPressed(InputAction action) {
        if (mConsumedActions.Contains(action)) {
            return false;
        }

        return mCurrentActions.Contains(action);
    }

    /// <summary>
    /// Checks if a given <see cref="InputAction"/> has just been pressed (hasn't been pressed the frame before).
    /// </summary>
    /// <param name="action">The <see cref="InputAction"/> that should be checked.</param>
    /// <returns>True if the <see cref="InputAction"/> has just been pressed.</returns>
    public bool JustPressed(InputAction action) {
        if (mConsumedActions.Contains(action)) {
            return false;
        }

        return IsPressed(action) && !mPreviousActions.Contains(action);
    }

    /// <summary>
    /// Checks if a given <see cref="InputAction"/> has just been released (has been pressed the frame before but is no longer pressed now).
    /// </summary>
    /// <param name="action">The <see cref="InputAction"/> that should be checked.</param>
    /// <returns>True if the <see cref="InputAction"/> has just been released.</returns>
    public bool JustReleased(InputAction action) {
        if (mConsumedActions.Contains(action)) {
            return false;
        }

        return !IsPressed(action) && mPreviousActions.Contains(action);
    }

    /// <summary>
    /// Checks if a given <see cref="InputAction"/> has been held for a given minimum amount of time.
    /// </summary>
    /// <param name="action">The <see cref="InputAction"/> that should be checked.</param>
    /// <param name="minHoldTime">The minimum amount of time the <see cref="InputAction"/> should be held for already.</param>
    /// <returns>True if the <see cref="InputAction"/> has been held for at least the given amount of time.</returns>
    public bool IsHeld(InputAction action, float minHoldTime = 0.5f) {
        if (mConsumedActions.Contains(action)) {
            return false;
        }

        return IsPressed(action) && mHoldTimes[action] >= minHoldTime;
    }

    /// <summary>
    /// Checks if a given <see cref="InputAction"/> has been double pressed.
    /// </summary>
    /// <param name="action">The <see cref="InputAction"/> that should be checked.</param>
    /// <param name="maxTimeBetweenPresses">The maximum amount of time between the two <see cref="InputAction"/> trigger events.</param>
    /// <returns>True if the <see cref="InputAction"/> has been double pressed within the given amount of time.</returns>
    public bool IsDoublePressed(InputAction action, float maxTimeBetweenPresses = 0.5f) {
        if (mConsumedActions.Contains(action)) {
            return false;
        }

        if (!JustPressed(action)) {
            return false;
        }

        var lastPressed = mLastActionTimes[action];
        var difference = mCurrentTime - lastPressed;

        return difference <= maxTimeBetweenPresses;
    }

    /// <summary>
    /// Consumes the given <see cref="InputAction"/>. Consumed <see cref="InputAction"/> will no longer register as a currently active <see cref="InputAction"/>.
    /// </summary>
    /// <param name="action">The <see cref="InputAction"/> to consume.</param>
    public void Consume(InputAction action) {
        mConsumedActions.Add(action);
    }

    /// <summary>
    /// Consumes all currently active <see cref="InputAction"/>s.
    /// </summary>
    public void ConsumeAll() {
        foreach (var action in mCurrentActions) {
            Consume(action);
        }
    }

    public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState) {
        // remember the gameTime (useful for double click query method)
        mCurrentTime = gameTime.CurTime();
        mDeltaTime = gameTime.DeltaTime();

        // the current actions from the previous frame are now the previous actions
        mPreviousActions = new HashSet<InputAction>(mCurrentActions);

        // clear the current actions
        mCurrentActions.Clear();

        // clear the consumed actions
        mConsumedActions.Clear();

        GetKeyboardInput(keyboardState);
        GetMouseInput(mouseState);

        // get action hold times
        foreach (var action in mHoldTimes.Keys) {
            mHoldTimes[action] = IsPressed(action) ? mHoldTimes[action] + mDeltaTime : 0;
        }
    }

    private void GetKeyboardInput(KeyboardState keyboardState) {
        TextInput = string.Empty;

        var pressedKeys = keyboardState.GetPressedKeys();
        if (pressedKeys.Length is 0) {
            mLastCharInput = Keys.None;
            return;
        }

        // get the current actions from the current frame
        foreach (var key in pressedKeys) {
            if (!mKeyMapping.ContainsKey(key)) {
                continue;
            }

            var action = mKeyMapping[key];

            // add the action to the current actions
            mCurrentActions.Add(action);

            // remember when the action was last pressed
            mLastActionTimes[action] = mCurrentTime;
        }

        // get the raw text input
        var firstKey = pressedKeys[0];
        if (mLastCharInput == firstKey) {
            return;
        }

        mLastCharInput = firstKey;

        if (firstKey is Keys.Back) {
            TextInput += "\b";
        } else if (firstKey is Keys.Enter) {
            TextInput += "\r";
        } else {
            var character = firstKey.ToChar(GetCurrentModifier(keyboardState));
            if (character is not null) {
                TextInput += character;
            }
        }
    }

    private ModifierKey GetCurrentModifier(KeyboardState keyboardState) {
        var result = ModifierKey.None;
        result |= keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) ? ModifierKey.Shift : ModifierKey.None;
        result |= keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl) ? ModifierKey.Ctrl : ModifierKey.None;
        result |= keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt) ? ModifierKey.Alt : ModifierKey.None;
        result |= keyboardState.IsKeyDown(Keys.LeftWindows) || keyboardState.IsKeyDown(Keys.RightWindows) ? ModifierKey.Windows : ModifierKey.None;
        return result;
    }

    private void GetMouseInput(MouseState mouseState) {
        // update the cursor position
        GlobalCursorPosition = mouseState.Position.ToVector2();

        // update the cursor scroll values
        var mouseScroll = mouseState.ScrollWheelValue;

        CursorScrollValueDelta = mouseScroll != CursorScrollValue
            ? mouseScroll - CursorScrollValue
            : 0;
        CursorScrollValue = mouseScroll;

        if (mouseState.LeftButton == ButtonState.Pressed) {
            var action = mMouseMapping[MouseButtons.Left];
            mCurrentActions.Add(action);
            mLastActionTimes[action] = mCurrentTime;
        }

        if (mouseState.RightButton == ButtonState.Pressed) {
            var action = mMouseMapping[MouseButtons.Right];
            mCurrentActions.Add(action);
            mLastActionTimes[action] = mCurrentTime;
        }

        if (mouseState.MiddleButton == ButtonState.Pressed) {
            var action = mMouseMapping[MouseButtons.Middle];
            mCurrentActions.Add(action);
            mLastActionTimes[action] = mCurrentTime;
        }
    }

    private static (Dictionary<InputAction, double>, Dictionary<InputAction, double>) InitializeActionTimes() {
        var lastActionTime = new Dictionary<InputAction, double>();
        var holdTime = new Dictionary<InputAction, double>();

        foreach (var action in Enum.GetValues(typeof(InputAction))) {
            lastActionTime.Add((InputAction)action, double.MinValue);
            holdTime.Add((InputAction)action, 0d);
        }

        return (lastActionTime, holdTime);
    }

    private (Dictionary<Keys, InputAction>, Dictionary<MouseButtons, InputAction>) LoadInputMapping() {
        // TODO: read from file!
        return LoadDefaultMapping();
    }

    private (Dictionary<Keys, InputAction>, Dictionary<MouseButtons, InputAction>) LoadDefaultMapping() {
        var keyMapping = new Dictionary<Keys, InputAction> {
            { Keys.W, InputAction.Up },
            { Keys.S, InputAction.Down },
            { Keys.A, InputAction.Left },
            { Keys.D, InputAction.Right },
            { Keys.Up, InputAction.Up },
            { Keys.Down, InputAction.Down },
            { Keys.Left, InputAction.Left },
            { Keys.Right, InputAction.Right },
            { Keys.Enter, InputAction.Enter },
            { Keys.Escape, InputAction.Quit },
            { Keys.P, InputAction.Pause },
        };

        var mouseMapping = new Dictionary<MouseButtons, InputAction> {
            { MouseButtons.Left, InputAction.LeftClick },
            { MouseButtons.Right, InputAction.RightClick },
            { MouseButtons.Middle, InputAction.MiddleClick },
        };

        return (keyMapping, mouseMapping);
    }

    private void SaveMapping() {
        throw new NotImplementedException();
    }
}