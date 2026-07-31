using System;
using System.Linq;
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
    public ReceiverContractPanel contractPanel;

    public Receiver(World world, GameAssets assets, TerrainTile targetTerrain) : base(world, assets, 0, targetTerrain)
    {
        maxNumWorkers = 0;
    }

    public void setContract(Contract contract)
    {
        currentContract = contract;
        InitializeMenuAndPanel();
        
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
                new TextMenuOption("Submit", font, Color.Black, () => AttemptCompleteContract()),
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

        contractPanel = new ReceiverContractPanel(
            world,
            assets,
            this,
            panelWidth
        );
    }

    internal virtual bool AttemptCompleteContract()
    {
        if (Inventory.Remove(currentContract.requirements))
        {
            menu.close();
            currentContract.timer.Stop(true);
            foreach (Worker w in world.gameEntities.OfType<Worker>().Where((w)=> w.route?.Target == this || w.route?.Source == this ))
            {
                w.UnasignFromAll();
                currentContract.CompleteContract((int)currentContract.timer.remainingTimeSeconds);
            }
            return true;
        }
        Console.WriteLine("We dont have it..."); // JUICE Could add a sound effect here.
        return false;
    }

    public override void Update(GameTime gameTime)
    {
        if (menu.isOpen && !contractPanel.isOpen) { contractPanel.open(); }
        if (menu.isClosing) { contractPanel.close(); }
        base.Update(gameTime);
    }
}
