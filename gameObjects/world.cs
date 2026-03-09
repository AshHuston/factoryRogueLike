using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factoryRL.Inputs;
using factoryRL.GameObjects.Terrain;
using System.Diagnostics;
using System;
namespace factoryRL.GameObjects;

public class World : Scene
{
    private readonly int tileSizePixels = 32;
    private readonly Entity[,] map = new Entity[500, 500];
    private int removeThisLater = 0;

    public World(Game1 game, GameAssets assets) : base(game, assets)
    {
        Console.WriteLine("Initializing world...");
        GenerateMap();
        HarvestableTerrain testOre = new HarvestableTerrain(assets.IronTerrain, new Vector2(10, 10));
        map[0, 0] = testOre;
        gameEntities.Add(testOre);
        Console.WriteLine($"Created world with {gameEntities.Count} entities.");
    }

    public void GenerateMap()
    {
        // We probably want to pass in some seed data here but idk exactly what that looks like atm. So I am going to hardcode it for now.
        
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
                        x * tileSizePixels,
                        y * tileSizePixels
                    );
                }
            }
        }
    }

    public override void Update(GameTime gameTime) 
    {
        removeThisLater++;
        map[removeThisLater, removeThisLater] = new HarvestableTerrain(assets.IronTerrain, new Vector2(removeThisLater, removeThisLater));
        gameEntities.Add(map[removeThisLater, removeThisLater]);
        AdjustEntityPositions(); // I feel like we dont want to be doing this every frame. I am not sure though where we want to call it though.
        base.Update(gameTime);
    }
}
