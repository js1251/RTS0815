using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities.Components.Collision;

internal interface ICollider {
    internal void Update(GameTime gameTime);
    internal void Draw(SpriteBatch spriteBatch);
}