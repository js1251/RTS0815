namespace GameEngine.Screens;

public interface IScreen {
    public bool DrawScreen { get; set; }
    public bool UpdateScreen { get; set; }
    public bool DrawLower { get; set; }
    public bool UpdateLower { get; set; }
    public void Update(GameTime gameTime);
    public void Draw(SpriteBatch spriteBatch);
}