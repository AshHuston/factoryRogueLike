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
    private readonly Dictionary<Type, Func<World, GameAssets, TerrainTile, WorkStation>> _factories;

    public StationBuilder(World _world, GameAssets _assets, Type _stationType)
    {
        isUIElement = true;
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
            { typeof(TimberYard), _assets.TimberYard },
            { typeof(Warehouse), _assets.Warehouse },
        };

        world = _world;
        assets = _assets;

        _factories = new()
        {
            { typeof(Mine), (world, assets, terrainTile) => new Mine(world, assets, (HarvestableTerrainTile)terrainTile ) },
            { typeof(LumberMill), (world, assets, terrainTile) => new LumberMill(world, assets, (HarvestableTerrainTile)terrainTile) },
            { typeof(TimberYard), (world, assets, terrainTile) => new TimberYard(world, assets, (HarvestableTerrainTile)terrainTile) },
            { typeof(Warehouse), (world, assets, terrainTile) => new Warehouse(world, assets,  terrainTile) },
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
        if (world.gameEntities.OfType<WorkStation>().Any(ws => ws.targetTerrain == tile)) { return false; }
        var field = StationType.GetField("mineableResourceTypes");
        var resources = (ResourceType[])field.GetValue(null);
        if (tile is HarvestableTerrainTile terrain)
        {
            if (resources.Contains(terrain.HarvestableTerrainTileData.Type)) { return true; }
        }

        if (resources.Length == 0) { return true; }
        
        return false;
    }

    private WorkStation getNewStation(Vector2 worldTileCoords)
    {
        return _factories[StationType](world, assets, (TerrainTile)getTile(worldTileCoords));
    }

    public override void Update(GameTime gameTime)
    {
        float offsetForScreenCenter = .25f;
        screenPosition = world.mouseWorldMapPosition - world.camCenter + new Vector2(world.game.GraphicsDevice.Viewport.Width, world.game.GraphicsDevice.Viewport.Height)*offsetForScreenCenter - new Vector2(_texture.Width/2, _texture.Height/2);

        if (world.game._inputManager.IsLeftClick())
        {
            Vector2 worldTileCoords = world.GetTileCoordinates(world.mouseWorldMapPosition);
            if (IsLegalTile(worldTileCoords))
            {
                world.Add(getNewStation(worldTileCoords));
                world.Remove(this);
                // SOUND EFFECT -> Should be a weighty sound effect to *feel* the placement of a workstation.
            }
            else
            {
                Console.WriteLine("That is an illegal tile!");
            }
        }
        
        base.Update(gameTime);
    }
}
