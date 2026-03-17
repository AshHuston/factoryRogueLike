using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace factoryRL.GameObjects;

public class World : Scene
{
    private readonly int tileSizePixels = 32;
    private Entity[,] map;
    public Vector2 camCenter;
    private Game1 game;
    internal Player player;

    public World(Game1 _game, GameAssets assets) : base(_game, assets)
    {
        game = _game;
        Console.WriteLine("Initializing world...");
        map = new Entity[500, 500];
        camCenter = new Vector2(map.GetLength(0) * tileSizePixels / 2, map.GetLength(1) * tileSizePixels / 2);
        GenerateMap();
        
        player = new Player(game, this, assets, camCenter);
        gameEntities.Add(player);
        gameEntities.Add(new Worker(game, this, assets, camCenter));
    }

    public void GenerateMap()
    {
        List<HarvestableTerrain> harvestableTerrains = new List<HarvestableTerrain>();
        Vector2 worldCenter = new Vector2(map.GetLength(0) / 2, map.GetLength(1) / 2);
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
                MathF.Floor(worldCenter.X) + random.Next(-maxRangeFromCenterTiles, maxRangeFromCenterTiles),
                MathF.Floor(worldCenter.Y) + random.Next(-maxRangeFromCenterTiles, maxRangeFromCenterTiles)
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
                        HarvestableTerrain terrain = new HarvestableTerrain(TerrainDatabase.Data[resourceType], targetTile);
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
                        (x * tileSizePixels) - camCenter.X + (game.GraphicsDevice.Viewport.Width / 2),
                        (y * tileSizePixels) - camCenter.Y + (game.GraphicsDevice.Viewport.Height / 2)
                    );
                }
            }
        }

        foreach (var entity in gameEntities)
        {
            if (entity is Meeple m)
            {
                m._position = new Vector2(
                m.worldPosition.X - camCenter.X + (game.GraphicsDevice.Viewport.Width / 2),
                m.worldPosition.Y - camCenter.Y + (game.GraphicsDevice.Viewport.Height / 2)
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

    public override void Update(GameTime gameTime) 
    {
        AdjustEntityPositions();
        player._position = new Vector2(
            player.worldPosition.X - camCenter.X + (game.GraphicsDevice.Viewport.Width / 2),
            player.worldPosition.Y - camCenter.Y + (game.GraphicsDevice.Viewport.Height / 2)
        );

        // TEMP This makes the character, not the mouse, move the screen. This is likely temporary.
        int edgeWidth = 65;
        if (player._position.X < edgeWidth){ camCenter.X -= player.mvSpdPx; }
        if (player._position.Y < edgeWidth){ camCenter.Y -= player.mvSpdPx; }
        if (player._position.X > game.GraphicsDevice.Viewport.Width - edgeWidth - tileSizePixels){ camCenter.X += player.mvSpdPx; }
        if (player._position.Y > game.GraphicsDevice.Viewport.Height - edgeWidth - tileSizePixels){ camCenter.Y += player.mvSpdPx; }
        
        // ------------------------------------------------------------------------------------
        
        base.Update(gameTime);
    }
}
