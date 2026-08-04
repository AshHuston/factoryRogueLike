using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects;

public class LumberMill : WorkStation
{
    public static new readonly ResourceType[] mineableResourceTypes =
    [
        ResourceType.Wood
    ];

    public LumberMill(World world, GameAssets assets, HarvestableTerrainTile targetTerrain) : base(world, assets, 2, targetTerrain)
    {
        maxNumWorkers = 2;
        _texture = assets.LumberMill;
    }

    public override void Update(GameTime gameTime)
    {
        if (world.game.perks.IsActive(perks.Perk.INCREASE_WORKERS_TIMBERMILL)) { maxNumWorkers = baseMaxNumWorkers + 1; }
        if (world.game.perks.IsActive(perks.Perk.INCREASE_SPEED_TIMBERMILL)) { mineSpeedMultiplier = 1.5f; }
        base.Update(gameTime);
    }
}
