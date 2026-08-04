using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects;

public class SalesHouse : Receiver
{
    public static new readonly ResourceType[] mineableResourceTypes = [];

    public SalesHouse(World world, GameAssets assets, TerrainTile targetTerrain) : base(world, assets, targetTerrain)
    {
        _texture = assets.saleshouse;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
