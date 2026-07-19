using System;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects;

public class Warehouse : WorkStation
{
    public static new readonly ResourceType[] mineableResourceTypes = [];

    public Warehouse(World world, GameAssets assets, TerrainTile targetTerrain) : base(world, assets, 0, targetTerrain)
    {
        maxNumWorkers = 0;
        _texture = assets.Warehouse;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
