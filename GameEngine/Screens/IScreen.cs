using GameEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Screens;

public interface IScreen {
    public bool DrawScreen { get; }
    public bool UpdateScreen { get; }
    public bool DrawLower { get; }
    public bool UpdateLower { get; }
    public ScreenManager ScreenManager { get; set; }
    public void Update(GameTime gameTime, InputManager inputManager);
    public void Draw(SpriteBatch spriteBatch);
}