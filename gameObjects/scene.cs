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
    readonly GameAssets assets;
    readonly Game1 game;
    public InputManager _inputManager;

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

    public void Update(GameTime gameTime) 
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

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var e in gameEntities)
        {
            e.Draw(spriteBatch);
        }
    }
}
