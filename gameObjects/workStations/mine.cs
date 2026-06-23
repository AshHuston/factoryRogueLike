using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;

namespace factoryRL.GameObjects;

public class Mine : WorkStation
{

    public static new readonly ResourceType[] mineableResourceTypes =
    [
        ResourceType.Iron,
        ResourceType.Coal,
        ResourceType.Copper,
        ResourceType.Stone
    ];

    public Mine(World world, GameAssets assets, HarvestableTerrain targetTerrain) : base(world, assets, 2, targetTerrain)
    {
        maxNumWorkers = 2;
        _texture = assets.Mine;
    }
}
