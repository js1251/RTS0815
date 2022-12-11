using System;
using GameEngine.util;
using Microsoft.Xna.Framework;

namespace GameEngine.Debugging;

internal class StringValueParser<T> {
    internal string Input { get; set; }
    internal T Value { get; set; }

    internal StringValueParser(T defaultValue) {
        Value = defaultValue;
    }

    internal void Parse() {
        var parseType = typeof(T);

        if (parseType == typeof(string)) {
            var defaultValue = (string)Convert.ChangeType(Value, typeof(string));
            Value = (T)Convert.ChangeType(ParseString(Input, defaultValue), typeof(T));
            return;
        }

        if (parseType == typeof(int)) {
            var defaultValue = (int)Convert.ChangeType(Value, typeof(int));
            Value = (T)Convert.ChangeType(ParseInt(Input, defaultValue), typeof(T));
            return;
        }

        if (parseType == typeof(float)) {
            var defaultValue = (float)Convert.ChangeType(Value, typeof(float));
            Value = (T)Convert.ChangeType(ParseFloat(Input, defaultValue), typeof(T));
            return;
        }

        if (parseType == typeof(double)) {
            var defaultValue = (double)Convert.ChangeType(Value, typeof(double));
            Value = (T)Convert.ChangeType(ParseDouble(Input, defaultValue), typeof(T));
            return;
        }

        if (parseType == typeof(bool)) {
            var defaultValue = (bool)Convert.ChangeType(Value, typeof(bool));
            Value = (T)Convert.ChangeType(ParseBool(Input, defaultValue), typeof(T));
            return;
        }

        if (parseType == typeof(Vector2)) {
            var defaultValue = (Vector2)Convert.ChangeType(Value, typeof(Vector2));
            Value = (T)Convert.ChangeType(ParseVector2(Input, defaultValue), typeof(T));
            return;
        }

        if (parseType == typeof(Vector3)) {
            var defaultValue = (Vector3)Convert.ChangeType(Value, typeof(Vector3));
            Value = (T)Convert.ChangeType(ParseVector3(Input, defaultValue), typeof(T));
            return;
        }

        if (parseType == typeof(Vector4)) {
            var defaultValue = (Vector4)Convert.ChangeType(Value, typeof(Vector4));
            Value = (T)Convert.ChangeType(ParseVector4(Input, defaultValue), typeof(T));
            return;
        }
    }

    private static string ParseString(string input, string defaultOutput) {
        return input;
    }

    private static int ParseInt(string input, int defaultOutput) {
        return int.TryParse(input, out var value)
            ? value
            : defaultOutput;
    }

    private static float ParseFloat(string input, float defaultOutput) {
        return float.TryParse(input, out var value)
            ? value
            : defaultOutput;
    }

    private static double ParseDouble(string input, double defaultOutput) {
        return double.TryParse(input, out var value)
            ? value
            : defaultOutput;
    }

    private static bool ParseBool(string input, bool defaultOutput) {
        return bool.TryParse(input, out var value)
            ? value
            : defaultOutput;
    }

    private static float[] ParseVector(string input, float[] defaultOutput) {
        var split = input.Trim().Split(';');
        var result = new float[split.Length];

        if (result.Length != defaultOutput.Length) {
            return defaultOutput;
        }

        for (var i = 0; i < split.Length; i++) {
            result[i] = ParseFloat(split[i].Trim(), defaultOutput[i]);
        }

        return result;
    }

    private static Vector2 ParseVector2(string input, Vector2 defaultOutput) {
        var dim = ParseVector(input, defaultOutput.ToArray());
        return dim.Length is 2
            ? new Vector2(dim[0], dim[1])
            : defaultOutput;
    }

    private static Vector3 ParseVector3(string input, Vector3 defaultOutput) {
        var dim = ParseVector(input, defaultOutput.ToArray());
        return dim.Length is 3
            ? new Vector3(dim[0], dim[1], dim[2])
            : defaultOutput;
    }

    private static Vector4 ParseVector4(string input, Vector4 defaultOutput) {
        var dim = ParseVector(input, defaultOutput.ToArray());
        return dim.Length is 4
            ? new Vector4(dim[0], dim[1], dim[2], dim[3])
            : defaultOutput;
    }
}