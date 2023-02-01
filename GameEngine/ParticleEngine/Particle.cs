using System;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.ParticleEngine;

public class Particle {
    private TimeSpan _initialLife;

    internal TimeSpan InitialLife {
        get => _initialLife;
        set {
            _initialLife = value;
            RemainingLife = value;
        }
    }

    internal TimeSpan RemainingLife { get; private set; }

    internal float Size { get; set; } = 1f;

    internal Vector2 Position { get; set; }
    internal Vector2 Velocity { get; set; }
    internal Vector2 Acceleration { get; set; }

    internal float Rotation { get; set; }
    internal float AngularVelocity { get; set; }

    internal Color Color { get; set; }
    internal float Alpha { get; set; } = 1f;

    internal Particle(Vector2 origin) {
        Position = origin;
    }

    internal void Update(GameTime gameTime) {
        // decrease remaining life
        RemainingLife -= gameTime.ElapsedGameTime;

        var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // default euler integration
        Velocity += Acceleration * deltaT;
        Position += Velocity * deltaT;

        // reset acceleration for next iteration
        Acceleration = Vector2.Zero;

        // same euler integration for rotation
        Rotation += AngularVelocity * deltaT;
    }

    internal void Draw(SpriteBatch spriteBatch) {
        spriteBatch.DrawFilledRectangle(Position, Size, Size, Color, Alpha, Rotation);
    }
}