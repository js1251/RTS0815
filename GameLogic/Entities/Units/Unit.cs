using GameLogic.Entities.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities.Units;

internal class Unit : Entity {
    internal IDrawType DrawType { get; set; }

    internal override void Update(GameTime gameTime) {
        DrawType.Update(gameTime);
    }

    internal override void Draw(SpriteBatch spriteBatch) {
        DrawType.Draw(spriteBatch);
    }
}