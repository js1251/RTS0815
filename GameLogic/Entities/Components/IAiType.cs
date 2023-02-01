using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities.Interfaces;

internal interface IAiType {
    internal void Update(GameTime gameTime);
    internal void Draw(SpriteBatch spriteBatch);
}