using System;
using factoryRL.GameObjects.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects.Terrain;
public class HarvestableTerrain : Entity
{
    public TerrainData terrainData;
    public ProgressSprite harvestProgressWheel;
    private float harvestCountDowntimeFrames = 0;
    private const int harvestCountDowntimeFramesMax = 15;
    private float harvestProgressPrevFrame = 0;

    public HarvestableTerrain(World _world, GameAssets assets, TerrainData _terrainData, Vector2 position)
    {
        terrainData = _terrainData;
        _texture = terrainData.Texture;
        _position = position;
        world = _world;
        harvestProgressWheel = new ProgressSprite(assets.ProgressWheel, 32, 32);
    }

    public override void Interact(Player player)
    {
       player.startHarvesting(this);
    }

    public (ResourceItemType Type, int Amount) HarvestResource(int amountToHarvest = 1)
    {
        terrainData.quantity -= amountToHarvest;
        return (terrainData.ItemType, amountToHarvest);   
    }

    public override void Update(GameTime gameTime)
    {
        if (harvestProgressWheel.GetProgress() == harvestProgressPrevFrame)
        {
            harvestCountDowntimeFrames++;
            if (harvestCountDowntimeFrames >= harvestCountDowntimeFramesMax)
            {
                harvestProgressWheel.SetProgress(0);
            }
        }
        harvestProgressPrevFrame = harvestProgressWheel.GetProgress();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        harvestProgressWheel.Draw(spriteBatch);
    }
}
