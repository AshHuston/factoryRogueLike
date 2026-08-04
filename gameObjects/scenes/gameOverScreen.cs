using System;
using factoryRL;
using factoryRL.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

public class GameOverScreen : Scene
{
    public Game1 game;
    String[] displayText = [
        "- - - GAME OVER - - -",
        "You ran out fo time and have lost.",
        "R.I.P.",
        "",
        "Press ENTER to reset and go back to the title screen.",
        "",
        "",
        "Press F to open the feedback form in your browser",
    ];

    public GameOverScreen(Game1 _game, GameAssets _assets) : base(_game, _assets)
    {
        NineSlicedSprite bg = new(assets.MenuBackgroundNS, 4, 4, _game.VirtualResolution.height, _game.VirtualResolution.width);
        Add(bg);
        game = _game;
    }

    private void OpenFeedbackForm()
    {
        string url = "https://docs.google.com/forms/d/e/1FAIpQLSe-EDmdnA57r2se0Rxw7SridX5dWgLBRXm8_S9T7nAe--rigQ/viewform?usp=publish-editor";

        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }

    public override void Update(GameTime gameTime)
    {
        if (game._inputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
        {
            game.ResetGame();
            game.GoToTitleScreen();
        }
        if (game._inputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F))
        {
            OpenFeedbackForm();
        }
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        int lineHeight = 15;
        int i = 0;
        foreach (String line in displayText)
        {
            spriteBatch.DrawString(
                assets.Pixel1Font,
                line.Replace(" ", "    "),
                new Vector2(25, 25 +(lineHeight*i)),
                Color.Black
            );
            i++;
        }
    }
}
