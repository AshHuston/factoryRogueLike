using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public abstract class Menu : Entity
{
    public Rectangle screenBounds;
    public bool isHovered;

    public override void Update(GameTime gameTime)
    {
        isHovered = screenBounds.Contains(world._inputManager.MouseScreenPosition);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}
