using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public interface IMenuOption
{
    public Action OnClick { get; set; }
}

public abstract class Menu : Entity
{
    public Rectangle screenBounds;
    public bool isHovered;
    internal IMenuOption[] options;
    public IMenuOption hoveredOption;
    private NineSlicedSprite MenuBackground;
    private int targetWidth;
    private int targetHeight;
    internal int width = 0;
    internal int height = 0;
    private int widthScaleSpeedPixels = 5;
    private int heightScaleSpeedPixels = 7;
    private bool isClosing = false;
    private int removeThreshold = 5;

    public Menu(World _world, GameAssets _assets, Rectangle _screenBounds, 
    IMenuOption[] _options, int _targetWidth, int _targetHeight)
    {
        world = _world;
        assets = _assets;
        screenBounds = _screenBounds;
        options = _options;
        targetWidth = _targetWidth;
        targetHeight = _targetHeight;
        MenuBackground = new NineSlicedSprite(assets.MenuBackgroundNS, 4, 4, 12, 12);
    }

    private void removeFromWorld()
    {
        world.Remove(this);
        width = 0;
        height = 0;
    }

    public void open(bool instant=false)
    {
        if (instant) { 
            width = targetWidth;
            height = targetHeight;    
        };
        isClosing = false;
        world.Add(this);
    }

    public void close(bool instant=false)
    {
        if (instant) { removeFromWorld(); };
        isClosing = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (!isClosing){
            if (width < targetWidth)
            {
                width += widthScaleSpeedPixels;
                if (width > targetWidth) width = targetWidth;
            }
            if (height < targetHeight)
            {            
                height += heightScaleSpeedPixels;
                if (height > targetHeight) height = targetHeight;
            }
        }
        else
        {
            width -= widthScaleSpeedPixels; 
            height -= heightScaleSpeedPixels;
            if (width<=removeThreshold || height<=removeThreshold){
                removeFromWorld();
            }
        }

        screenBounds.Width = width;
        screenBounds.Height = height;

        MenuBackground.Resize(4, 4, height, width);
        MenuBackground.screenPosition = new Vector2(screenBounds.X, screenBounds.Y);

        isHovered = screenBounds.Contains(world._inputManager.MouseScreenPosition);

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        MenuBackground.Draw(spriteBatch);
    }
}
