using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities.Components.Collision;

internal class BoxCollider : ICollider {
    void ICollider.Update(GameTime gameTime) {
        throw new System.NotImplementedException();
    }

    void ICollider.Draw(SpriteBatch spriteBatch) {
        throw new System.NotImplementedException();
    }
}