using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects.Terrain;
public class HarvestableTerrain : Entity
{
    public TerrainData terrainData;
    private int minetimeRemainingMiliseconds;
    private Meeple miningMeeple = null;
    private ProgressSprite miningProgressWheel;

    public HarvestableTerrain(World _world, GameAssets assets, TerrainData _terrainData, Vector2 position)
    {
        terrainData = _terrainData;
        _texture = terrainData.Texture;
        _position = position;
        world = _world;
        miningProgressWheel = new ProgressSprite(assets.ProgressWheel, 32, 32);
    }

    public override void Interact(Meeple meeple)
    {
        miningMeeple = meeple;
        minetimeRemainingMiliseconds = terrainData.MiningTimeMiliseconds;
    }

    public override void Update(GameTime gameTime)
    {
        if (miningMeeple != null) {
            if (world._inputManager.IsLeftClickReleased())
            {
                miningMeeple = null;
                minetimeRemainingMiliseconds = 0;
            }
            else
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                minetimeRemainingMiliseconds -= (int)deltaTime;
                miningProgressWheel.SetProgress(1 - (float)minetimeRemainingMiliseconds / terrainData.MiningTimeMiliseconds);
                miningProgressWheel._position = _position - new Vector2(12, 12); // Should move this probably

                if (minetimeRemainingMiliseconds <= 0 && miningMeeple != null)
                {
                    miningMeeple.inventory.Add(terrainData.Type);
                    minetimeRemainingMiliseconds = terrainData.MiningTimeMiliseconds;
                }
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (miningMeeple != null){
            miningProgressWheel.Draw(spriteBatch);
        }
    }
}
