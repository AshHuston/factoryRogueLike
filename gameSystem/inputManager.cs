using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.ComponentModel.Design;
using System.Diagnostics;
namespace factoryRL.Inputs;

public class InputManager(Game1 _game)
{
    private MouseState _previousMouse;
    private MouseState _currentMouse;
    public Vector2 MouseWorldPosition;
    public Vector2 MouseScreenPosition;
    private readonly Game1 game = _game;

    public void Update()
    {
        _currentMouse = Mouse.GetState();
        MouseScreenPosition = new Vector2(_currentMouse.X, _currentMouse.Y);
        if (game.currentScene is GameObjects.World currentWorld)
        {
            MouseWorldPosition = currentWorld.camCenter + MouseScreenPosition - new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
        }
        else
        {
            MouseWorldPosition = MouseScreenPosition;
        }
    }

    public bool IsLeftClick(bool checkHeld = false){
        if(checkHeld) {return _currentMouse.LeftButton == ButtonState.Pressed; }
        return _currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released;
    }

    public bool IsRightClick(bool checkHeld = false)
    {
        if(checkHeld) {return _currentMouse.RightButton == ButtonState.Pressed; }
        return _currentMouse.RightButton == ButtonState.Pressed && _previousMouse.RightButton == ButtonState.Released;
    }

    public bool IsKeyPressed(Keys key)
    {
        return Keyboard.GetState().IsKeyDown(key);
    }

    // public bool isUpPressed()
    // {
    //     return Keyboard.GetState().IsKeyDown(Keys.Up);
    // }

    // public bool isDownPressed()
    // {
    //     return Keyboard.GetState().IsKeyDown(Keys.Down);
    // }

    // public bool isLeftPressed()
    // {
    //     return Keyboard.GetState().IsKeyDown(Keys.Left);
    // }

    // public bool isRightPressed()
    // {
    //     return Keyboard.GetState().IsKeyDown(Keys.Right);
    // }

    // public bool isTPressed()
    // {
    //     return Keyboard.GetState().IsKeyDown(Keys.T);
    // }

    // public bool isGPressed()
    // {
    //     return Keyboard.GetState().IsKeyDown(Keys.G);
    // }

    public void EndUpdate()
    {
        _previousMouse = _currentMouse;
    }
}
