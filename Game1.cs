using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using factoryRL.Inputs;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using factoryRL.GameObjects;
using System;

namespace factoryRL;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public InputManager _inputManager;
    private GameAssets _assets;
    public Scene currentScene;
    private RenderTarget2D _gameRenderTarget;
    public (int width, int height) VirtualResolution { get; set; } = (600, 400);
    public (int width, int height) ViewportResolution { get; set; } = (1200, 800);
    public float scale = 1f;

    private Texture2D pixel;

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

        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        _assets = new GameAssets
        {
            pixel = pixel,
            IronTerrain = Content.Load<Texture2D>("ironTerrain"),
            CoalTerrain = Content.Load<Texture2D>("coalTerrain"),
            StoneTerrain = Content.Load<Texture2D>("stoneTerrain"),
            CopperTerrain = Content.Load<Texture2D>("copperTerrain"),
            IronOre = Content.Load<Texture2D>("ironOre"),
            CoalOre = Content.Load<Texture2D>("coalOre"),
            StoneOre = Content.Load<Texture2D>("stoneOre"),
            CopperOre = Content.Load<Texture2D>("copperOre"),
            Forest = Content.Load<Texture2D>("forestTerrain"),
            Player = Content.Load<Texture2D>("player"),
            Worker = Content.Load<Texture2D>("worker"),
            MovmentIndicicator = Content.Load<Texture2D>("MovementIndicator"),
            backgroundTextureTile = Content.Load<Texture2D>("grassTile"),
            hoveredTileIndicator = Content.Load<Texture2D>("tileFrame"),
            ProgressWheel = Content.Load<Texture2D>("progressWheel"),
            Mine = Content.Load<Texture2D>("mineStation"),
            LumberMill = Content.Load<Texture2D>("sawmill"),
            TimberYard = Content.Load<Texture2D>("timberyard"),
            MenuBackgroundNS = Content.Load<Texture2D>("v2-menuTextureNS"),
            Pixel1Font = Content.Load<SpriteFont>("fonts/pixel1"),
            Courier = Content.Load<Texture2D>("courier"),
            Warehouse = Content.Load<Texture2D>("warehouse"),
            workerIcon = Content.Load<Texture2D>("worker-mini"),
            goldCoin = Content.Load<Texture2D>("goldCoin"),
        };

        ResourceDatabase.Initialize(_assets);
        HarvestableTerrainTileDatabase.Initialize(_assets);

        currentScene = new World(this, _assets);
    }

    public void DrawLine(
        SpriteBatch spriteBatch,
        Vector2 start,
        Vector2 end,
        Color color,
        float thickness = 1f)
    {
        Vector2 edge = end - start;

        float angle = MathF.Atan2(edge.Y, edge.X);

        spriteBatch.Draw(
            pixel,
            start,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(edge.Length(), thickness),
            SpriteEffects.None,
            0);
    }

    protected override void Update(GameTime gameTime)
    {
        // IMPROVE: Zoom in/out with mouse scroll. This current;y does not work. Well, it does, but the worldmap grid does not scale right unless you are at certain scales of the base VirtualResolution. This is likely because of how the world map grid is drawn, but I have not looked into it yet.
        // const float scrollSensitivity = 10f;
        // float aspectRatio = VirtualResolution.width / VirtualResolution.height;
        // if(_inputManager.IsMouseScrolledDown()) { 
        //     VirtualResolution = (
        //         (int)(VirtualResolution.width + (scrollSensitivity * aspectRatio)),
        //         (int)(VirtualResolution.height + scrollSensitivity)
        //     );
        //     Console.WriteLine($"Scrolled down. New virtual resolution: {VirtualResolution.width}x{VirtualResolution.height}");
        // }
        // if(_inputManager.IsMouseScrolledUp()) { 
        //     VirtualResolution = (
        //         (int)(VirtualResolution.width - (scrollSensitivity * aspectRatio)),
        //         (int)(VirtualResolution.height - scrollSensitivity)
        //     );
        //     Console.WriteLine($"Scrolled down. New virtual resolution: {VirtualResolution.width}x{VirtualResolution.height}");
        // }
        // 
        // if (_inputManager.IsKeyPressed(Keys.Q)){ Console.WriteLine("Q"); ViewportResolution = (300, 150); }
        // if (_inputManager.IsKeyPressed(Keys.W)){ ViewportResolution = (600, 300); }
        // if (_inputManager.IsKeyPressed(Keys.E)){ ViewportResolution = (900, 450); }
        // if (_inputManager.IsKeyPressed(Keys.R)){ ViewportResolution = (1800, 900); }
        // -------------------------------------------------------------------------------------------------------------------

        _graphics.PreferredBackBufferWidth = ViewportResolution.width;
        _graphics.PreferredBackBufferHeight = ViewportResolution.height;
        _graphics.ApplyChanges();

        scale = ViewportResolution.width / VirtualResolution.width;

        _inputManager.Update();
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        currentScene.Update(gameTime);

        base.Update(gameTime);
        _inputManager.EndUpdate();
    }

    protected override void Draw(GameTime gameTime)
    {
        _gameRenderTarget = new RenderTarget2D(
            GraphicsDevice,
            VirtualResolution.width,
            VirtualResolution.height
        );

        GraphicsDevice.SetRenderTarget(_gameRenderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw your game normally here
        currentScene.Draw(_spriteBatch);

        _spriteBatch.End();

        // Switch back to screen
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        // Draw scaled render target to window
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(
            _gameRenderTarget,
            destinationRectangle: new Rectangle(0, 0, ViewportResolution.width, ViewportResolution.height),
            Color.White
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
