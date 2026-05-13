using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using factoryRL.Inputs;
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
    private RenderTarget2D _gameRenderTarget;
    public (int width, int height) VirtualResolution => (300, 150);
    public (int width, int height) ViewportResolution => (1800, 900);
    public float scale;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _inputManager = new InputManager(this);

        _graphics.PreferredBackBufferWidth = ViewportResolution.width;
        _graphics.PreferredBackBufferHeight = ViewportResolution.height;
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
            CoalTerrain = Content.Load<Texture2D>("coalTerrain"),
            StoneTerrain = Content.Load<Texture2D>("stoneTerrain"),
            CopperTerrain = Content.Load<Texture2D>("copperTerrain"),
            Forest = Content.Load<Texture2D>("forestTerrain"),
            Player = Content.Load<Texture2D>("player"),
            Worker = Content.Load<Texture2D>("worker"),
            IronOre = Content.Load<Texture2D>("ironTerrain"),
            MovmentIndicicator = Content.Load<Texture2D>("MovementIndicator"),
            backgroundTextureTile = Content.Load<Texture2D>("grassTile"),
            hoveredTileIndicator = Content.Load<Texture2D>("tileFrame"),
            ProgressWheel = Content.Load<Texture2D>("progressWheel"),
            Mine = Content.Load<Texture2D>("mineStation"),
            LumberMill = Content.Load<Texture2D>("sawmill"),
            MenuBackgroundNS = Content.Load<Texture2D>("v2-menuTextureNS"),
        };

        _gameRenderTarget = new RenderTarget2D(
            GraphicsDevice,
            VirtualResolution.width,
            VirtualResolution.height
        );

        ResourceDatabase.Initialize(_assets);
        TerrainDatabase.Initialize(_assets);

        currentScene = new World(this, _assets);
    }

    protected override void Update(GameTime gameTime)
    {
        scale = GraphicsDevice.Viewport.Width / VirtualResolution.width;

        _inputManager.Update();
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        currentScene.Update(gameTime);

        base.Update(gameTime);
        _inputManager.EndUpdate();
    }

    protected override void Draw(GameTime gameTime)
    {
        // GraphicsDevice.Clear(Color.Black); // Clears previous frame

        // _spriteBatch.Begin(SpriteSortMode.Immediate);
        // currentScene.Draw(_spriteBatch);
        // _spriteBatch.End();

        // base.Draw(gameTime);



        GraphicsDevice.SetRenderTarget(_gameRenderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(
            samplerState: SamplerState.PointClamp
        );

        // Draw your game normally here
        currentScene.Draw(_spriteBatch);

        _spriteBatch.End();

        // Switch back to screen
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        // Draw scaled render target to window
        _spriteBatch.Begin(
            samplerState: SamplerState.PointClamp
        );

        _spriteBatch.Draw(
            _gameRenderTarget,
            destinationRectangle: new Rectangle(
                0,
                0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height
            ),
            Color.White
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
