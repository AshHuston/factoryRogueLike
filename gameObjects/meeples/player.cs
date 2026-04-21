using System;
using factoryRL.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public class Player : Meeple
{
    private readonly InputManager input;
    private readonly Texture2D indicatorTexture;
    private Viewport viewport;
    private readonly int interactionRange = 15;

    public Player(Game1 _game, World _world, GameAssets gameAssets, Vector2 _worldPosition)
    {
        worldPosition = _worldPosition;
        world = _world;
        input = _game._inputManager;
        viewport = _game.GraphicsDevice.Viewport;
        _texture = gameAssets.Player;
        indicatorTexture = gameAssets.MovmentIndicicator;
        mvSpdPx = 5;
        targetWorldPosition = worldPosition;
    }

    private void interact(Vector2 interactionMapPosition)
    {
        var (X, Y) = world.GetTileCoordinates(new Vector2(interactionMapPosition.X, (int)interactionMapPosition.Y));
        try
        {
            world.map[(int)X, (int)Y].Interact(this);
        }
        catch (Exception)
        {
            return;
        }
    }

    public override void Update(GameTime gameTime)
    {
        StepTowards(targetWorldPosition);

        if (input.IsLeftClick()) { 
            targetWorldPosition = new Vector2(
                world.mouseWorldMapPosition.X - _texture.Width/2,
                world.mouseWorldMapPosition.Y - _texture.Height/2
            );

            if (Vector2.Distance(worldPosition, targetWorldPosition) <= interactionRange)
            {
                interact(world.mouseWorldMapPosition);
            }
        }

        if (input.IsRightClick(true)||input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
        { 
            targetWorldPosition = worldPosition;
        } 
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Vector2.Distance(worldPosition, targetWorldPosition) > interactionRange)
        {
            spriteBatch.Draw(
                indicatorTexture,
                targetWorldPosition - world.camCenter + new Vector2(viewport.Width / 2, viewport.Height / 2) + new Vector2(_texture.Width/2, _texture.Height/2),
                null,
                Color.White,
                0f,
                new Vector2(indicatorTexture.Width / 2, indicatorTexture.Height / 2),
                1f,
                SpriteEffects.None,
                0f
            );
        }
        base.Draw(spriteBatch);
    }
}
