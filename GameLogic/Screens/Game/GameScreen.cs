using System.Collections.Generic;
using GameEngine.Assets;
using GameEngine.Input;
using GameEngine.ParticleEngine;
using GameEngine.Rendering;
using GameEngine.Screens;
using GameLogic.Entities;
using GameLogic.ParticleEffects;
using GameLogic.Screens.Pause;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Screens.Game;

internal class GameScreen : Screen {
    private readonly List<Arrow> mArrows = new();
    private readonly List<IParticleSystem> mParticleSystems = new();

    internal GameScreen() {
        Camera = new TransformCamera {
            MinZoom = 0.01f,
            MoveSpeedPerSecond = 1000f,
        };
        DrawScreen = true;
        UpdateScreen = true;
        DrawLower = false;
        UpdateLower = false;

        mParticleSystems.Add(new SolidColorParticleSystem(new ExampleBloodEffect(), Vector2.Zero) {
            MaxParticles = 100,
            ParticlesPerSecond = 30,
        });
    }

    public override void Update(GameTime gameTime, InputManager inputManager) {
        if (inputManager.JustPressed(InputAction.Quit)) {
            ScreenStack.PopScreen();
            inputManager.Consume(InputAction.Quit);
            return;
        }

        if (inputManager.JustPressed(InputAction.Pause)) {
            ScreenStack.PushScreen(new PauseScreen());
            inputManager.Consume(InputAction.Pause);
            return;
        }

        ScreenContext.DebugValue<int>("int", 0);
        ScreenContext.DebugValue<double>("double", 0d);
        ScreenContext.DebugValue<Vector2>("vector2", Vector2.Zero);

        if (inputManager.JustPressed(InputAction.LeftClick)) {
            var arrow = new Arrow {
                WorldPosition = Vector2.Zero,
                Target = ScreenContext.LocalToGlobal(inputManager.GlobalCursorPosition)
            };

            mArrows.Add(arrow);
        }

        if (inputManager.JustPressed(InputAction.RightClick)) {
            foreach (var particleSystem in mParticleSystems) {
                particleSystem.IsEmitting = !particleSystem.IsEmitting;
            }
        }

        foreach (var arrow in mArrows) {
            arrow.Update(gameTime);
        }

        foreach (var particleSystem in mParticleSystems) {
            particleSystem.Update(gameTime);
        }
    }

    public override void Draw(SpriteBatch spriteBatch) {
        ScreenContext.DebugDraw(() => {
            // origin axis lines
            spriteBatch.DrawString(AssetStore.Fonts["Calibri"], "1m", new Vector2(100, 0), 48, Color.Red);
            spriteBatch.DrawString(AssetStore.Fonts["Calibri"], "1m", new Vector2(0, 100), 48, Color.Green);
            spriteBatch.DrawArrow(Vector2.Zero, Vector2.UnitX * 100, Color.Red, 5f);
            spriteBatch.DrawArrow(Vector2.Zero, Vector2.UnitY * 100, Color.Green, 5f);
        });

        foreach (var arrow in mArrows) {
            arrow.Draw(spriteBatch);
        }

        foreach (var particleSystem in mParticleSystems) {
            particleSystem.Draw(spriteBatch);
        }
    }
}