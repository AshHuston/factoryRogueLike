using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factoryRL.Inputs;
namespace factoryRL.GameObjects;

public class Scene
{
    protected List<Entity> gameEntities = [];
    protected List<Entity> entitiesToAdd = [];
    protected List<Entity> entitiesToRemove = [];
    protected readonly GameAssets assets;
    readonly Game1 game;
    public InputManager _inputManager;
    internal int tileSizePixels;
    internal Texture2D backgroundTexture;

    public Scene(Game1 _game, GameAssets _assets)
    {
        game = _game;
        assets = _assets;
        _inputManager = game._inputManager;
    }

    public void Add(Entity entity)
    {
        entitiesToAdd.Add(entity);
    }

    public void Remove(Entity entity)
    {
        entitiesToRemove.Add(entity);
    }

    public virtual void DrawBackground(SpriteBatch spriteBatch)
    {
    }

    public virtual void Update(GameTime gameTime) 
    {
        foreach (var e in gameEntities)
        {
            e.Update(gameTime);
        }

        gameEntities.AddRange(entitiesToAdd);
        entitiesToAdd.Clear();
        gameEntities.RemoveAll(entitiesToRemove.Contains);
        entitiesToRemove.Clear();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        DrawBackground(spriteBatch);
        foreach (var e in gameEntities)
        {
            e.Draw(spriteBatch);
        }
    }
}
