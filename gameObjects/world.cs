using factoryRL.contract;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace factoryRL.GameObjects;

public class World : Scene
{
    internal Entity[,] map;
    internal Point mapCenter;
    public Vector2 camCenter;
    internal Game1 game;
    private readonly Texture2D hoverIndicatorTexture;
    internal Vector2 mouseWorldMapPosition = new Vector2(0, 0);
    private readonly BuildMenu buildMenu;
    private bool isDisplayingBuildMenu = false;
    private int playerGold = 100;
    //                                                  DIAL These will change how often a cart spawns  v
    private (float Base, float Current, float increaseRate) miniContractCartSpawnRate = (0.000001f, 0.000001f, 0.00001f);
    private readonly Random random = new();

    public World(Game1 _game, GameAssets assets) : base(_game, assets)
    {
        game=_game;
        tileSizePixels = (int)Math.Round(32*game.scale);
        int mapWidth = 200;
        int mapHeight = 200;
        map = new Entity[mapWidth, mapHeight];
        mapCenter = new(mapWidth / 2, mapHeight / 2);
        camCenter = new Vector2(map.GetLength(0) * tileSizePixels / 2, map.GetLength(1) * tileSizePixels / 2);
        GenerateMap();
        Add(new CourierRouteAssigner(this, assets));

        backgroundTexture = assets.backgroundTextureTile;
        hoverIndicatorTexture = assets.hoveredTileIndicator;

        Add(new SalesHouse(this, assets, (TerrainTile)map[mapCenter.X, mapCenter.Y]));
        
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
                    }
                    if (terrain is not HarvestableTerrainTile && map[x,y] != null){ continue; }
                    if (map[x,y] is HarvestableTerrainTile) { continue; }
                    map[x, y] = terrain;
                    Add(terrain);
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
        int tilesX = (game.GraphicsDevice.Viewport.Width / tileSizePixels) + 4;
        int tilesY = (game.GraphicsDevice.Viewport.Height / tileSizePixels) + 4;
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

    public Contract GenerateMiniContract()
    {
        Random random = new();

        // 1. Difficulty from 1.0 to 10.0
        float difficulty = (float)(1 + random.NextDouble() * 9);

        // 2. Pick 1-2 resource types
        int resourceCount = difficulty switch
        {
            < 4 => 1,
            < 7 => random.Next(1, 3),
            _ => random.Next(2, 4)
        };

        ResourceItemType[] allResources =
        [
            ResourceItemType.Coal,
            ResourceItemType.IronOre,
            ResourceItemType.Stone,
            ResourceItemType.CopperOre,
            ResourceItemType.Wood
        ];

        List<(ResourceItemType Item, int Amount)> requirements = [];

        foreach (ResourceItemType resource in allResources
            .OrderBy(_ => random.Next())
            .Take(resourceCount))
        {
            int amount = random.Next(15, 61);
            requirements.Add((resource, amount));
        }

        int totalResources = requirements.Sum(r => r.Amount);

        // Difficulty affects time pressure.
        // Higher difficulty = less time per resource.
        float secondsPerResource = MathHelper.Lerp(
            4.0f,   // easy
            1.0f,   // hard
            (difficulty - 1) / 9f
        );

        int timeSeconds = (int)MathF.Round(
            totalResources * secondsPerResource
        );

        // Difficulty affects payout.
        float rewardMultiplier = MathHelper.Lerp(
            0.8f,   // easy
            2.0f,   // hard
            (difficulty - 1) / 9f
        );

        int goldReward = (int)MathF.Round(
            totalResources * rewardMultiplier
        );

        return new Contract(
            this,
            assets,
            requirements,
            timeSeconds,
            TimerDisplayType.Wheel,
            goldReward
        );
    }
    
    public TerrainTile FindEmptyTileNear(Vector2 tileCoords, int minRange = 3, int maxRange = 10)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int dx = random.Next(-maxRange, maxRange + 1);
            int dy = random.Next(-maxRange, maxRange + 1);

            float distance = MathF.Sqrt(dx * dx + dy * dy);

            if (distance < minRange || distance > maxRange)
                continue;

            int x = (int)tileCoords.X + dx;
            int y = (int)tileCoords.Y + dy;

            if (
                x < 0 ||
                y < 0 ||
                x >= map.GetLength(0) ||
                y >= map.GetLength(1)
            )
            {
                continue;
            }

            if (map[x, y] is TerrainTile terrain &&
                terrain is not HarvestableTerrainTile)
            {
                return terrain;
            }
        }

        return null;
    }

    private TerrainTile FindOffScreenTile()
    {
        Vector2 offset = new((random.Next(2) * 2 - 1) * game.VirtualResolution.width, (random.Next(2) * 2 - 1) * game.VirtualResolution.width);
        Vector2 offScreenCoords = camCenter+offset;
        Vector2 tileCoords = GetTileCoordinates(offScreenCoords);
        return (TerrainTile)map[(int)tileCoords.X, (int)tileCoords.Y]; 
    }

    private void SpawnMiniContractCart()
    {
        Add(new Cart(this, assets, FindOffScreenTile(), GenerateMiniContract()));
        miniContractCartSpawnRate.Current = miniContractCartSpawnRate.Base;
    }

    private bool CanSpawnMiniContractCart()
    {
        if (gameEntities.OfType<Cart>().Any()) { return false; }
        // Add any other falsy paths we may want.

        return true;
    }

    private void MaybeSpawnMiniContractCart()
    {
        if (!CanSpawnMiniContractCart()){ return; }
        float roll = random.NextSingle();
        if (roll <= miniContractCartSpawnRate.Current)
        {
            SpawnMiniContractCart();
            Console.WriteLine("Made a cart!");
            return;
        }
        miniContractCartSpawnRate.Current += miniContractCartSpawnRate.increaseRate;
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
                int totalGoldCost = 0;
                foreach (var entity in gameEntities.OfType<StationBuilder>().ToList())
                {
                    totalGoldCost += entity.goldCost;
                    gameEntities.Remove(entity);
                }   
                AddGold(totalGoldCost);
                isDisplayingBuildMenu = false;
            }
            else
            {
                buildMenu.open();
                isDisplayingBuildMenu = true;
            }
        }

        // TEMP This makes the character, not the mouse, move the screen. This is likely temporary.
        int edgeWidth = 2;
        int panSpdPx = 5;
        if (_inputManager.MouseScreenPosition.X < edgeWidth){ camCenter.X -= panSpdPx; }
        if (_inputManager.MouseScreenPosition.Y < edgeWidth){ camCenter.Y -= panSpdPx; }
        if (_inputManager.MouseScreenPosition.X > game.VirtualResolution.width - edgeWidth - tileSizePixels){ camCenter.X += panSpdPx; }
        if (_inputManager.MouseScreenPosition.Y > game.VirtualResolution.height - edgeWidth - tileSizePixels){ camCenter.Y += panSpdPx; }
        // ------------------------------------------------------------------------------------
        
        MaybeSpawnMiniContractCart();

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
