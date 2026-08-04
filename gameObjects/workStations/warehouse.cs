using System;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public class Warehouse : WorkStation
{
    public static new readonly ResourceType[] mineableResourceTypes = [];

    public Warehouse(World world, GameAssets assets, TerrainTile targetTerrain) : base(world, assets, 0, targetTerrain)
    {
        maxNumWorkers = 0;
        _texture = assets.Warehouse;
    }

    internal override void InitializeMenuAndPanel()
    {
        int menuWidth = 80;
        int menuHeight = 30;
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
                // new TextMenuOption("+ Worker", font, Color.Black, () => AssignWorker()),
                // new TextMenuOption("- Worker", font, Color.Black, () => UnassignWorker()),
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

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
