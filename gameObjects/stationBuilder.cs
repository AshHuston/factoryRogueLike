using System;
using System.Collections.Generic;
using System.Linq;
using factoryRL.GameObjects.Resources;
using factoryRL.GameObjects.Terrain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;
public class StationBuilder : Entity
{
    private Type StationType;
    private readonly Dictionary<Type, Func<World, GameAssets, HarvestableTerrain, WorkStation>> _factories;

    public StationBuilder(World _world, GameAssets _assets, Type _stationType)
    {

        foreach (var builder in _world.gameEntities
            .OfType<StationBuilder>()
            .Where(b => !ReferenceEquals(b, this)))
        {
            _world.Remove(builder);
        }

        Dictionary<Type, Texture2D> _textures = new()
        {
            { typeof(Mine), _assets.Mine },
            { typeof(LumberMill), _assets.LumberMill },
        };

        world = _world;
        assets = _assets;

        _factories = new()
        {
            { typeof(Mine), (world, assets, targetTerrain) => new Mine(world, assets, targetTerrain) },
            { typeof(LumberMill), (world, assets, targetTerrain) => new LumberMill(world, assets, targetTerrain) },
        };
        
        _texture = _textures[_stationType];
        StationType = _stationType;
    }

    private Entity getTile(Vector2 worldTileCoords)
    {
        Entity worldTile = world.map[(int)worldTileCoords.X, (int)worldTileCoords.Y];
        return worldTile;
    }

    private bool IsLegalTile(Vector2 worldTileCoords)
    {
        Entity tile = getTile(worldTileCoords);
        var field = StationType.GetField("mineableResourceTypes");
        var resources = (ResourceType[])field.GetValue(null);

        if (tile is HarvestableTerrain terrain)
        {
            if (resources.Contains(terrain.terrainData.Type)) { return true; }
        }

        if (resources.Length == 0) { return true; } // For now, non-harvesting stations can go anywhere.
        
        return false;
    }

    private WorkStation getNewStation(Vector2 worldTileCoords)
    {
        return _factories[StationType](world, assets, (HarvestableTerrain)getTile(worldTileCoords));
    }

    public override void Update(GameTime gameTime)
    {
        float offsetForScreenCenter = .25f;
        _position = world.mouseWorldMapPosition - world.camCenter + new Vector2(world.game.GraphicsDevice.Viewport.Width, world.game.GraphicsDevice.Viewport.Height)*offsetForScreenCenter - new Vector2(_texture.Width/2, _texture.Height/2);

        if (world.game._inputManager.IsLeftClick())
        {
            Vector2 worldTileCoords = world.GetTileCoordinates(world.mouseWorldMapPosition);
            if (IsLegalTile(worldTileCoords))
            {
                world.Add(getNewStation(worldTileCoords));
                world.Remove(this);
                // SOUND EFFECT -> Should be a weighty sound effect to *feel* the placement of a workstation.
            }
        }
        
        base.Update(gameTime);
    }
}
