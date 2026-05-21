
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace factoryRL.GameObjects;

public class TextMenuOption : IMenuOption
{
    public string Text { get; }
    public SpriteFont Font { get; }
    public Color Color { get; }
    public Action OnClick { get; set; }

    public TextMenuOption(string _text, SpriteFont _font, Color _color, Action _callback)
    {
        Text = _text;
        Font = _font;
        Color = _color;
        OnClick = _callback;
    }
}

public class TextMenu : Menu
{
    public int textHeight = 10;
    public int textPadding = 10;
    public TextMenu(
        World _world,
        GameAssets _assets,
        Rectangle _screenBounds,
        TextMenuOption[] _options,
        int _targetWidth,
        int _targetHeight
    ) 
        : base(_world, _assets, _screenBounds, _options, _targetWidth, _targetHeight) 
    {
        
    }

    public static string GetSubstringThatFits(string text, SpriteFont font, float maxWidth)
    {
        if (string.IsNullOrEmpty(text) || maxWidth <= 0)
            return "";

        for (int length = text.Length; length > 0; length--)
        {
            string substring = text.Substring(0, length);

            if (font.MeasureString(substring).X <= maxWidth)
                return substring;
        }

        return "";
    }

    public override void Update(GameTime gameTime)
    {
        isHovered = screenBounds.Contains(world._inputManager.MouseScreenPosition);
        if (isHovered)
        {
            int relativeY = (int)(world._inputManager.MouseScreenPosition.Y - screenBounds.Y - textPadding);
            int optionIndex = relativeY / (textHeight + textPadding);
            if (optionIndex >= 0 && optionIndex < options.Length)
            {
                hoveredOption = options[optionIndex];
                if (world._inputManager.IsLeftClick())
                {
                    hoveredOption.OnClick();
                }
            }
        }
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        for (int i = 0; i < options.Length; i++)
        {
            TextMenuOption option = (TextMenuOption)options[i];
            Vector2 textPosition = new Vector2(
                screenBounds.X + textPadding,
                screenBounds.Y + textPadding + i * (textHeight + textPadding)
            );
            string textToDraw = GetSubstringThatFits(option.Text, option.Font, screenBounds.Width - 2 * textPadding);
            if (option == hoveredOption)
            {
                Texture2D hoverBackground = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                hoverBackground.SetData(new[] { new Color(128, 128, 128, 128) });
                spriteBatch.Draw(
                    hoverBackground,
                    new Rectangle(
                        screenBounds.X + textPadding / 2,
                        (int)textPosition.Y - textPadding / 2,
                        screenBounds.Width - (3 * textPadding / 2),
                        (int)option.Font.MeasureString(textToDraw).Y + textPadding
                    ),
                    Color.Gray * 0.5f
                );
            }
            spriteBatch.DrawString(option.Font, textToDraw, textPosition, option.Color);
        }
    }
}
