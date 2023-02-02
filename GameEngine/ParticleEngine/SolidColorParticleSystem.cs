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
    private TimeSpan _timeSinceLastEmission;

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
            // stop emitting if max number of particles is hit
            if (_particles.Count > MaxParticles) {
                return;
            }

            var particle = new Particle(_origin);
            _effect.ApplyInitializers(particle);
            _particles.Add(particle);
        }
    }

    public void Update(GameTime gameTime) {
        SpawnParticlesIfRequired(gameTime);
        UpdateParticles(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch) {
        foreach (var particle in _particles) {
            particle.Draw(spriteBatch);
        }
    }

    private void SpawnParticlesIfRequired(GameTime gameTime) {
        // only spawn particles if emitting
        if (!IsEmitting) {
            // reset last emission to not spawn a lot of particles if emission is turned on again
            _timeSinceLastEmission = TimeSpan.Zero;
            return;
        }

        _timeSinceLastEmission += gameTime.ElapsedGameTime;

        // only spawn particles depending on ParticlesPerSecond
        if (_timeSinceLastEmission.TotalSeconds <= 1f / ParticlesPerSecond) {
            return;
        }

        // if the spawn-rate is lower than the tick-rate, multiple particles might need to be spawned
        var amount = (int)Math.Ceiling(ParticlesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);
        EmitOnce(amount);

        _timeSinceLastEmission = TimeSpan.Zero;
    }

    private void UpdateParticles(GameTime gameTime) {
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
}