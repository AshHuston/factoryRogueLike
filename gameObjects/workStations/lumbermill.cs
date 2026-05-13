using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;

namespace factoryRL.GameObjects;

public class LumberMill : WorkStation
{
    public LumberMill(World world, GameAssets assets, HarvestableTerrain targetTerrain) : base(world, assets, 2, targetTerrain)
    {
        maxNumWorkers = 2;
        mineableResourceTypes = [ResourceType.Wood];
        _texture = assets.LumberMill;
    }
}
