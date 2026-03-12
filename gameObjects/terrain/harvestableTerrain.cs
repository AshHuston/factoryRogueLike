using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects.Terrain;
public class HarvestableTerrain : Entity
{
    public TerrainData terrainData;
    public HarvestableTerrain(TerrainData _terrainData, Vector2 position)
    {
        terrainData = _terrainData;
        _texture = terrainData.Texture;
        _position = position;
    }

    public override void Update(GameTime gameTime)
    {
        // Logic for harvesting the terrain would go here
    }
}
