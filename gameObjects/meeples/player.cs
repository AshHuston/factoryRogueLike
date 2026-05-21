using System;
using factoryRL.GameObjects.Terrain;
using factoryRL.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace factoryRL.GameObjects;

public class Player : Meeple
{
    private readonly InputManager input;
    private readonly Texture2D indicatorTexture;
    private Viewport viewport;
    private readonly int interactionRange = 15;
    private Entity entityInteractingWith = null;
    private int harvestTimeRemainingMiliseconds;
    private Game1 game;

    //private NineSlicedSprite test;

    public Player(Game1 _game, World _world, GameAssets gameAssets, Vector2 _worldPosition)
    {
        game = _game;
        worldPosition = _worldPosition;
        world = _world;
        input = _game._inputManager;
        viewport = _game.GraphicsDevice.Viewport;
        _texture = gameAssets.Player;
        indicatorTexture = gameAssets.MovmentIndicicator;
        mvSpdPx = 5;
        targetWorldPosition = worldPosition;

        // Texture2D menuTexture = gameAssets.MenuBackgroundNS;
        // test = new NineSlicedSprite(menuTexture, 4, 4, 70, 70);
        // test._position = new Vector2(25, 25);
    }

    private void interact(Vector2 interactionMapPosition)
    {
        var (X, Y) = world.GetTileCoordinates(new Vector2(interactionMapPosition.X, (int)interactionMapPosition.Y));
        try
        {
            world.map[(int)X, (int)Y].Interact(this);
            Console.WriteLine($"Interacted with tile at ({X}, {Y})");
        }
        catch (Exception)
        {
            return;
        }
    }

    public void startHarvesting(HarvestableTerrain terrain)
    {
        entityInteractingWith = terrain;
        harvestTimeRemainingMiliseconds = terrain.terrainData.MiningTimeMiliseconds;
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

        if (input.IsLeftClickReleased() && entityInteractingWith != null)
        {
            entityInteractingWith = null;
            harvestTimeRemainingMiliseconds = 0;
        }
    
        if (entityInteractingWith is HarvestableTerrain terrain && Vector2.Distance(worldPosition, targetWorldPosition) <= interactionRange)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            harvestTimeRemainingMiliseconds -= (int)deltaTime;
            terrain.harvestProgressWheel.SetProgress(1 - (float)harvestTimeRemainingMiliseconds / terrain.terrainData.MiningTimeMiliseconds);
            terrain.harvestProgressWheel._position = new(Mouse.GetState().X/game.scale, Mouse.GetState().Y/game.scale);

            if (harvestTimeRemainingMiliseconds <= 0)
            {
                var (type, amount) = terrain.HarvestResource();
                AddToInventory(type, amount);
                harvestTimeRemainingMiliseconds = terrain.terrainData.MiningTimeMiliseconds;
                Console.WriteLine(inventory.Count);
            }   
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        //test.Draw(spriteBatch);

        if (Vector2.Distance(worldPosition, targetWorldPosition) > interactionRange)
        {
            spriteBatch.Draw(
                indicatorTexture,
                targetWorldPosition - world.camCenter + new Vector2(game.VirtualResolution.width/2, game.VirtualResolution.height/2) + new Vector2(_texture.Width/2, _texture.Height/2),
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
