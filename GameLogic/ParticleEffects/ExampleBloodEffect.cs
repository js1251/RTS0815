using System;
using GameEngine.ParticleEngine;
using Microsoft.Xna.Framework;

namespace GameLogic.ParticleEffects {
    internal class ExampleBloodEffect : ParticleEffect {
        public override void ApplyInitializers(Particle particle) {
            InitLifetimeRandom(particle, TimeSpan.FromSeconds(0.5f), TimeSpan.FromSeconds(1f));
            InitOffsetRandom(particle, new Vector2(-10, -10), new Vector2(10, 10));
            InitSizeRandom(particle, 3f, 10f);
            InitVelocityRandom(particle, new Vector2(-200, -200), new Vector2(200, 200));
            InitColorRandom(particle, new Color(110, 2, 2), new Color(230, 44, 44));
            InitRotationRandom(particle, 0, 360);
        }

        public override void ApplyModifiers(Particle particle) {
            //10m/s^2 (assuming 1 unit is 1cm)
            var gravity = Vector2.UnitY * 1000f; // TODO: add to global constant
            ModifyAcceleration(particle, gravity);

            MapLifetimeToAlpha(particle, 1f, 0f);
        }
    }
}