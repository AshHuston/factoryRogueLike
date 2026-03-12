using factoryRL.Inputs;
using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects;

public class Player : Meeple
{
    private readonly InputManager input;

    public Player(Game1 _game, World _world, GameAssets gameAssets, Vector2 _worldPosition)
    {
        worldPosition = _worldPosition;
        world = _world;
        input = _game._inputManager;
        _texture = gameAssets.Player;
        mvSpdPx = 5;
        targetWorldPosition = worldPosition;
    }

    public override void Update(GameTime gameTime)
    {
        if (input.isUpPressed())    {targetWorldPosition.Y -= mvSpdPx;}
        if (input.isDownPressed())  {targetWorldPosition.Y += mvSpdPx;}
        if (input.isRightPressed()) {targetWorldPosition.X += mvSpdPx;}
        if (input.isLeftPressed())  {targetWorldPosition.X -= mvSpdPx;}
        StepTowards(targetWorldPosition);
    }
}
