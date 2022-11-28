using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering;

public static class DrawExtension {
    #region fields

    private static Texture2D sTexture;

    #endregion fields

    #region Api Extensions

    public static void DrawOutlineRectangle(this SpriteBatch spriteBatch, Vector2 pos, Vector2 size, Color color, float thickness = 1f, float alpha = 1f) {
        var (x, y) = size;

        var polygons = new[] {
            new Vector2(0, 0) + pos,
            new Vector2(x, 0) + pos,
            new Vector2(x, y) + pos,
            new Vector2(0, y) + pos
        };

        spriteBatch.DrawOutlinePolygon(polygons, color, thickness, alpha);
    }

    public static void DrawOutlineRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, float thickness = 1f, float alpha = 1f) {
        DrawOutlineRectangle(spriteBatch, rectangle.Location.ToVector2(), rectangle.Size.ToVector2(), color, thickness, alpha);
    }

    public static void DrawFilledSquare(this SpriteBatch spriteBatch, Vector2 pos, float size, Color color, float alpha = 1f, float rotation = 0f) {
        spriteBatch.DrawFilledRectangle(pos, size, size, color, alpha, rotation);
    }

    public static void DrawFilledRectangle(this SpriteBatch spriteBatch, Vector2 pos, float width, float height, Color color, float alpha = 1f, float rotation = 0f) {
        spriteBatch.Draw(GetTexture(spriteBatch), pos, null, color * alpha, rotation, Vector2.One / 2, new Vector2(width, height), SpriteEffects.None, 0);
    }

    public static void DrawFilledRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, float alpha = 1f, float rotation = 0f) {
        var scale = new Vector2 {
            X = rectangle.Width,
            Y = rectangle.Height,
        };

        var pos = rectangle.Location.ToVector2() + scale / 2;
        spriteBatch.Draw(GetTexture(spriteBatch), pos, null, color * alpha, rotation, Vector2.One / 2, scale, SpriteEffects.None, 0);
    }

    public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f, float alpha = 1f) {
        var distance = Vector2.Distance(point1, point2);
        var rotation = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        DrawLine(spriteBatch, point1, distance, rotation, color, thickness, alpha);
    }

    public static void DrawArrow(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f, float alpha = 1f) {
        var distance = (point2 - point1).Length();
        var dir = (point2 - point1) / distance;
        var rotation = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        DrawLine(spriteBatch, point1, distance, rotation, color, alpha, thickness);

        var backOffset = -dir * thickness * 10;
        var perpendicular = new Vector2(-dir.Y, dir.X);
        var sideOffset = perpendicular * thickness * 2.5f;

        DrawLine(spriteBatch, point2, point2 + backOffset - sideOffset, color, thickness, alpha);
        DrawLine(spriteBatch, point2, point2 + backOffset + sideOffset, color, thickness, alpha);
    }

    public static void DrawCross(this SpriteBatch spriteBatch, Vector2 position, float length, Color color, float thickness = 1f, float alpha = 1f, float rotation = 0f) {
        var halfLength = length / 2f;

        var rotationMatrix = Matrix.CreateRotationZ(rotation);

        var x = Vector2.Transform(Vector2.UnitX, rotationMatrix);
        var y = Vector2.Transform(Vector2.UnitY, rotationMatrix);

        var p1 = position + x * halfLength;
        var p2 = position - x * halfLength;
        var p3 = position + y * halfLength;
        var p4 = position - y * halfLength;

        spriteBatch.DrawLine(p1, p2, color, thickness, alpha);
        spriteBatch.DrawLine(p3, p4, color, thickness, alpha);
    }

    public static void DrawOutlinePolygon(this SpriteBatch spriteBatch, Vector2[] vertex, Color color, float thickness = 1f, float alpha = 1f) {
        if (vertex.Length <= 0) {
            return;
        }

        for (var i = 0; i < vertex.Length - 1; i++) {
            DrawLine(spriteBatch, vertex[i], vertex[i + 1], color, thickness, alpha);
        }

        DrawLine(spriteBatch, vertex[^1], vertex[0], color, thickness, alpha);
    }

    public static void DrawOutlineCircle(this SpriteBatch spriteBatch, Vector2 center, float radius, Color color, int segments = 16, float thickness = 1f, float alpha = 1f) {
        var vertex = new Vector2[segments];

        var increment = Math.PI * 2.0 / segments;
        var theta = 0.0;

        for (var i = 0; i < segments; i++) {
            vertex[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            theta += increment;
        }

        DrawOutlinePolygon(spriteBatch, vertex, color, thickness, alpha);
    }

    #endregion Api Extensions

    #region Helpers

    private static Texture2D GetTexture(this SpriteBatch spriteBatch) {
        if (sTexture != null) {
            return sTexture;
        }

        sTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        sTexture.SetData(new[] { Color.White });

        return sTexture;
    }

    private static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float rotation, Color color, float thickness = 1f, float alpha = 1f) {
        var origin = new Vector2(0f, 0.5f);
        var scale = new Vector2(length, thickness);
        spriteBatch.Draw(GetTexture(spriteBatch), point, null, color * alpha, rotation, origin, scale, SpriteEffects.None, 0);
    }

    #endregion helpers
}