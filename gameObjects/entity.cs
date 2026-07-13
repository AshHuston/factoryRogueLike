using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace factoryRL.GameObjects;

public abstract class Entity
{
    public bool IsAlive = true;
    public Texture2D _texture;
    public Vector2 _position;
    protected World world;
    protected GameAssets assets;
    public float Alpha = 1;

    public virtual void Interact(Player player) { }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, Color.White*Alpha);
    }
}
