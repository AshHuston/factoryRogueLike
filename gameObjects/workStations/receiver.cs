using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using factoryRL.contract;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public abstract class Receiver : WorkStation
{
    public static new readonly ResourceType[] mineableResourceTypes = [];
    public Contract currentContract;

    public Receiver(World world, GameAssets assets, TerrainTile targetTerrain) : base(world, assets, 0, targetTerrain)
    {
        maxNumWorkers = 0;
    }

    internal override void InitializeMenuAndPanel()
    {
        int menuWidth = 80;
        int menuHeight = 50;
        int menuMargin = 10;
        SpriteFont font = assets.Pixel1Font;
        int panelPaddingPx = 10;
        int panelWidth = menuWidth;
        int panelItemHeight = 10;
        int uniqueItems = Inventory.GetUniqueItemCount();
        int panelHeight = ((uniqueItems+2)*panelPaddingPx) + ((1+uniqueItems)*panelItemHeight);
        int panelMargin = menuMargin;
        int panelY = (int)(1.5 * panelMargin) + menuHeight;

        menu = new TextMenu(
            world,
            assets,
            new Rectangle(world.game.VirtualResolution.width-menuWidth-menuMargin, menuMargin, menuWidth, menuHeight),
            [
                new TextMenuOption("X", font, Color.Red, () => CloseMenu()),
                new TextMenuOption("Complete Contract", font, Color.Black, () => AttemptCompleteContract()),
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

    internal virtual bool AttemptCompleteContract()
    {
        if (Inventory.Remove(currentContract.requirements))
        {
            Console.WriteLine("We have it!");
            menu.close();
            currentContract.timer.Stop(true);
            foreach (Worker w in world.gameEntities.OfType<Worker>().Where((w)=> w.route?.Target == this || w.route?.Source == this ))
            {
                w.UnasignFromAll();
                currentContract.CompleteContract((int)currentContract.timer.remainingTimeSeconds);
            }
            return true;
        }
        Console.WriteLine("We dont have it...");
        return false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
