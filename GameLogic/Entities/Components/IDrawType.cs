using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities.Interfaces;

internal interface IDrawType {
    internal void Update(GameTime gameTime);
    internal void Draw(SpriteBatch spriteBatch);
}