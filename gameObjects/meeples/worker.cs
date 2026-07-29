using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using factoryRL.courierRoute;
using factoryRL.GameObjects.Resources;

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

    public Worker(Game1 _game, World _world, GameAssets gameAssets, Vector2 _worldPosition) : base()
    {
        // This is just a test for now.
        // Inventory.Add(ResourceItemType.IronOre, 1);
        // ------------------------------

        WorldPosition = _worldPosition;
        world = _world;
        _texture = gameAssets.Worker;
        mvSpdPx = 3;
        targetWorldPosition = WorldPosition;
        assets = gameAssets;
    }
    
    public void setTargetPosition(Vector2 worldPosition)
    {
        targetWorldPosition = worldPosition;
    }

    public void setTargetPosition(Entity e)
    {
        setTargetPosition(e.WorldPosition);
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

    public void UnasignFromAll()
    {
        WorkStation[] stations = world.gameEntities.OfType<WorkStation>().ToArray();
        foreach (WorkStation station in stations)
        {
            station.UnassignWorker(this);
        }
        route = null;
        assignedWorkStation = null;
        _texture = assets.Worker;
        Alpha = 1;
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
            SetIdleTargetPosition(new(world.mapCenter.X, world.mapCenter.Y));
        }
        // ^Idle logic

        // CourierRoute logic
        if (route != null)
        {
            CourierState state = CourierState.GoingToPickup;
            if (route.Source != null) { targetWorldPosition = route.Source.targetTerrain.WorldPosition; }
            if (Inventory.Has(route.ResourceType))
            {
                state = CourierState.GoingToDropoff;
                targetWorldPosition = route.Target.targetTerrain.WorldPosition;    
            }

            if (Vector2.Distance(targetWorldPosition, WorldPosition) <= interactionRange)
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

            if (!Inventory.Has(route.ResourceType) && route.Source == null) 
            {
                UnasignFromAll();
            }
        }
        // ^CourierRoute logic

        if (world.game._inputManager.IsRightClick())
        {
            Rectangle hitBox = new((int)WorldPosition.X, (int)WorldPosition.Y, _texture.Width, _texture.Height);
            if (hitBox.Contains(world.game._inputManager.MouseWorldPosition) && Alpha != 0)// Invisible (inside a station) = nonclickable
            {
                UnasignFromAll();
            }
        }

        StepTowards(targetWorldPosition);

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (route != null && Inventory.Has(route.ResourceType))
        {
            Texture2D sprite = ResourceDatabase.ItemData[route.ResourceType].Texture;
            Vector2 offset = new(sprite.Width/2, -15);
            spriteBatch.Draw(
                sprite,
                screenPosition + offset,
                Color.White
            );
        }
    }
}
