using System;
using System.Collections.Generic;
using System.Linq;
using factoryRL.Functions;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using factoryRL.perks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public abstract class WorkStation : Entity
{
    public int baseMaxNumWorkers;
    public int maxNumWorkers;
    public int currentNumWorkers = 0;
    public TerrainTile targetTerrain;
    internal List<Worker> assignedWorkers = [];
    public static readonly ResourceType[] mineableResourceTypes = [];
    public Inventory Inventory { get; } = new();
    private int harvestTimeRemainingMiliseconds;
    public ResourceItemType exportType = ResourceItemType.None;
    internal TextMenu menu;
    internal WorkStationPanel panel;
    internal float mineSpeedMultiplier = 1f;

    public WorkStation(World _world, GameAssets _assets, int maxWorkers, TerrainTile _targetTerrain)
    {
        world = _world;
        assets = _assets;
        targetTerrain = _targetTerrain;
        WorldPosition = targetTerrain.WorldPosition;
        maxNumWorkers = maxWorkers;
        baseMaxNumWorkers = maxWorkers;

        if (targetTerrain is HarvestableTerrainTile t)
        {
            exportType = t.HarvestableTerrainTileData.ItemType;
        }

        InitializeMenuAndPanel();
    }

    internal virtual void InitializeMenuAndPanel()
    {
        int menuWidth = 80;
        int menuHeight = 100;
        int menuMargin = 10;
        SpriteFont font = assets.Pixel1Font;
        int panelPaddingPx = 10;
        int panelWidth = menuWidth;
        int panelItemHeight = 10;
        int uniqueItems = Inventory.GetUniqueItemCount();
        int panelHeight = ((uniqueItems+2)*panelPaddingPx) + ((1+uniqueItems)*panelItemHeight);
        int panelMargin = menuMargin;
        int panelY = (int)(1.5*panelMargin) + menuHeight;

        menu = new TextMenu(
            world,
            assets,
            new Rectangle(world.game.VirtualResolution.width-menuWidth-menuMargin, menuMargin, menuWidth, menuHeight),
            [
                new TextMenuOption("X", font, Color.Red, () => CloseMenu()),
                new TextMenuOption("+ Worker", font, Color.Black, () => AssignWorker()),
                new TextMenuOption("- Worker", font, Color.Black, () => UnassignWorker()),
                // Upgrade
                // Sell/scrap
                // Vein info
            ],
            menuWidth,
            menuHeight
        );

        panel = new WorkStationPanel(
            world,
            assets,
            new Rectangle(world.game.VirtualResolution.width-panelMargin-panelWidth, panelY, panelWidth, panelHeight),
            this,
            panelWidth,
            panelHeight
        );
    }

    public bool CanAssignWorker()
    {
        return currentNumWorkers < maxNumWorkers;
    }

    public bool AssignWorker(Worker worker)
    {
        if (!assignedWorkers.Contains(worker) && CanAssignWorker())
        {
            worker.UnasignFromAll();
            assignedWorkers.Add(worker);
            worker.setTargetPosition(this);
            return true;
        }
        return false;
    }

    public bool AssignWorker()
    {
        Worker foundWorker = world.FindClosestEntity<Worker>(WorldPosition, (w) => !assignedWorkers.Contains(w) && w.IsIdle());
        return foundWorker == null ? false : AssignWorker(foundWorker);
    }

    public bool UnassignWorker(Worker worker)
    {
        if (assignedWorkers.Remove(worker))
        {
            worker.Alpha = 1;
            return true;
        }
        return false;
    }

    public bool UnassignWorker()
    {
        return assignedWorkers.Count > 0 
            ? UnassignWorker(assignedWorkers[0])
            : false;
    }

    private void OpenMenu()
    {
        if (menu.isOpen) { return; }
        foreach (WorkStation w in world.gameEntities.OfType<WorkStation>())
        {
            w.CloseMenu();
        }
        menu.open();
    }

    internal void CloseMenu()
    {
        menu.close();
    }

    public override void Update(GameTime gameTime)
    {
        currentNumWorkers = assignedWorkers.Count;

        if (currentNumWorkers > 0 && targetTerrain is HarvestableTerrainTile t) 
        {
            float baseMineSpd = 0.5f;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float perkMineSpeedMultiplier = world.game.perks.IsActive(Perk.INCREASE_WORKER_MINESPEED) ? 1.25f : 1;
            harvestTimeRemainingMiliseconds -= (int)(
                baseMineSpd *
                deltaTime *
                currentNumWorkers *
                perkMineSpeedMultiplier *
                mineSpeedMultiplier
            );
            t.harvestProgressWheel.SetProgress(1 - (float)harvestTimeRemainingMiliseconds / t.HarvestableTerrainTileData.MiningTimeMiliseconds);
            t.harvestProgressWheel.screenPosition = screenPosition;

            if (harvestTimeRemainingMiliseconds <= 0)
            {
                var (type, amount) = t.HarvestResource();
                Inventory.Add(type, amount);
                harvestTimeRemainingMiliseconds = t.HarvestableTerrainTileData.MiningTimeMiliseconds;
            }
        }
        
        if (EntityFunctions.Clicked(targetTerrain)||EntityFunctions.Clicked(this)) { OpenMenu(); }
        

        foreach (Worker w in assignedWorkers) {
            if (w.WorldPosition == WorldPosition){ w.Alpha = 0; }
        }

        if (menu.isOpen && !panel.isOpen) { panel.open(); }
        if (menu.isClosing) { panel.close(); }

        base.Update(gameTime);  
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (menu.isOpen)
        {
            spriteBatch.Draw(
                assets.hoveredTileIndicator,
                screenPosition,
                Color.CornflowerBlue
            );
        }
    }
}
