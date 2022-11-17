﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities;

internal abstract class Entity {
    internal abstract void Update(GameTime gameTime);
    internal abstract void Draw(SpriteBatch spriteBatch);
}