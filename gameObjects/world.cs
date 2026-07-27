using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using factoryRL.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace factoryRL.GameObjects;

public class World : Scene
{
    internal Entity[,] map;
    internal Point mapCenter;
    public Vector2 camCenter;
    internal Game1 game;
    internal Player player;
    private Texture2D hoverIndicatorTexture;
    internal Vector2 mouseWorldMapPosition = new Vector2(0, 0);
    private BuildMenu buildMenu;
    private bool isDisplayingBuildMenu = false;
    private int playerGold = 0;

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
        Add(new CourierRouteAssigner(this, assets));

        backgroundTexture = assets.backgroundTextureTile;
        hoverIndicatorTexture = assets.hoveredTileIndicator;

        player = new Player(game, this, assets, camCenter);
        Add(player);

        //Test station vvv
        HarvestableTerrainTile testTerrain = new(this, assets, HarvestableTerrainTileDatabase.Data[ResourceType.Wood], new Vector2(mapCenter.X, mapCenter.Y));
        map[mapCenter.X, mapCenter.Y] = testTerrain;
        Add(testTerrain);
        Add(new TimberYard(this, assets, testTerrain));
        // ----------------------------------------------------------------------------------------------------------------

        testFont = assets.Pixel1Font;

        buildMenu = new BuildMenu(
            this,
            assets,
            new Rectangle(15, 15, 0, 0)
        );

        int TestWorkers = 2; // TESTING PURPOSES
        for (int i=0; i<TestWorkers; i++)
        {
            Add(new Worker(game, this, assets, camCenter));
        }
    }

    public void GenerateMap()
    {
        float decayFactor = 0.25f;
        ResourceType[] resourceTypes = [
            ResourceType.Iron,
            ResourceType.Coal,
            ResourceType.Copper,
            ResourceType.Stone,
            ResourceType.Wood
        ];

        Random random = new();
        int maxRangeFromCenterTiles = 15;

        foreach (var resourceType in resourceTypes)
        {
            Vector2 seedTile = new(
                mapCenter.X + random.Next(-maxRangeFromCenterTiles, maxRangeFromCenterTiles),
                mapCenter.Y + random.Next(-maxRangeFromCenterTiles, maxRangeFromCenterTiles)
            );
            
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Vector2 targetTile = new(x, y);
                    TerrainTile terrain = new(this, assets, targetTile);
                    float distanceFromSeed = Vector2.Distance(seedTile, targetTile);
                    float probability = MathF.Max(0, 1 - (distanceFromSeed * decayFactor));
                    if (random.NextDouble() < probability)
                    {
                        terrain = new HarvestableTerrainTile(this, assets, HarvestableTerrainTileDatabase.Data[resourceType], targetTile);
                        gameEntities.Add(terrain);
                    }
                    if (terrain is not HarvestableTerrainTile && map[x,y] != null){ continue; }
                    map[x, y] = terrain;
                }
            }
        }
        RemoveInvalidHarvestableTerrainTile();
    }

    public int Gold()
    {
        return playerGold;
    }

    public int AddGold(int amt = 1)
    {
        playerGold += amt;
        return playerGold;
    }

    public bool SubtractGold(int amt = 1)
    {
        if (playerGold>=amt){ 
            playerGold -= amt;
            return true;
        }
        return false;
    }

    public int SetGold(int amt)
    {
        playerGold = amt;
        return playerGold;
    }

    // not sure this is actually doing anything right or even useful tbh. 7/18/26
    private bool IsInvalidHarvestableTerrainTile(Entity entity)
    {
        if (entity is HarvestableTerrainTile terrain)
        {
            Vector2 tile = GetTileCoordinates(terrain.WorldPosition);
            bool isOutOfMap = 
                tile.X < 0
                || tile.Y < 0
                || tile.X >= map.GetLength(0)
                || tile.Y >= map.GetLength(1);

            return isOutOfMap || map[(int)tile.X, (int)tile.Y] != terrain;
        } else {
            return false;
        }
    }

    public void RemoveInvalidHarvestableTerrainTile()
    {
        gameEntities.RemoveAll(IsInvalidHarvestableTerrainTile);
    }

    internal Vector2 GetTileCoordinates(Vector2 worldPosition)
    {
        return new Vector2(
            MathF.Floor(worldPosition.X / tileSizePixels),
            MathF.Floor(worldPosition.Y / tileSizePixels)
        );
    }

    public Vector2 GetContainingTileScreenCoordinates(Vector2 worldPosition)
    {
        return worldPosition - camCenter + new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
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

    public Vector2 WorldToScreen(Vector2 worldPos)
    {
        return worldPos
            - camCenter
            + new Vector2(
                game.ViewportResolution.width / (game.scale * 2),
                game.ViewportResolution.height / (game.scale * 2)
            );
    }

    public Vector2 ScreenToWorld(Vector2 screenPos)
    {
        return screenPos
            + camCenter
            - new Vector2(
                game.ViewportResolution.width / (game.scale * 2),
                game.ViewportResolution.height / (game.scale * 2)
            );
    }

    public override void Update(GameTime gameTime) 
    {
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
                gameEntities.RemoveAll(entity => entity is StationBuilder);
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
        if (player.screenPosition.X < edgeWidth){ camCenter.X -= player.mvSpdPx; }
        if (player.screenPosition.Y < edgeWidth){ camCenter.Y -= player.mvSpdPx; }
        if (player.screenPosition.X > game.VirtualResolution.width - edgeWidth - tileSizePixels){ camCenter.X += player.mvSpdPx; }
        if (player.screenPosition.Y > game.VirtualResolution.height - edgeWidth - tileSizePixels){ camCenter.Y += player.mvSpdPx; }
        // ------------------------------------------------------------------------------------

        // TEMP test gold
        // if(_inputManager.IsKeyPressed(Keys.Space)) { AddGold(); }
        
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (!hasHoveredMenu) {
            spriteBatch.Draw(
                hoverIndicatorTexture,
                GetContainingTileScreenCoordinates(mouseWorldMapPosition),
                null,
                Color.White,
                0f,
                new Vector2(hoverIndicatorTexture.Width / 2, hoverIndicatorTexture.Height / 2),
                1f,
                SpriteEffects.None,
                0f
            );   
        }

        int UImargin = 8;
        int UIpadding = 4;
        spriteBatch.Draw(
            assets.goldCoin,
            new Vector2(UImargin, game.VirtualResolution.height-assets.goldCoin.Height-UImargin),
            Color.White
        );
        spriteBatch.DrawString(
            assets.Pixel1Font,
            $"{playerGold}",
            new Vector2(UImargin+UIpadding+assets.goldCoin.Width, game.VirtualResolution.height-assets.goldCoin.Height-UImargin),
            Color.Black,
            0,
            new Vector2(0,0),
            2,
            new SpriteEffects(),
            1
        );
    }
}
