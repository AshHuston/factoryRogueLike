using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects.Terrain
{
    public class HarvestableTerrain : Entity
    {
        
        public HarvestableTerrain(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            // Logic for harvesting the terrain would go here
        }

    }
}
