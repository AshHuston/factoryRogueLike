using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects.Terrain;
public class TerrainTile : Entity
{
    public Vector2 TilePosition;

    public TerrainTile(World _world, GameAssets _assets, Vector2 _tilePosition)
    {
        world = _world;
        TilePosition = _tilePosition;
        WorldPosition = TilePosition * world.tileSizePixels;
        assets = _assets;
        _texture = assets.backgroundTextureTile;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {   
        if (_texture == assets.backgroundTextureTile) { return; }
        base.Draw(spriteBatch);
    }
}
