
using System.Data;
using System.Diagnostics;
using factoryRL.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public class Player : Entity
{
    private readonly InputManager input;
    private int mvSpdPx = 5; // Don't make readonly. We will adjust this with a perk.

    public Player(Game1 _game, World _world, GameAssets gameAssets, Vector2 position)
    {
        
        _position = position;
        world = _world;
        input = _game._inputManager;
        _texture = gameAssets.Player;
    }

    public override void Update(GameTime gameTime)
    {
        if (input.isUpPressed())    {_position.Y -= mvSpdPx;}
        if (input.isDownPressed())  {_position.Y += mvSpdPx;}
        if (input.isRightPressed()) {_position.X += mvSpdPx;}
        if (input.isLeftPressed())  {_position.X -= mvSpdPx;}
    }
}