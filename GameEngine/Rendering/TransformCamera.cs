using System;
using GameEngine.Input;
using Microsoft.Xna.Framework;
using GameEngine.util;

namespace GameEngine.Rendering;

public class TransformCamera : Camera {
    /// <summary>
    /// How many units per second the camera moves over the screen.
    /// </summary>
    public float MoveSpeedPerSecond { get; init; } = 100f;

    /// <summary>
    /// A zoom per mouse wheel increment factor. Needs to be quite low. Defaults to 0.001f.
    /// </summary>
    public float ZoomDeltaFactor { get; init; } = 0.001f;

    /// <summary>
    /// The minimum zoom level. Default is 0.5f.
    /// </summary>
    public float MinZoom { get; init; } = 0.5f;

    /// <summary>
    /// The maximum zoom level. Defaults to 2x.
    /// </summary>
    public float MaxZoom { get; init; } = 2f;

    /// <summary>
    /// The rectangle that defines the camera's view.
    /// </summary>
    public Rectangle Viewport { get; set; }

    private Vector2 mCurrentPosition;
    private Vector2 mMoveAmount;
    private float mCurrentZoom;
    private float mZoomAmount;

    public override void Update(GameTime gameTime, InputManager inputManager) {
        base.Update(gameTime, inputManager);

        Transform.Decompose(out var scale, out _, out var translation);
        mCurrentZoom = scale.X;
        mCurrentPosition = new Vector2(translation.X, translation.Y);

        UpdateTranslate(gameTime, inputManager);
        UpdateZoom(inputManager);

        var updated = false;

        if (mZoomAmount != 0) {
            updated = true;
            var cursorOnScreen = LocalToGlobal(inputManager.GlobalCursorPosition);
            Transform = Matrix.CreateTranslation(new Vector3(-cursorOnScreen, 0f)) *
                        Matrix.CreateScale(1f + mZoomAmount) *
                        Matrix.CreateTranslation(new Vector3(cursorOnScreen, 0f)) *
                        Transform;
        }

        if (mMoveAmount != Vector2.Zero) {
            updated = true;
            Transform = Matrix.CreateTranslation(new Vector3(mMoveAmount, 0f)) *
                        Transform;
        }

        if (!updated) {
            return;
        }

        UpdateViewPort();

        mMoveAmount = Vector2.Zero;
        mZoomAmount = 0f;
    }

    public bool IsInView(Vector2 position) {
        return Viewport.Contains(position);
    }

    public bool IsInView(Rectangle rectangle) {
        return Viewport.Intersects(rectangle);
    }

    public void Move(Vector2 amount) {
        mMoveAmount += amount;
    }

    public void MoveTo(Vector2 position) {
        mMoveAmount += position - mCurrentPosition;
    }

    public void Zoom(float amount) {
        mZoomAmount += amount;
    }

    public void ZoomTo(float factor) {
        mZoomAmount = factor - mCurrentZoom;
    }

    private void UpdateViewPort() {
        var (x, y) = GlobalToLocal(Vector2.Zero);
        var (f, f1) = GlobalToLocal(new Vector2(Application.ScrW, Application.ScrH));
        Viewport = new Rectangle((int)x, (int)y, (int)(f - x), (int)(f1 - y));
    }

    private void UpdateTranslate(GameTime gameTime, InputManager inputManager) {
        var moveAmount = MoveSpeedPerSecond * gameTime.DeltaTime();
        var translation = Vector2.Zero;

        if (inputManager.IsPressed(InputAction.Up)) {
            inputManager.Consume(InputAction.Up);

            translation.Y += moveAmount;
        }

        if (inputManager.IsPressed(InputAction.Down)) {
            inputManager.Consume(InputAction.Down);

            translation.Y -= moveAmount;
        }

        if (inputManager.IsPressed(InputAction.Left)) {
            inputManager.Consume(InputAction.Left);

            translation.X += moveAmount;
        }

        if (inputManager.IsPressed(InputAction.Right)) {
            inputManager.Consume(InputAction.Right);

            translation.X -= moveAmount;
        }

        Move(translation);
    }

    private void UpdateZoom(InputManager inputManager) {
        var zoomDelta = inputManager.CursorScrollValueDelta;

        if (zoomDelta == 0f) {
            return;
        }

        var zoom = zoomDelta * ZoomDeltaFactor;

        if (zoom > 0f) {
            if (mCurrentZoom > MaxZoom) {
                return;
            }

            zoom = Math.Min(MaxZoom - mCurrentZoom, zoom);
        } else {
            if (mCurrentZoom < MinZoom) {
                return;
            }

            zoom = Math.Max(MinZoom - mCurrentZoom, zoom);
        }

        Zoom(zoom);
    }
}