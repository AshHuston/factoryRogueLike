using factoryRL.Inputs;
using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects;

public class Player : Entity
{
    private readonly InputManager input;
    public int mvSpdPx = 5; // Don't make readonly. We will adjust this with a perk.
    public Vector2 worldPosition;

    public Player(Game1 _game, World _world, GameAssets gameAssets, Vector2 _worldPosition)
    {
        
        worldPosition = _worldPosition;
        world = _world;
        input = _game._inputManager;
        _texture = gameAssets.Player;
    }

    public override void Update(GameTime gameTime)
    {
        if (input.isUpPressed())    {worldPosition.Y -= mvSpdPx;}
        if (input.isDownPressed())  {worldPosition.Y += mvSpdPx;}
        if (input.isRightPressed()) {worldPosition.X += mvSpdPx;}
        if (input.isLeftPressed())  {worldPosition.X -= mvSpdPx;}
    }
}
