using System.Collections.Generic;
using factoryRL.contract;
using factoryRL.GameObjects;
using factoryRL.GameObjects.Resources;

public static class GameContracts
{

    public static Dictionary<int, Contract> DEMOContracts;

    public static void Initialize(World _world, GameAssets _assets)
    {

        DEMOContracts = new Dictionary<int, Contract>()
        {
            {
                1,
                new Contract(
                    _world,
                    _assets,
                    [
                        ( ResourceItemType.Coal, 25 ),
                        ( ResourceItemType.Wood, 25 ),
                    ],
                    400,
                    TimerDisplayType.Digital
                )
            },
            {
                2,
                new Contract(
                    _world,
                    _assets,
                    [
                        ( ResourceItemType.Coal, 65 ),
                        ( ResourceItemType.Stone, 45 ),
                        ( ResourceItemType.Wood, 65 ),
                    ],
                    350,
                    TimerDisplayType.Digital
                )
            },
            {
                3,
                new Contract(
                    _world,
                    _assets,
                    [
                        ( ResourceItemType.Coal, 100 ),
                        ( ResourceItemType.IronOre, 100 ),
                        ( ResourceItemType.Stone, 100 ),
                        ( ResourceItemType.Wood, 100 ),
                    ],
                    300,
                    TimerDisplayType.Digital
                )
            },
            {
                4,
                new Contract(
                    _world,
                    _assets,
                    [
                        ( ResourceItemType.Coal, 200 ),
                        ( ResourceItemType.IronOre, 200 ),
                        ( ResourceItemType.Stone, 200 ),
                        ( ResourceItemType.CopperOre, 100 ),
                        ( ResourceItemType.Wood, 200 ),
                    ],
                    250,
                    TimerDisplayType.Digital
                )
            },
            {
                5,
                new Contract(
                    _world,
                    _assets,
                    [
                        ( ResourceItemType.Coal, 250 ),
                        ( ResourceItemType.IronOre, 250 ),
                        ( ResourceItemType.Stone, 250 ),
                        ( ResourceItemType.CopperOre, 150 ),
                        ( ResourceItemType.Wood, 250 ),
                    ],
                    200,
                    TimerDisplayType.Digital
                )
            },
            {
                6,
                new Contract(
                    _world,
                    _assets,
                    [
                        ( ResourceItemType.Coal, 450 ),
                        ( ResourceItemType.IronOre, 450 ),
                        ( ResourceItemType.Stone, 450 ),
                        ( ResourceItemType.CopperOre, 300 ),
                        ( ResourceItemType.Wood, 450 ),
                    ],
                    300,
                    TimerDisplayType.Digital
                )
            },
        };
    }
}
