using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects.Resources;

public class ResourceData
{
    public string Name { get; set; }
    public Texture2D Texture { get; set; }
    public ResourceType Type { get; set; }
    public int fuelRank = 0;
}
