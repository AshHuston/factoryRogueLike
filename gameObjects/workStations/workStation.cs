using System;
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
    private HarvestableTerrain targetTerrain;
    internal List<Worker> assignedWorkers = [];
    public static readonly ResourceType[] mineableResourceTypes = [];
    public Inventory Inventory { get; } = new();
    private int harvestTimeRemainingMiliseconds;

    public WorkStation(World _world, GameAssets _assets, int maxWorkers, HarvestableTerrain _targetTerrain)
    {
        world = _world;
        assets = _assets;
        targetTerrain = _targetTerrain;
        _position = targetTerrain._position;
        maxNumWorkers = maxWorkers;
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
        _position = targetTerrain._position;
        currentNumWorkers = assignedWorkers.Count;
        //currentNumWorkers = 1; //TEMP

        if (targetTerrain != null) {
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                harvestTimeRemainingMiliseconds -= (int)(deltaTime*currentNumWorkers);
                targetTerrain.harvestProgressWheel.SetProgress(1 - (float)harvestTimeRemainingMiliseconds / targetTerrain.terrainData.MiningTimeMiliseconds);
                targetTerrain.harvestProgressWheel._position = _position;

                if (harvestTimeRemainingMiliseconds <= 0)
                {
                    var (type, amount) = targetTerrain.HarvestResource();
                    Inventory.Add(type, amount);
                    harvestTimeRemainingMiliseconds = targetTerrain.terrainData.MiningTimeMiliseconds;
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
