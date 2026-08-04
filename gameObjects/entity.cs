using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace factoryRL.GameObjects;

public abstract class Entity
{
    public bool IsAlive = true;
    public Texture2D _texture;
    public Vector2 WorldPosition = new(-5000, -5000); //Just to avoid new sprites flickering on screen
    public World world;
    protected GameAssets assets;
    public float Alpha = 1;
    public Vector2 screenPosition;
    public bool isUIElement = false;

    private Vector2 GetScreenPosition(bool isUIElement)
    {
        if (!isUIElement && world != null){
            return world.WorldToScreen(WorldPosition);
        }
        return screenPosition;
    }

    public virtual void Interact(Player player) { }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        screenPosition = GetScreenPosition(isUIElement);
        if (_texture != null){
            spriteBatch.Draw(
                _texture,
                screenPosition,
                Color.White * Alpha
            );
        }
    }
}
