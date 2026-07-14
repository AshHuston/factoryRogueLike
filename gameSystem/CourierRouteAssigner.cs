using System;
using System.Linq;
using System.Runtime.CompilerServices;
using factoryRL.courierRoute;
using factoryRL.GameObjects;
using factoryRL.GameObjects.Resources;
using factoryRL.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factoryRL.Functions;
using System.Data.Common;

public class CourierRouteAssigner : Entity
{
    private InputManager InputManager;
    private CourierRoute RepresentedRoute = new CourierRoute(null, null, ResourceItemType.None); 
    private bool DrawTheLine = false;
    private Point HoveredTile = new Point(0,0);

    public CourierRouteAssigner(World _world, GameAssets _assets)
    {
        world = _world;
        assets = _assets;
        InputManager = world.game._inputManager;
    }
    // This object needs to: watch for click-drags, draw the line, when released check if its a valid route, if it is, find the nearest eligible worker, and assign them the route.

    private void ResetRepresentedRoute()
    {
        RepresentedRoute.Source = null;
        RepresentedRoute.Target = null;
        RepresentedRoute.ResourceType = ResourceItemType.None;
    }

    public override void Update(GameTime gameTime)
    {
        Vector2 mousePos = world.GetTileCoordinates(InputManager.MouseWorldPosition);
        HoveredTile = new Point((int)mousePos.X, (int)mousePos.Y);
        if (RepresentedRoute.Source != null)
        {
            DrawTheLine = true;
        }

        if (InputManager.IsLeftClick())
        {   
            WorkStation clickedStation = world.gameEntities.OfType<WorkStation>().FirstOrDefault(s => 
                EntityFunctions.Clicked(
                    new Rectangle(
                        (int)(world.GetTileCoordinates(s._position).X*world.tileSizePixels)-s._texture.Width/2,
                        (int)(world.GetTileCoordinates(s._position).Y*world.tileSizePixels)-s._texture.Height/2,
                        s._texture.Width*2,
                        s._texture.Height*2 //SCALE All these last *2 are for the // SCALE
                    )
                )
            );

            if(clickedStation != null)
            {
                RepresentedRoute.Source = clickedStation;
            }
            else
            {
                return;
            }
        }

        if (InputManager.IsLeftClickReleased())
        {
            if (world.gameEntities.OfType<WorkStation>().FirstOrDefault(s => 
                EntityFunctions.Clicked(
                    new Rectangle(
                        (int)(world.GetTileCoordinates(s._position).X*world.tileSizePixels)-s._texture.Width/2,
                        (int)(world.GetTileCoordinates(s._position).Y*world.tileSizePixels)-s._texture.Height/2,
                        s._texture.Width*2,
                        s._texture.Height*2 //SCALE All these last *2 are for the // SCALE
                    )
                )
            ) is WorkStation hoveredStation)
            {
                if (RepresentedRoute.Source != null &&
                    RepresentedRoute.Source != hoveredStation
                 ){
                    RepresentedRoute.Target = hoveredStation;
                    Worker isNowCourier = world.FindClosestEntity<Worker>(InputManager.MouseWorldPosition, w => { return w.IsIdle(); });
                    if (isNowCourier != null){
                        isNowCourier.AssignRoute(RepresentedRoute);
                    }
                    else
                    {
                        // Play sound effect or something, i dunno.
                        Console.WriteLine("You dont have any unassigend guys bro");
                    }
                }
            }
            DrawTheLine = false;
            ResetRepresentedRoute();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (DrawTheLine)
        {
            world.game.DrawLine(
                spriteBatch,
                world.GetTileCoordinates(RepresentedRoute.Source._position*world.tileSizePixels) + new Vector2(world.tileSizePixels/2, world.tileSizePixels/2),
                world.GetContainingTileScreenCoordinates(world.mouseWorldMapPosition),
                Color.Black,
                2f
            );
        }
    }
}
