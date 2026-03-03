using Microsoft.Xna.Framework.Input;
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

    public void EndUpdate()
    {
        _previousMouse = _currentMouse;
    }
}
