using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using factoryRL.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;
public class StationBuilder : Entity
{
    public StationBuilder(World _world, GameAssets _assets, Type stationType)
    {
        Dictionary<Type, Texture2D> _textures = new()
        {
            { typeof(Mine), _assets.Mine },
            { typeof(LumberMill), _assets.LumberMill },
        };
        world = _world;
        _texture = _textures[stationType];
    }

    public override void Update(GameTime gameTime)
    {
        //_position = world.mouseWorldMapPosition - new Vector2(_texture.Width/2, _texture.Height/2);
        _position = world.mouseWorldMapPosition - world.camCenter;// + new Vector2(world.game.GraphicsDevice.Viewport.Width*world.game.scale / 2, world.game.GraphicsDevice.Viewport.Height / 2);
        // For some reason the sprite does not get drawn in the right place and i am confused why...
        
        base.Update(gameTime);
    }
}
