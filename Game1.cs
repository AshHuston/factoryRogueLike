using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using factoryRL.Inputs;
using System.Reflection;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using factoryRL.GameObjects;
namespace factoryRL;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public InputManager _inputManager;
    private GameAssets _assets;
    public Scene currentScene;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _inputManager = new InputManager();

        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _assets = new GameAssets
        {
            IronTerrain = Content.Load<Texture2D>("ironTerrain"),
            Player = Content.Load<Texture2D>("player")
        };

        ResourceDatabase.Initialize(_assets);
        TerrainDatabase.Initialize(_assets);

        currentScene = new World(this, _assets);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        currentScene.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue); // Clears previous frame

        _spriteBatch.Begin();
        currentScene.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
