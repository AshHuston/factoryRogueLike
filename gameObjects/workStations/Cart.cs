using System;
using System.Linq;
using System.Reflection.PortableExecutable;
using factoryRL.contract;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects;

public class Cart : Receiver
{
    public static new readonly ResourceType[] mineableResourceTypes = [];
    private bool droppedInventory = false;
    private Vector2 spawnWorldPos;
    private Vector2 targetWorldPos;
    private int movementSpeedPxPerFrame = 2;

    public Cart(World world, GameAssets assets, TerrainTile targetTerrain, Contract _contract) : base(world, assets, targetTerrain)
    {
        spawnWorldPos = WorldPosition;
        targetWorldPos = FindWorldPosNearSalesHouse();
        _texture = assets.Cart;
        currentContract = _contract;
        currentContract.SetReceiver(this);
    }

    private Vector2 FindWorldPosNearSalesHouse()
    {
        SalesHouse salesHouse = world.FindClosestEntity<SalesHouse>(WorldPosition);
        // if (salesHouse == null){ return null; } Should we handle a world with no saleshouse?
        return world.FindEmptyTileNear(world.GetTileCoordinates(salesHouse.WorldPosition), 2, 4).WorldPosition;
    }

    private void DropInventory()
    {
        if (!droppedInventory)
        {
            world.Add(new Heap(world, assets, targetTerrain, Inventory));
            droppedInventory = true;
        }
    }

    public void Leave()
    {
        DropInventory();
        targetWorldPos = spawnWorldPos;
    }

    internal override bool AttemptCompleteContract()
    {
        if (base.AttemptCompleteContract())
        {
            Leave();
            currentContract.timer.Stop();
            world.AddGold(currentContract.goldvalue);
            return true;
        }
        return false;
    }

    private void StepTowards(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - WorldPosition;

        if (direction == Vector2.Zero) return;

        direction.Normalize();

        if (Vector2.Distance(WorldPosition, targetPosition) <= movementSpeedPxPerFrame)
        {
            WorldPosition = targetPosition;
        }else
        {
            WorldPosition += direction * movementSpeedPxPerFrame;
        }
    }

    public override void Update(GameTime gameTime)
    {
        StepTowards(targetWorldPos);
        currentContract.timer.SetWheelScreenPosition(screenPosition + new Vector2(0, -30));

        if (!currentContract.timer.HasStarted() && WorldPosition == targetWorldPos) { 
            targetTerrain = (TerrainTile)world.map[(int)world.GetTileCoordinates(WorldPosition).X, (int)world.GetTileCoordinates(WorldPosition).Y]; //This should really be set elsewhere.
            currentContract.Start();
            foreach ((ResourceItemType Item, int Quantity) r in currentContract.requirements)
            {
                Console.WriteLine(r);
            }
        }

        if (WorldPosition==targetWorldPos && targetWorldPos==spawnWorldPos) { world.Remove(this); }

        base.Update(gameTime);
    }
}
