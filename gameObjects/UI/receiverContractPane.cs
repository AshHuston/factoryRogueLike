using System;
using System.Linq.Expressions;
using factoryRL.Functions;
using factoryRL.GameObjects.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;


public class ReceiverContractPanel : Entity
{
    public Rectangle screenBounds;
    public bool isHovered;
    private NineSlicedSprite MenuBackground;
    public int targetWidth;
    public int targetHeight;
    internal int width = 0;
    internal int height = 0;
    private int widthScaleSpeedPixels = 5;
    private int heightScaleSpeedPixels = 5;
    private bool isClosing = false;
    private int removeThreshold = 5;
    private Receiver targetStation;
    public bool isOpen = false;
    int panelPaddingPx;

    public ReceiverContractPanel(
        World _world,
        GameAssets _assets,
        Receiver _targetStation,
        int _targetWidth
    )
    {
        world = _world;
        assets = _assets;
        targetStation = _targetStation;
        targetWidth = _targetWidth;
        MenuBackground = new NineSlicedSprite(assets.MenuBackgroundNS, 4, 4, 12, 12);
    }

    private void removeFromWorld()
    {
        world.Remove(this);

        // Are these useful/needed?
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
        isOpen = true;
        world.Add(this);
    }

    public void close(bool instant=false)
    {
        if (instant) { removeFromWorld(); };
        isClosing = true;
    }

    public override void Update(GameTime gameTime)
    {
        panelPaddingPx = 8;
        int panelItemHeight = 11;
        int uniqueItems = targetStation.currentContract.requirements.Count;
        targetHeight = ((uniqueItems+2)*panelPaddingPx) + ((1+uniqueItems)*panelItemHeight);

        if (!isClosing){
            width = Math.Clamp(
                width + Math.Sign(targetWidth - width) * widthScaleSpeedPixels,
                Math.Min(width, targetWidth),
                Math.Max(width, targetWidth)
            );

            height = Math.Clamp(
                height + Math.Sign(targetHeight - height) * heightScaleSpeedPixels,
                Math.Min(height, targetHeight),
                Math.Max(height, targetHeight)
            );
        }
        else
        {
            width -= widthScaleSpeedPixels; 
            height -= heightScaleSpeedPixels;
            if (width<=removeThreshold || height<=removeThreshold){
                isOpen = false;
                removeFromWorld();
            }
        }

        screenBounds = targetStation.panel.screenBounds;
        screenBounds.Y += targetStation.panel.screenBounds.Height + panelPaddingPx;

        screenBounds.Width = width;
        screenBounds.Height = height;

        MenuBackground.Resize(4, 4, height, width);
        MenuBackground.screenPosition = new Vector2(screenBounds.X, screenBounds.Y);

        isHovered = EntityFunctions.Hovered(screenBounds);

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        MenuBackground.Draw(spriteBatch);
        spriteBatch.DrawString(
            assets.Pixel1Font,
            "Requirements",
            MenuBackground.screenPosition + new Vector2 (panelPaddingPx, panelPaddingPx+4),
            Color.Black
        );

        int i = 0;
        int lineHeight = 14;
        foreach ((ResourceItemType Item, int Quantity) r in targetStation.currentContract.requirements)
        {
            i++;
            spriteBatch.Draw(
                ResourceDatabase.ItemData[r.Item].Texture,
                MenuBackground.screenPosition + new Vector2 (panelPaddingPx, Math.Max(panelPaddingPx, i*(panelPaddingPx+lineHeight))),
                Color.White
            );
            spriteBatch.DrawString(
                assets.Pixel1Font,
                $"x{r.Quantity}",
                MenuBackground.screenPosition + new Vector2((int)(1.5*panelPaddingPx)+ResourceDatabase.ItemData[r.Item].Texture.Width, i*(panelPaddingPx+lineHeight)+4),
                Color.Black
            );
        }
    }
}
