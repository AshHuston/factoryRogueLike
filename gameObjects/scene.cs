using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factoryRL.Inputs;
using System.Linq;
using System;
using factoryRL.GameObjects.Terrain;
namespace factoryRL.GameObjects;

public class Scene
{
    internal List<Entity> gameEntities = [];
    protected List<Entity> entitiesToAdd = [];
    protected List<Entity> entitiesToRemove = [];
    protected readonly GameAssets assets;
    readonly Game1 game;
    public InputManager _inputManager;
    internal int tileSizePixels;
    internal Texture2D backgroundTexture;
    public bool hasHoveredMenu = false;

    private static readonly Dictionary<Type, int> DrawOrder = new()
    {
        { typeof(TerrainTile), 0 },
        { typeof(HarvestableTerrainTile), 1 },
        { typeof(WorkStation), 2 },
        { typeof(Meeple), 3 },
        { typeof(Player), 4 },
    };

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

    public T FindClosestEntity<T>(
        Vector2 target,
        Func<T, bool> filter = null)
        where T : Entity
    {
        T closest = null;
        float closestDistSq = float.MaxValue;

        foreach (var e in gameEntities.OfType<T>())
        {
            if (filter != null && !filter(e))
                continue;

            float distSq = Vector2.DistanceSquared(e.WorldPosition, target);

            if (distSq < closestDistSq)
            {
                closestDistSq = distSq;
                closest = e;
            }
        }

        return closest;
    }

    public virtual void DrawBackground(SpriteBatch spriteBatch)
    {
    }

    private void SortGameEntitiesList()
    {
        static int GetOrder(Entity entity)
        {
            Type type = entity.GetType();
            while (type != null)
            {
                if (DrawOrder.TryGetValue(type, out int order))
                    return order;

                type = type.BaseType;
            }
            return int.MaxValue;
        }

        gameEntities = [.. gameEntities.OrderBy(GetOrder)];
    }

    public virtual void Update(GameTime gameTime) 
    {
        foreach (var e in gameEntities)
        {
            e.Update(gameTime);
        }

        hasHoveredMenu = gameEntities.Any(o => o is Menu menu && menu.isHovered);

        if (entitiesToAdd.Count > 0)
        {
            SortGameEntitiesList();
            gameEntities.AddRange(entitiesToAdd);
            entitiesToAdd.Clear();
        }
        if (entitiesToRemove.Count > 0)
        {
            SortGameEntitiesList();
            gameEntities.RemoveAll(entitiesToRemove.Contains);
            entitiesToRemove.Clear();
        }
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
