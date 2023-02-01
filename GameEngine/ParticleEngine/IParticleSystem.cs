using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.ParticleEngine;

public interface IParticleSystem {
    public int MaxParticles { get; init; }
    public int ParticlesPerSecond { get; init; }
    public bool IsEmitting { get; set; }

    /// <summary>
    /// Emits a given amount of particles within the current update cycle
    /// </summary>
    /// <param name="amount">The amount of particles to spawn immediately</param>
    public void EmitOnce(int amount);

    public void Update(GameTime gameTime);
    public void Draw(SpriteBatch spriteBatch);
}