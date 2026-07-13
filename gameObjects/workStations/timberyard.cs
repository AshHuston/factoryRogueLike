using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;

namespace factoryRL.GameObjects;

public class TimberYard : WorkStation
{
    public static new readonly ResourceType[] mineableResourceTypes =
    [
        ResourceType.Wood
    ];

    public TimberYard(World world, GameAssets assets, HarvestableTerrain targetTerrain) : base(world, assets, 2, targetTerrain)
    {
        maxNumWorkers = 2;
        _texture = assets.TimberYard;
    }
}
