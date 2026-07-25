using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using factoryRL.Functions;
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
        if (!assignedWorkers.Contains(worker) && CanAssignWorker())
        {
            assignedWorkers.Add(worker);
            worker.setTargetPosition(this);
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

        if (currentNumWorkers > 0 && targetTerrain is HarvestableTerrainTile t) 
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
        
        // TEMPORARY way to assign workers.
        if (world._inputManager.IsRightClick() && world._inputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.LeftShift, false) && EntityFunctions.Hovered(this))
        {
            Worker foundWorker = world.FindClosestEntity<Worker>(WorldPosition, (w) => !assignedWorkers.Contains(w));
            AssignWorker(foundWorker);
        }
        // ^^^

        foreach (Worker w in assignedWorkers) {
            if (w.WorldPosition == WorldPosition){ w.Alpha = 0; }
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}
