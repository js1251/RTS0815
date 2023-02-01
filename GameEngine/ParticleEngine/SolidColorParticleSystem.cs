using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.ParticleEngine;

public class SolidColorParticleSystem : IParticleSystem {
    public int MaxParticles { get; init; }
    public int ParticlesPerSecond { get; init; }
    public bool IsEmitting { get; set; }

    private readonly ParticleEffect _effect;
    private readonly List<Particle> _particles;
    private readonly Vector2 _origin;

    public SolidColorParticleSystem(ParticleEffect effect, Vector2 origin) {
        _effect = effect;
        _origin = origin;

        _particles = new List<Particle>();

        // only allows one particle by default
        // only spawn one particle per second by default
        // can be overridden by setting MaxParticles & ParticlesPerSecond
        MaxParticles = 1;
        ParticlesPerSecond = 1;
    }

    public void EmitOnce(int amount) {
        for (var i = 0; i < amount; i++) {
            var particle = new Particle(_origin);
            _effect.ApplyInitializers(particle);
            _particles.Add(particle);
        }
    }

    public void Update(GameTime gameTime) {
        // spawn new particles if emitting
        if (IsEmitting) {
            var amount = (int)Math.Ceiling(ParticlesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);
            EmitOnce(amount);
        }

        // iterate over list of particles backwards to allow removal during iteration
        for (var i = _particles.Count - 1; i >= 0; i--) {
            var particle = _particles[i];

            // remove particle if it's dead
            if (particle.RemainingLife <= TimeSpan.Zero) {
                _particles.RemoveAt(i);
                continue;
            }

            // updating is done after life check to allow for the last update in the particle's lifetime
            particle.Update(gameTime);

            // apply effect modifiers
            _effect.ApplyModifiers(particle);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        foreach (var particle in _particles) {
            particle.Draw(spriteBatch);
        }
    }
}