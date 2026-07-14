using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;

namespace factoryRL.GameObjects;

public class Warehouse : WorkStation
{

    public static new readonly ResourceType[] mineableResourceTypes = [];

    public Warehouse(World world, GameAssets assets) : base(world, assets, 2, null)
    {
        maxNumWorkers = 0;
        _texture = assets.Warehouse;
    }
}
