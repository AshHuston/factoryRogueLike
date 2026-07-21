using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects.Terrain;
public class TerrainTile : Entity
{
    public Vector2 TilePosition;

    public TerrainTile(World _world, GameAssets _assets, Vector2 _tilePosition)
    {
        world = _world;
        TilePosition = _tilePosition;
        WorldPosition = TilePosition * world.tileSizePixels;
        _texture = _assets.pixel;
    }
}
