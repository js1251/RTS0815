using System;
using GameEngine.util;
using Microsoft.Xna.Framework;

namespace GameEngine.ParticleEngine;

public abstract class ParticleEffect {
    /// <summary>
    /// Applies the initializers to the given particle.
    /// Should only ever include calls to methods making changes to the particle ONCE during initialization.
    /// </summary>
    /// <param name="particle">The particle to initialize.</param>
    public abstract void ApplyInitializers(Particle particle);

    /// <summary>
    /// Applies changes to the particle during its lifetime each update.
    /// Should only ever include calls to methods making constant changes to the particle each update.
    /// </summary>
    /// <param name="particle">The particle to modify.</param>
    public abstract void ApplyModifiers(Particle particle);

    protected void InitLifetimeRandom(Particle particle, TimeSpan min, TimeSpan max) {
        particle.InitialLife = TimeSpan.FromSeconds(RandomExtension.NextDouble(min.TotalSeconds, max.TotalSeconds));
    }

    protected void InitSizeRandom(Particle particle, float min, float max) {
        particle.Size = RandomExtension.NextFloat(min, max);
    }

    protected void InitOffsetRandom(Particle particle, Vector2 min, Vector2 max) {
        particle.Position += RandomExtension.NextVector2(min, max);
    }

    protected void InitVelocityRandom(Particle particle, Vector2 min, Vector2 max) {
        particle.Velocity = RandomExtension.NextVector2(min, max);
    }

    protected void InitAccelerationRandom(Particle particle, Vector2 min, Vector2 max) {
        particle.Acceleration = RandomExtension.NextVector2(min, max);
    }

    protected void InitRotationRandom(Particle particle, float min, float max) {
        particle.Rotation = RandomExtension.NextFloat(min, max);
    }

    protected void InitAngularVelocityRandom(Particle particle, float min, float max) {
        particle.AngularVelocity = RandomExtension.NextFloat(min, max);
    }

    protected void InitColorRandom(Particle particle, Color min, Color max) {
        particle.Color = RandomExtension.NextColor(min, max);
    }

    protected void InitAlphaRandom(Particle particle, float min, float max) {
        particle.Alpha = RandomExtension.NextFloat(min, max);
    }

    protected void ModifySize(Particle particle, float size) {
        particle.Size += size;
    }

    protected void ModifyVelocity(Particle particle, Vector2 velocity) {
        particle.Velocity += velocity;
    }

    protected void ModifyAcceleration(Particle particle, Vector2 acceleration) {
        particle.Acceleration += acceleration;
    }

    protected void ModifyAngularVelocity(Particle particle, float angularVelocity) {
        particle.AngularVelocity += angularVelocity;
    }

    protected void MapLifetimeToSize(Particle particle, float start, float end) {
        particle.Size = (float)particle.RemainingLife.TotalSeconds.Remap(0f, particle.InitialLife.TotalSeconds, start, end);
    }

    protected void MapLifetimeToVelocity(Particle particle, Vector2 start, Vector2 end) {
        var dir = Vector2.Normalize(particle.Velocity);
        var speed = (float)particle.RemainingLife.TotalSeconds.Remap(0f, particle.InitialLife.TotalSeconds, start.Length(), end.Length());
        particle.Velocity = dir * speed;
    }

    protected void MapLifetimeToColor(Particle particle, Color start, Color end) {
        particle.Color = particle.RemainingLife.TotalSeconds.Remap(0f, particle.InitialLife.TotalSeconds, start, end);
    }

    protected void MapLifetimeToAlpha(Particle particle, float start, float end) {
        particle.Alpha = (float)particle.RemainingLife.TotalSeconds.Remap(0f, particle.InitialLife.TotalSeconds, end, start);
    }

    protected void MapLifetimeToAngularVelocity(Particle particle, float start, float end) {
        particle.AngularVelocity = (float)particle.RemainingLife.TotalSeconds.Remap(0f, particle.InitialLife.TotalSeconds, start, end);
    }
}