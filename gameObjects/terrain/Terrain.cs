using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects.Terrain;
public class TerrainTile : Entity
{
    public TerrainTile(World _world, GameAssets _assets, Vector2 position)
    {
        _position = position;
        world = _world;
        _texture = _assets.pixel;
    }
}
