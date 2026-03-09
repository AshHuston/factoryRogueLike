using System.Collections.Generic;

namespace factoryRL.GameObjects.Resources;

public static class ResourceDatabase
{
    private static GameAssets _assets;

    public static Dictionary<ResourceType, ResourceData> Data;

    public static void Initialize(GameAssets assets)
    {
        _assets = assets;

        Data = new Dictionary<ResourceType, ResourceData>()
        {
            {
                ResourceType.Iron,
                new ResourceData
                {
                    Name = "Iron",
                    Texture = _assets.IronOre,
                    Type = ResourceType.Iron,
                }
            },
            {
                ResourceType.Copper,
                new ResourceData
                {
                    Name = "Copper",
                    Texture = _assets.CopperOre,
                    Type = ResourceType.Copper,
                }
            },
            {
                ResourceType.Coal,
                new ResourceData
                {
                    Name = "Coal",
                    Texture = _assets.CoalOre,
                    Type = ResourceType.Coal,
                    fuelRank = 2,
                }
            },
            {
                ResourceType.Stone,
                new ResourceData
                {
                    Name = "Stone",
                    Texture = _assets.StoneOre,
                    Type = ResourceType.Stone,
                }
            },
            {
                ResourceType.Wood,
                new ResourceData
                {
                    Name = "Wood",
                    Texture = _assets.Forest,
                    Type = ResourceType.Wood,
                    fuelRank = 1,
                }
            }
        };
    }
}
