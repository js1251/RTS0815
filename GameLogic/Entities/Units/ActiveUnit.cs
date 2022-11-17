using GameLogic.Entities.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities.Units;

internal class ActiveUnit : Unit {
    internal ICollideType CollideType { get; set; }
    internal IMoveType MoveType { get; set; }

    internal override void Update(GameTime gameTime) {
        base.Update(gameTime);

        MoveType.Update(gameTime);
        CollideType.Update(gameTime);
    }

    internal override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        MoveType.Draw(spriteBatch);
        CollideType.Draw(spriteBatch);
    }
}