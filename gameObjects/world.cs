using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace factoryRL.GameObjects;

public class World : Scene
{
    internal Entity[,] map;
    internal Vector2 mapCenter;
    public Vector2 camCenter;
    internal Game1 game;
    internal Player player;
    private Texture2D hoverIndicatorTexture;
    internal Vector2 mouseWorldMapPosition = new Vector2(0, 0);
    private BuildMenu buildMenu;
    private bool isDisplayingBuildMenu = false;

    private SpriteFont testFont;

    public World(Game1 _game, GameAssets assets) : base(_game, assets)
    {
        game=_game;
        tileSizePixels = (int)Math.Round(32*game.scale);
        int mapWidth = 500;
        int mapHeight = 500;
        map = new Entity[mapWidth, mapHeight];
        mapCenter = new(mapWidth / 2, mapHeight / 2);
        camCenter = new Vector2(map.GetLength(0) * tileSizePixels / 2, map.GetLength(1) * tileSizePixels / 2);
        GenerateMap();

        backgroundTexture = assets.backgroundTextureTile;
        hoverIndicatorTexture = assets.hoveredTileIndicator;

        player = new Player(game, this, assets, camCenter);
        Add(player);

        //Test station vvv
        HarvestableTerrain testTerrain = new(this, assets, TerrainDatabase.Data[ResourceType.Wood], new Vector2(10, 10));
        map[(int)mapCenter.X, (int)mapCenter.Y] = testTerrain;
        Add(testTerrain);
        Add(new LumberMill(this, assets, testTerrain));
        // ----------------------------------------------------------------------------------------------------------------

        testFont = assets.Pixel1Font;

        buildMenu = new BuildMenu(
            this,
            assets,
            new Rectangle(15, 15, 0, 0)
        );

        for (int i=0; i<20; i++)
        {
            Add(new Worker(game, this, assets, camCenter));
        }
        Worker worker = new Worker(game, this, assets, camCenter + new Vector2(30,30));
        Add(worker);
    }

