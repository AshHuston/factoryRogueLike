using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;

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

    public Mine(World world, GameAssets assets, HarvestableTerrainTile targetTerrain) : base(world, assets, 2, targetTerrain)
    {
        maxNumWorkers = 2;
        _texture = assets.Mine;
    }

    public override void Update(GameTime gameTime)
    {
        if (world.game.perks.IsActive(perks.Perk.INCREASE_WORKERS_MINE)) { maxNumWorkers = baseMaxNumWorkers + 1; }
        if (world.game.perks.IsActive(perks.Perk.INCREASE_SPEED_MINE)) { mineSpeedMultiplier = 1.5f; }
        base.Update(gameTime);
    }
}
