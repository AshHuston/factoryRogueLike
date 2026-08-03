using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using factoryRL.GameObjects;
namespace factoryRL.Functions;

public static class EntityFunctions
{
    public static bool SpritesCollideRectangles(Vector2 origin1, Texture2D texture1, Vector2 origin2, Texture2D texture2){
        Rectangle rect1 = new(
            (int)MathF.Round(origin1.X),
            (int)MathF.Round(origin1.Y),
            (int)MathF.Round(origin1.X+texture1.Width),
            (int)MathF.Round(origin1.Y+texture1.Height)
        );
        Rectangle rect2 = new(
            (int)MathF.Round(origin2.X),
            (int)MathF.Round(origin2.Y),
            (int)MathF.Round(origin2.X+texture2.Width),
            (int)MathF.Round(origin2.Y+texture2.Height)
        );

        return rect1.Intersects(rect2);
    }

    public static bool EntitiesCollide(Entity e1, Entity e2){
        Vector2 origin1 = e1.WorldPosition;
        Vector2 origin2 = e2.WorldPosition;
        Texture2D texture1 = e1._texture;
        Texture2D texture2 = e2._texture;

        return SpritesCollideRectangles(origin1, texture1, origin2, texture2);
    }

    public static bool Clicked(Rectangle rect, bool rightClick = false){
        MouseState mouseState = Mouse.GetState();
        if ((!rightClick && mouseState.LeftButton == ButtonState.Pressed) || (rightClick && mouseState.RightButton == ButtonState.Pressed))
        {
            return rect.Contains(mouseState.X/2, mouseState.Y/2);
        }
        return false;
    }

    public static bool Clicked(Entity e, bool rightClick = false){
        Vector2 origin = e.screenPosition;
        Texture2D texture = e._texture;

        Rectangle rect = new Rectangle(
            (int)MathF.Round(origin.X),
            (int)MathF.Round(origin.Y),
            texture.Width,
            texture.Height
        );

        return Clicked(rect, rightClick);
    }

    public static bool Hovered(Rectangle rect){
        MouseState mouseState = Mouse.GetState();
        return rect.Contains(mouseState.X/2, mouseState.Y/2);
    }

    public static bool Hovered(Entity e){
        Vector2 origin = e.screenPosition;
        Texture2D texture = e._texture;
        
        Rectangle rect = new Rectangle(
            (int)MathF.Round(origin.X),
            (int)MathF.Round(origin.Y),
            texture.Width,
            texture.Height
        );

        return Hovered(rect);
    }
}
