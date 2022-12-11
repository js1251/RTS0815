using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameEngine.Screens;

public static class ScreenContext {
    internal static Screen CurrentScreen { get; set; }
    internal static DebugScreen DebugScreen { get; set; }
    internal static bool DebugEnabled { get; set; }

    private static readonly List<Action> mDebugDrawActions = new();

    public static Vector2 GlobalToLocal(Vector2 global) {
        return CurrentScreen.Camera.GlobalToLocal(global);
    }

    public static Vector2 LocalToGlobal(Vector2 local) {
        return CurrentScreen.Camera.LocalToGlobal(local);
    }

    public static void ToggleDebug() {
        DebugEnabled = !DebugEnabled;
    }

    public static T DebugValue<T>(string name, T defaultValue) {
        return DebugScreen is not null ? DebugScreen.DebugValue(name, defaultValue) : defaultValue;
    }

    public static void DebugDraw(Action drawAction) {
        if (!DebugEnabled) {
            return;
        }

        mDebugDrawActions.Add(drawAction);
    }

    internal static void DrawDebug() {
        foreach (var drawAction in mDebugDrawActions) {
            drawAction.Invoke();
        }

        mDebugDrawActions.Clear();
    }
}