using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities.Components.Collision;

internal interface ICollidable {
    protected ICollider Collider { get; set; }
    internal void Update(GameTime gameTime);
    internal void Draw(SpriteBatch spriteBatch);
}