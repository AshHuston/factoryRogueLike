using System.Collections.Generic;
using factoryRL.Inputs;
using Microsoft.Xna.Framework;
using factoryRL.GameObjects.Resources;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace factoryRL.GameObjects;

public class Worker : Meeple
{
    public int inventoryCapacity = 1;

    public Worker(Game1 _game, World _world, GameAssets gameAssets, Vector2 _worldPosition)
    {
        // This is just a test for now.
        AddToInventory(ResourceItemType.IronOre, 1);
        // ------------------------------

        worldPosition = _worldPosition;
        world = _world;
        _texture = gameAssets.Worker;
        mvSpdPx = 3;
        targetWorldPosition = worldPosition;
    }

    public override void Update(GameTime gameTime)
    {
        StepTowards(targetWorldPosition);

        Rectangle hitBox = new Rectangle((int)worldPosition.X, (int)worldPosition.Y, _texture.Width, _texture.Height);
        if (hitBox.Contains(world.game._inputManager.MouseWorldPosition))
        {
            if (world.game._inputManager.IsRightClick())
            {
                // This needs to remove all assignments from this worker, and then...
                // This needs to find a camp that is not full    v Camp           v filter for NOT full
                targetWorldPosition = world.FindClosestEntity<Player>(worldPosition).worldPosition;
            }
            if (world.game._inputManager.IsLeftClick())
            {
                //Got left clicked
            }
        }

        // Testing only
        //targetWorldPosition = world.player.worldPosition;
        // ------------------------------
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        // if (inventory.Count == 1)
        // {
        //     Texture2D resourceTexture = ResourceDatabase.Data[inventory[0].Type].Texture;
        //     Vector2 resourcePosition = _position - new Vector2(0, _texture.Height/2);
        //     spriteBatch.Draw(resourceTexture, resourcePosition, Color.White);
        // }
    }
}
