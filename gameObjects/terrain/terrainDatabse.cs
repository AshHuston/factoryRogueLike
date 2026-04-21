using System.Collections.Generic;
using factoryRL.GameObjects.Resources;

namespace factoryRL.GameObjects.Terrain;

public static class TerrainDatabase
{
    private static GameAssets _assets;

    public static Dictionary<ResourceType, TerrainData> Data;

    public static void Initialize(GameAssets assets)
    {
        _assets = assets;

        Data = new Dictionary<ResourceType, TerrainData>()
        {
            {
                ResourceType.Iron,
                new TerrainData
                {
                    Name = "Iron",
                    Texture = _assets.IronTerrain,
                    Type = ResourceType.Iron,
                    ItemType = ResourceItemType.IronOre,
                    MiningTimeMiliseconds = 1500,
                }
            },
            {
                ResourceType.Copper,
                new TerrainData
                {
                    Name = "Copper",
                    Texture = _assets.CopperTerrain,
                    Type = ResourceType.Copper,
                    ItemType = ResourceItemType.CopperOre,
                    MiningTimeMiliseconds = 1500,
                }
            },
            {
                ResourceType.Coal,
                new TerrainData
                {
                    Name = "Coal",
                    Texture = _assets.CoalTerrain,
                    Type = ResourceType.Coal,
                    ItemType = ResourceItemType.Coal,
                    MiningTimeMiliseconds = 1000,
                }
            },
            {
                ResourceType.Stone,
                new TerrainData
                {
                    Name = "Stone",
                    Texture = _assets.StoneTerrain,
                    Type = ResourceType.Stone,
                    ItemType = ResourceItemType.Stone,
                    MiningTimeMiliseconds = 1000,
                }
            },
            {
                ResourceType.Wood,
                new TerrainData
                {
                    Name = "Wood",
                    Texture = _assets.Forest,
                    Type = ResourceType.Wood,
                    ItemType = ResourceItemType.Wood,
                    MiningTimeMiliseconds = 750,
                }
            },
        };
    }
}
