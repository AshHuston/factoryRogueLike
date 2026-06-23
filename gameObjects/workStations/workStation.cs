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
    public List<(ResourceItemType Type, int Amount)> inventory = [];
    private int harvestTimeRemainingMiliseconds;

    public WorkStation(World _world, GameAssets _assets, int maxWorkers, HarvestableTerrain _targetTerrain)
    {
        world = _world;
        assets = _assets;
        targetTerrain = _targetTerrain;
        _position = targetTerrain._position;
        maxNumWorkers = maxWorkers;
    }

    public void AddToInventory(ResourceItemType type, int amount)
    {
        var existingItem = inventory.Find(item => item.Type == type);
        if (existingItem != default)
        {
            existingItem.Amount += amount;
        }
        else
        {
            inventory.Add((type, amount));
        }
    }

    public (ResourceItemType Type, int Amount) RemoveFromInventory(ResourceItemType type, int amount)
    {
        var existingItem = inventory.Find(item => item.Type == type);
        if (existingItem != default)
        {
            int amountToRemove = Math.Min(existingItem.Amount, amount);
            existingItem.Amount -= amountToRemove;
            if (existingItem.Amount <= 0)
            {
                inventory.Remove(existingItem);
            }
            return (type, amountToRemove);
        }
        return (type, 0);
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
                    AddToInventory(type, amount);
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
