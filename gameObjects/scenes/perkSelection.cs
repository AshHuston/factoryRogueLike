using System;
using factoryRL;
using factoryRL.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Collections.Generic;
using factoryRL.perks;
using Microsoft.Xna.Framework.Input;

public class PerkSelectScreen : Scene
{
    public Game1 game;
    private List<KeyValuePair<Perk, (bool isActive, string label)>> perkOptions;
    string[] displayText = [];

    public PerkSelectScreen(Game1 _game, GameAssets _assets, int _completedRound, (int flat, int bonus) gold, List<KeyValuePair<Perk, (bool isActive, string label)>> _perkOptions) : base(_game, _assets)
    {
        NineSlicedSprite bg = new(assets.MenuBackgroundNS, 4, 4, _game.VirtualResolution.height, _game.VirtualResolution.width);
        Add(bg);
        game = _game;
        perkOptions = _perkOptions;

        int interestGold = game.perks.IsActive(Perk.GOLD_GENERATES_INTEREST) ? game.Gold()/10 : 0;
        game.AddGold(interestGold + gold.bonus + gold.flat);

        displayText = [
            $"Congratulations on completing round {_completedRound}!",
            "",
            interestGold>0 ? $"Interest: {interestGold}" : "",
            $"Base gold: {gold.flat}",
            $"Bonus gold: {gold.bonus}",
            "",
            $"Total gold: +{gold.flat+gold.bonus}",
            "",
            "",
            "",
            "SELECT PERK:",
            $"Press 1: {perkOptions[0].Value.label}",
            $"Press 2: {perkOptions[1].Value.label}",
            $"Press 3: {perkOptions[2].Value.label}",
            "",
            "",
            "",
            "",
            "",
            "Press F to open the feedback form in your browser",
        ];
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

    public void addPerkAndMoveOn(int perkZeroIndex)
    {
        game.perks.Activate(perkOptions[perkZeroIndex].Key);
        game.currentRound++;
        World w = game.GoToWorldScreen();
        foreach (Entity e in w.gameEntities)
        {
            if (e is WorkStationPanel p) { p.close(true); }
            if (e is ReceiverContractPanel r) { r.close(true); }
            if (e is Menu m) { m.close(true); }
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (game._inputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F))
        {
            OpenFeedbackForm();
        }


        Keys[] keys =
        [
            Keys.D1,
            Keys.D2,
            Keys.D3
        ];
        for (int i = 0; i < keys.Length; i++)
        {
            if (game._inputManager.IsKeyPressed(keys[i]))
            {
                addPerkAndMoveOn(i);
                break;
            }
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
