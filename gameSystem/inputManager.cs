using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace factoryRL.Inputs;

public class InputManager
{
    private MouseState _previousMouse;
    private MouseState _currentMouse;

    public void Update()
    {
        _currentMouse = Mouse.GetState();
    }

    public bool IsLeftClick()
    {
        return _currentMouse.LeftButton == ButtonState.Pressed &&
                _previousMouse.LeftButton == ButtonState.Released;
    }

    public bool IsRightClick()
    {
        return _currentMouse.RightButton == ButtonState.Pressed &&
               _previousMouse.RightButton == ButtonState.Released;
    }

    public bool isUpPressed()
    {
        return Keyboard.GetState().IsKeyDown(Keys.Up);
    }

    public bool isDownPressed()
    {
        return Keyboard.GetState().IsKeyDown(Keys.Down);
    }

    public bool isLeftPressed()
    {
        return Keyboard.GetState().IsKeyDown(Keys.Left);
    }

    public bool isRightPressed()
    {
        return Keyboard.GetState().IsKeyDown(Keys.Right);
    }

    public void EndUpdate()
    {
        _previousMouse = _currentMouse;
    }
}
