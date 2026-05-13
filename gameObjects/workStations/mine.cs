using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;

namespace factoryRL.GameObjects;

public class Mine : WorkStation
{
    public Mine(World world, GameAssets assets, HarvestableTerrain targetTerrain) : base(world, assets, 2, targetTerrain)
    {
        maxNumWorkers = 2;
        mineableResourceTypes = [
            ResourceType.Iron,
            ResourceType.Coal,
            ResourceType.Copper,
            ResourceType.Stone
        ];
        _texture = assets.Mine;
    }
}
