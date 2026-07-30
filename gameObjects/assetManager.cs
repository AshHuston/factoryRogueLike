using Microsoft.Xna.Framework.Graphics;
namespace factoryRL.GameObjects;

public class GameAssets
{
    public Texture2D Restart;
    public Texture2D backgroundTextureTile;
    public Texture2D ProgressWheel;
    internal Texture2D pixel;
    internal Texture2D workerIcon;
    internal Texture2D goldCoin;
    internal Texture2D heap;

    public Texture2D IronOre { get; internal set; }
    public Texture2D CopperOre { get; internal set; }
    public Texture2D CoalOre { get; internal set; }
    public Texture2D StoneOre { get; internal set; }
    public Texture2D Forest { get; internal set; }
    public Texture2D StoneTerrain { get; internal set; }
    public Texture2D CoalTerrain { get; internal set; }
    public Texture2D IronTerrain { get; internal set; }
    public Texture2D CopperTerrain { get; internal set; }
    public Texture2D Player { get; internal set; }
    public Texture2D Worker { get; internal set; }
    public Texture2D MovmentIndicicator { get; internal set; }
    public Texture2D hoveredTileIndicator { get; internal set; }
    public Texture2D Mine { get; internal set; }
    public Texture2D LumberMill { get; internal set; }
    public Texture2D MenuBackgroundNS { get; internal set; }
    public SpriteFont Pixel1Font { get; internal set; }
    public Texture2D TimberYard { get; internal set; }
    public Texture2D Courier { get; internal set; }
    public Texture2D Warehouse { get; internal set; }
    public Texture2D logItem { get; internal set; }
}

// 3/3/26 Pretty sure this needs to just have an array there and have a function to load them in, but for now this is fine.  I just want to get something on the screen.
