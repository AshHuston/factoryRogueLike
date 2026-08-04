using factoryRL.GameObjects.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects.Terrain;
public class HarvestableTerrainTile : TerrainTile
{
    public HarvestableTerrainTileData HarvestableTerrainTileData;
    public ProgressSprite harvestProgressWheel;
    private float harvestCountDowntimeFrames = 0;
    private const int harvestCountDowntimeFramesMax = 15;
    private float harvestProgressPrevFrame = 0;

    public HarvestableTerrainTile(World _world, GameAssets assets, HarvestableTerrainTileData _HarvestableTerrainTileData, Vector2 position): base(_world, assets, position)
    {
        HarvestableTerrainTileData = _HarvestableTerrainTileData;
        _texture = HarvestableTerrainTileData.Texture;
        harvestProgressWheel = new ProgressSprite(assets.ProgressWheel, 32, 32);
    }

    public override void Interact(Player player)
    {
       player.startHarvesting(this);
    }

    public (ResourceItemType Type, int Amount) HarvestResource(int amountToHarvest = 1)
    {
        HarvestableTerrainTileData.quantity -= amountToHarvest;
        return (HarvestableTerrainTileData.ItemType, amountToHarvest);   
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
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        harvestProgressWheel.Draw(spriteBatch);
    }
}
