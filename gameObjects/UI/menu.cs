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
    private int width;
    private int height;
    private int widthScaleSpeedPixels = 5;
    private int heightScaleSpeedPixels = 5;

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

    public override void Update(GameTime gameTime)
    {
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

        screenBounds.Width = width;
        screenBounds.Height = height;

        MenuBackground.Resize(4, 4, height, width);
        MenuBackground._position = new Vector2(screenBounds.X, screenBounds.Y);

        isHovered = screenBounds.Contains(world._inputManager.MouseScreenPosition);
        if (isHovered)
        {
            // Calculate which option is hovered based on mouse position
        }
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        MenuBackground.Draw(spriteBatch);
    }
}
