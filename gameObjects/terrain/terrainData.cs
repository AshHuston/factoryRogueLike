using factoryRL.GameObjects.Resources;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects.Terrain;

public class HarvestableTerrainTileData
{
    public string Name { get; set; }
    public int MiningTimeMiliseconds { get; set; }
    public Texture2D Texture { get; set; }
    public ResourceType Type { get; set; }
    public ResourceItemType ItemType { get; set; }
    public int quantity { get; set; }
}
