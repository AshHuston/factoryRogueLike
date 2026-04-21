using System.Collections.Generic;
using factoryRL.GameObjects.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public abstract class WorkStation : Entity
{
    public int maxNumWorkers;
    public int currentNumWorkers;
    private List<Worker> assignedWorkers = [];
    private List<ResourceType> mineableResourceTypes = [];
    public List<(ResourceItemType Type, int Amount)> inventory = [];

    public override void Update(GameTime gameTime)
    {
        currentNumWorkers = assignedWorkers.Count;
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}