    public void GenerateMap()
    {
        List<HarvestableTerrain> harvestableTerrains = new List<HarvestableTerrain>();
        float decayFactor = 0.25f;
        ResourceType[] resourceTypes = [
            ResourceType.Iron,
            ResourceType.Coal,
            ResourceType.Copper,
            ResourceType.Stone,
            ResourceType.Wood
        ];

        Random random = new Random();
        int maxRangeFromCenterTiles = 15;

        foreach (var resourceType in resourceTypes)
        {
            Vector2 seedTile = new Vector2(
                MathF.Floor(mapCenter.X) + random.Next(-maxRangeFromCenterTiles, maxRangeFromCenterTiles),
                MathF.Floor(mapCenter.Y) + random.Next(-maxRangeFromCenterTiles, maxRangeFromCenterTiles)
            );
            
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Vector2 targetTile = new Vector2(x, y);
                    float distanceFromSeed = Vector2.Distance(seedTile, targetTile);
                    float probability = MathF.Max(0, 1 - (distanceFromSeed * decayFactor));
                    if (random.NextDouble() < probability)
                    {
                        //Debug.WriteLine(targetTile);
                        HarvestableTerrain terrain = new(this, assets, TerrainDatabase.Data[resourceType], targetTile);
                        gameEntities.Add(terrain);
                        map[x, y] = terrain;
                    }
                }
            }
        }
        RemoveInvalidHarvestableTerrain();
    }

    public void AdjustEntityPositions()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                Entity entity = map[x, y];
                if (entity != null)
                {
                    entity._position = new Vector2(
                        (x * tileSizePixels) - camCenter.X + (game.GraphicsDevice.Viewport.Width / (game.scale*2)),
                        (y * tileSizePixels) - camCenter.Y + (game.GraphicsDevice.Viewport.Height / (game.scale*2))
                    );
                }
            }
        }

        foreach (var entity in gameEntities)
        {
            if (entity is Meeple m)
            {
                m._position = new Vector2(
                    m.worldPosition.X - camCenter.X + (game.GraphicsDevice.Viewport.Width / (game.scale*2)),
                    m.worldPosition.Y - camCenter.Y + (game.GraphicsDevice.Viewport.Height / (game.scale*2))
                );
            }
        }
    }

    private bool IsInvalidHarvestableTerrain(Entity entity)
    {
        if (entity is HarvestableTerrain terrain)
        {
            bool isOutOfMap = terrain._position.X < 0 || terrain._position.Y < 0 || terrain._position.X >= map.GetLength(0) || terrain._position.Y >= map.GetLength(1);

            return isOutOfMap || map[(int)terrain._position.X, (int)terrain._position.Y] != terrain;
        } else {
            return false;
        }
    }

    public void RemoveInvalidHarvestableTerrain()
    {
        gameEntities.RemoveAll(IsInvalidHarvestableTerrain);
    }

    internal Vector2 GetTileCoordinates(Vector2 worldPosition)
    {
        return new Vector2(
            MathF.Floor(worldPosition.X / tileSizePixels),
            MathF.Floor(worldPosition.Y / tileSizePixels)
        );
    }

    public override void DrawBackground(SpriteBatch spriteBatch)
    {
        int tilesX = (game.GraphicsDevice.Viewport.Width / tileSizePixels) + 2;
        int tilesY = (game.GraphicsDevice.Viewport.Height / tileSizePixels) + 2;
        Vector2 topLeftTile = GetTileCoordinates(camCenter) - new Vector2(tilesX, tilesY) / 2;

        for (int x = 0; x < tilesX; x++)
        {
            for (int y = 0; y < tilesY; y++)
            {
                Vector2 tilePos = topLeftTile + new Vector2(x, y);
                spriteBatch.Draw(
                    backgroundTexture,
                    new Vector2(tilePos.X * tileSizePixels - camCenter.X + (game.GraphicsDevice.Viewport.Width / 2), tilePos.Y * tileSizePixels - camCenter.Y + (game.GraphicsDevice.Viewport.Height / 2)),
                    Color.White
                );
            }
        }
    }

    public override void Update(GameTime gameTime) 
    {
        AdjustEntityPositions();
        player._position = new Vector2(
            player.worldPosition.X - camCenter.X + (game.ViewportResolution.width / (game.scale*2)),
            player.worldPosition.Y - camCenter.Y + (game.ViewportResolution.height / (game.scale*2))
        );

        mouseWorldMapPosition = new Vector2(
            _inputManager.MouseWorldPosition.X - (_inputManager.MouseWorldPosition.X % tileSizePixels) + tileSizePixels / 2,
            _inputManager.MouseWorldPosition.Y - (_inputManager.MouseWorldPosition.Y % tileSizePixels) + tileSizePixels / 2
        );

        //                                          Arbitrary key choice  v
        if (_inputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
        {
            if (isDisplayingBuildMenu)
            {
                buildMenu.close();
                isDisplayingBuildMenu = false;
            }
            else
            {
                buildMenu.open();
                isDisplayingBuildMenu = true;
            }
        }

        // TEMP This makes the character, not the mouse, move the screen. This is likely temporary.
        int edgeWidth = 65;
        if (player._position.X < edgeWidth){ camCenter.X -= player.mvSpdPx; }
        if (player._position.Y < edgeWidth){ camCenter.Y -= player.mvSpdPx; }
        if (player._position.X > game.VirtualResolution.width - edgeWidth - tileSizePixels){ camCenter.X += player.mvSpdPx; }
        if (player._position.Y > game.VirtualResolution.height - edgeWidth - tileSizePixels){ camCenter.Y += player.mvSpdPx; }
        
        // ------------------------------------------------------------------------------------
        
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (!hasHoveredMenu) {
            spriteBatch.Draw(
                hoverIndicatorTexture,
                mouseWorldMapPosition - camCenter + new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2),
                null,
                Color.White,
                0f,
                new Vector2(hoverIndicatorTexture.Width / 2, hoverIndicatorTexture.Height / 2),
                1f,
                SpriteEffects.None,
                0f
            );   
        }
    }
}
