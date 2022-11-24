using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities; 

internal class Camera : Entity {
    internal override Vector2 WorldPosition { get; set; }

    internal override void Update(GameTime gameTime) {
        throw new System.NotImplementedException();
    }

    internal override void Draw(SpriteBatch spriteBatch) {
        throw new System.NotImplementedException();
    }
}