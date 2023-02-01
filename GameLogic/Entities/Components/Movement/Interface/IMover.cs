using Microsoft.Xna.Framework;

namespace GameLogic.Entities.Components.Movement.Interface;

internal interface IMover<T> where T : IMovable {
    public void Update(GameTime gameTime);
}