
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public class BuildMenuOption : IMenuOption
{
    public Texture2D Texture { get; }
    public Color Color { get; }
    public int Cost { get; }

    public Action OnClick { get; set; }

    public BuildMenuOption(
        World world,
        Texture2D texture,
        Color color,
        int cost,
        Action<int> purchaseAction)
    {
        Texture = texture;
        Color = color;
        Cost = cost;

        OnClick = () =>
        {
            if (world.SubtractGold(Cost))
            {
                purchaseAction(Cost);
            }
        };
    }
}

public class BuildMenu : Menu
{
    public int optHeight = 32; //Sprite size
    public int optPadding = 10;

    public BuildMenu(
        World _world,
        GameAssets _assets,
        Rectangle _screenBounds
    ) 
        : base(
            _world,
            _assets,
            _screenBounds,
            [ // menu visual length = 5
                new BuildMenuOption(
                    _world,
                    _assets.Mine,
                    Color.Black,
                    35, 
                    goldCost => _world.Add(
                        new StationBuilder(
                            _world,
                            _assets,
                            typeof(Mine),
                            goldCost))),
                            
                new BuildMenuOption(
                    _world,
                    _assets.TimberYard,
                    Color.Black,
                    25,
                    goldCost => _world.Add(
                        new StationBuilder(
                            _world,
                            _assets,
                            typeof(TimberYard),
                            goldCost))),

                // new BuildMenuOption(
                //     _world,
                //     _assets.Warehouse,
                //     Color.Black,
                //     35,
                //     goldCost => _world.Add(
                //         new StationBuilder(
                //             _world,
                //             _assets,
                //             typeof(Warehouse),
                //             goldCost))),

                new BuildMenuOption(
                    _world,
                    _assets.Worker,
                    Color.Black,
                    15,
                    goldCost => _world.Add(
                        new Worker(
                            _world.game,
                            _world,
                            _assets,
                            _world.camCenter))),
            ],
            120, // option width + padding + padding
            220 // option width + padding*5 + padding
        ) 
    {
    }

    public override void Update(GameTime gameTime)
    {
        isHovered = screenBounds.Contains(world._inputManager.MouseScreenPosition);
        if (isHovered)
        {
            int relativeY = (int)(world._inputManager.MouseScreenPosition.Y - screenBounds.Y - optPadding);
            int optionIndex = relativeY / (optHeight + optPadding);
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

        if (!isClosing){
            int UIpadding = 4; 
            for (int i = 0; i < options.Length; i++)
            {
                BuildMenuOption option = (BuildMenuOption)options[i];
                optHeight = option.Texture.Height;
                Vector2 optPosition = new Vector2(
                    screenBounds.X + optPadding,
                    screenBounds.Y + optPadding + (i * (optHeight + optPadding))
                );
                if (option == hoveredOption)
                {
                    Texture2D hoverBackground = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                    hoverBackground.SetData(new[] { new Color(128, 128, 128, 128) });
                    spriteBatch.Draw(
                        hoverBackground,
                        new Rectangle(
                            screenBounds.X + optPadding / 2,
                            (int)optPosition.Y - optPadding / 2,
                            screenBounds.Width - (3 * optPadding / 2),
                            optHeight + optPadding
                        ),
                        Color.Gray * 0.5f
                    );
                }
                spriteBatch.Draw(
                    option.Texture,
                    optPosition,
                    new Rectangle(
                        0,
                        0,
                        Math.Min(width-optPadding, option.Texture.Width),
                        Math.Min(height-((optPadding*(i+1))+(optHeight*i)),
                        option.Texture.Height)
                    ),
                    Color.White
                );
                spriteBatch.Draw(
                    assets.goldCoin,
                    optPosition + new Vector2(option.Texture.Width+optPadding, assets.goldCoin.Height/3),
                    Color.White
                );
                spriteBatch.DrawString(
                    assets.Pixel1Font,
                    $"-{option.Cost}",
                    optPosition + new Vector2(option.Texture.Width+optPadding+assets.goldCoin.Width+UIpadding, assets.goldCoin.Height/3),
                    Color.Black,
                    0,
                    new Vector2(0,0),
                    2,
                    new SpriteEffects(),
                    1
                );
            }
        }
    }
}
