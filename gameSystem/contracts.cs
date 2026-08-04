using System.Collections.Generic;
using System.Linq;
using factoryRL.GameObjects;
using factoryRL.GameObjects.Resources;

namespace factoryRL.contract;

public class Contract
{
    public ContractTimer timer;
    public List<(ResourceItemType Item, int Quantity)> requirements;
    private Receiver receiver;
    private World world;
    public int goldvalue;
    private readonly GameAssets assets;

    public Contract(
        World _world,
        GameAssets _assets,
        List<(ResourceItemType Item, int Quantity)> _requirements,
        int timerSeconds,
        TimerDisplayType timerType,
        int _goldValue = 0
    )
    {
        assets = _assets;
        timer = new ContractTimer(_world, _assets, this, timerSeconds, timerType);
        world = _world;
        world.Add(timer);
        requirements = _requirements;
        goldvalue = _goldValue;
    }

    public void SetReceiver(Receiver _receiver)
    {
        receiver = _receiver;
    }

    public void Start()
    {
        timer.Start();
    }

    public virtual void CompleteContract(int excessSeconds)
    {
        int maxBonusGold = 200;
        (int flat, int bonus) gold = (
            flat: goldvalue,
            bonus: (int)(timer.remainingTimeSeconds / timer.totalSeconds * maxBonusGold)
        );

        Game1 g = world.game;
        g.currentScene =  new PerkSelectScreen(g, assets, g.currentRound, gold, g.perks.GetRandomPerks());
        foreach (Entity e in world.gameEntities)
        {
            if (e is Cart c) { c.Leave(); }
            if (e is ContractTimer t) { world.Remove(t); }
        }
    }

    public virtual void FailContract()
    {
        // Handle card leaving
        if (receiver is Cart c){
            c.Leave();
            world.Remove(timer);
            foreach (Worker w in world.gameEntities.OfType<Worker>().Where((w)=>w.route?.Target==receiver || w.route?.Source == receiver))
            {
                w.UnasignFromAll();
            }
            return;
        }

        // Or the gameover screen
        world.game.GoToGameOverScreen();
    }
}
