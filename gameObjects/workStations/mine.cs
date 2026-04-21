using System.Collections.Generic;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public abstract class Mine : WorkStation
{
    public Mine(World world, GameAssets assets, HarvestableTerrain targetTerrain) : base(world, assets, 2, targetTerrain)
    {
        maxNumWorkers = 2;
        mineableResourceTypes = [];
        //_texture = assets.Mine;
    }
}
