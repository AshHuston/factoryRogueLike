using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using factoryRL.courierRoute;

namespace factoryRL.GameObjects;

public enum CourierState
{
    GoingToPickup,
    PickingUp,
    GoingToDropoff,
    DroppingOff,
    Idle
}

public class Worker : Meeple
{
    public int inventoryCapacity = 1;
    public CourierRoute route = null;
    public WorkStation assignedWorkStation = null;

    public Worker(Game1 _game, World _world, GameAssets gameAssets, Vector2 _worldPosition)
    {
        // This is just a test for now.
        // Inventory.Add(ResourceItemType.IronOre, 1);
        // ------------------------------

        worldPosition = _worldPosition;
        world = _world;
        _texture = gameAssets.Worker;
        mvSpdPx = 3;
        targetWorldPosition = worldPosition;
        assets = gameAssets;
    }
    
    private void SetIdleTargetPosition(Vector2 rangeCenter)
    {
        if (IsIdle())
        {
            int range = 6;
            Vector2 offset = new(
                Random.Shared.Next(-range, range + 1),
                Random.Shared.Next(-range, range + 1)
            );
            targetWorldPosition = (rangeCenter+offset)*world.tileSizePixels;
        }
    }

    private void UnasignFromAll()
    {
        WorkStation[] stations = world.gameEntities.OfType<WorkStation>().ToArray();
        foreach (WorkStation station in stations)
        {
            station.UnassignWorker(this);
        }
        route = null;
        assignedWorkStation = null;
        _texture = assets.Worker;
    }

    public bool IsIdle()
    {
        foreach (WorkStation station in world.gameEntities.OfType<WorkStation>())
        {
            if (station.assignedWorkers.Contains(this))
            {
                return false;
            }
        }
        assignedWorkStation = null;
        return route == null;
    }

    public void AssignRoute(CourierRoute routeToAssign)
    {
        route = new CourierRoute(routeToAssign);
        _texture = assets.Courier;
    } 

    public override void Update(GameTime gameTime)
    {
        // Idle logic
        int avgFramesToMoveWhileIdle = 300;
        if (Random.Shared.Next(0, avgFramesToMoveWhileIdle) == 0)
        {
            SetIdleTargetPosition(world.mapCenter);
        }
        // ^Idle logic

        // CourierRoute logic
        if (route != null)
        {
            CourierState state = CourierState.GoingToPickup;
            targetWorldPosition = route.Source.targetTerrain._position;
            if (Inventory.Has(route.ResourceType))
            {
                state = CourierState.GoingToDropoff;
                targetWorldPosition = route.Target.targetTerrain._position;    
            }

            if (Vector2.Distance(targetWorldPosition, worldPosition) <= interactionRange)
            {
                if (state == CourierState.GoingToPickup)
                {
                    // Pick up item
                    if (route.Source.Inventory.Has(route.ResourceType))
                    {
                        route.Source.Inventory.Remove(route.ResourceType);
                        Inventory.Add(route.ResourceType);
                    }
                }
                else if (state == CourierState.GoingToDropoff)
                {
                    // Deposit item
                    if (Inventory.Has(route.ResourceType))
                    {
                        Inventory.Remove(route.ResourceType);
                        route.Target.Inventory.Add(route.ResourceType);
                    }
                }
            }
        }
        // ^CourierRoute logic

        StepTowards(targetWorldPosition);

        Rectangle hitBox = new Rectangle((int)worldPosition.X, (int)worldPosition.Y, _texture.Width, _texture.Height);
        if (world.game._inputManager.IsRightClick())
        {
            if (hitBox.Contains(world.game._inputManager.MouseWorldPosition) && Alpha != 0)// Invisible (inside a station) = nonclickable
            {
                UnasignFromAll();
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}
