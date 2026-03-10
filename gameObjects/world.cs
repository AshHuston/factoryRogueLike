using Microsoft.Xna.Framework;
using System;
namespace factoryRL.GameObjects;

public class World : Scene
{
    private readonly int tileSizePixels = 32;
    private readonly Entity[,] map = new Entity[500, 500];
    //private HarvestableTerrain testOre;

    public World(Game1 game, GameAssets assets) : base(game, assets)
    {
        Console.WriteLine("Initializing world...");
        GenerateMap();
        
        //map[0, 0] = testOre;
        Player player = new Player(game, this, assets, new Vector2(60, 60));
        gameEntities.Add(player);
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
        AdjustEntityPositions(); // I feel like we dont want to be doing this every frame. I am not sure though where we want to call it though.
        base.Update(gameTime);
    }
}
