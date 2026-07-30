using System.Collections.Generic;
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

    public Contract(
        World _world,
        GameAssets _assets,
        List<(ResourceItemType Item, int Quantity)> _requirements,
        int timerSeconds,
        TimerDisplayType timerType,
        int _goldValue = 0
    )
    {
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
        // Handle moving onto the perk selection and the screen to move onto the next "blind"
    }

    public virtual void FailContract()
    {
        // Handle card leaving
        if (receiver is Cart c){
            c.Leave();
            world.Remove(timer);
            return;
        }

        // Or the gameover screen
    }
}
