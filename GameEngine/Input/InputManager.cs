using System;
using System.Collections.Generic;
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
    public Vector2 CursorPosition { get; private set; }
    public float CursorScrollValue { get; private set; }
    public float CursorScrollValueDelta { get; private set; }

    private Dictionary<Keys, InputAction> mKeyMapping;
    private Dictionary<MouseButtons, InputAction> mMouseMapping;

    private readonly HashSet<InputAction> mCurrentActions;
    private HashSet<InputAction> mPreviousActions;
    private HashSet<InputAction> mConsumedActions;
    private readonly Dictionary<InputAction, double> mLastActionTimes;
    private readonly Dictionary<InputAction, double> mHoldTimes;
    private double mCurrentTime;
    private double mDeltaTime;

    public InputManager() {
        mCurrentActions = new HashSet<InputAction>();
        mPreviousActions = new HashSet<InputAction>();
        mConsumedActions = new HashSet<InputAction>();

        (mKeyMapping, mMouseMapping) = LoadInputMapping();
        (mLastActionTimes, mHoldTimes) = InitializeActionTimes();
    }

    public void SetMapping(InputAction action, Keys input) {
        if (mKeyMapping.ContainsKey(input)) {
            mKeyMapping[input] = action;
            return;
        }

        mKeyMapping.Add(input, action);
    }

    public void SetMapping(InputAction action, MouseButtons input) {
        if (mMouseMapping.ContainsKey(input)) {
            mMouseMapping[input] = action;
            return;
        }

        mMouseMapping.Add(input, action);
    }

    public void ResetMapping() {
        (mKeyMapping, mMouseMapping) = LoadDefaultMapping();
    }

    public bool IsPressed(InputAction action) {
        if (mConsumedActions.Contains(action)) {
            return false;
        }

        return mCurrentActions.Contains(action);
    }

    public bool JustPressed(InputAction action) {
        if (mConsumedActions.Contains(action)) {
            return false;
        }

        return IsPressed(action) && !mPreviousActions.Contains(action);
    }

    public bool JustReleased(InputAction action) {
        if (mConsumedActions.Contains(action)) {
            return false;
        }

        return !IsPressed(action) && mPreviousActions.Contains(action);
    }

    public bool IsHeld(InputAction action, float minHoldTime = 0.5f) {
        if (mConsumedActions.Contains(action)) {
            return false;
        }

        return IsPressed(action) && mHoldTimes[action] >= minHoldTime;
    }

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

    public void Consume(InputAction action) {
        mConsumedActions.Add(action);
    }

    public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState) {
        // remember the gameTime (useful for double click query method)
        mCurrentTime = gameTime.TotalGameTime.TotalSeconds;
        mDeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

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
        // get the current actions from the current frame
        foreach (var key in keyboardState.GetPressedKeys()) {
            if (!mKeyMapping.ContainsKey(key)) {
                continue;
            }

            var action = mKeyMapping[key];

            // add the action to the current actions
            mCurrentActions.Add(action);

            // remember when the action was last pressed
            mLastActionTimes[action] = mCurrentTime;
        }
    }

    private void GetMouseInput(MouseState mouseState) {
        // update the cursor position
        CursorPosition = mouseState.Position.ToVector2();

        // update the cursor scroll values
        CursorScrollValueDelta = CursorScrollValue - mouseState.ScrollWheelValue;
        CursorScrollValue = mouseState.ScrollWheelValue;

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