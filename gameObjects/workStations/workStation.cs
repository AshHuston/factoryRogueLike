using System.Collections.Generic;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public abstract class WorkStation : Entity
{
    public int maxNumWorkers;
    public int currentNumWorkers = 0;
    public TerrainTile targetTerrain;
    internal List<Worker> assignedWorkers = [];
    public static readonly ResourceType[] mineableResourceTypes = [];
    public Inventory Inventory { get; } = new();
    private int harvestTimeRemainingMiliseconds;
    public ResourceItemType exportType = ResourceItemType.None;

    public WorkStation(World _world, GameAssets _assets, int maxWorkers, TerrainTile _targetTerrain)
    {
        world = _world;
        assets = _assets;
        targetTerrain = _targetTerrain;
        WorldPosition = targetTerrain.WorldPosition;
        maxNumWorkers = maxWorkers;

        if (targetTerrain is HarvestableTerrainTile t)
        {
            exportType = t.HarvestableTerrainTileData.ItemType;
        }
    }

    public bool CanAssignWorker()
    {
        return currentNumWorkers < maxNumWorkers;
    }

    public bool AssignWorker(Worker worker)
    {
        if (CanAssignWorker())
        {
            assignedWorkers.Add(worker);
            currentNumWorkers++;
            return true;
        }
        return false;
    }

    public bool UnassignWorker(Worker worker)
    {
        if (assignedWorkers.Remove(worker))
        {
            currentNumWorkers--;
            return true;
        }
        return false;
    }

    public override void Update(GameTime gameTime)
    {
        currentNumWorkers = assignedWorkers.Count;

        if (targetTerrain is HarvestableTerrainTile t) {
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                harvestTimeRemainingMiliseconds -= (int)(deltaTime*currentNumWorkers);
                t.harvestProgressWheel.SetProgress(1 - (float)harvestTimeRemainingMiliseconds / t.HarvestableTerrainTileData.MiningTimeMiliseconds);
                t.harvestProgressWheel.screenPosition = screenPosition;

                if (harvestTimeRemainingMiliseconds <= 0)
                {
                    var (type, amount) = t.HarvestResource();
                    Inventory.Add(type, amount);
                    harvestTimeRemainingMiliseconds = t.HarvestableTerrainTileData.MiningTimeMiliseconds;
                }
            }
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}
