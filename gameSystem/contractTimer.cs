using System;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using factoryRL.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.contract;

public enum TimerDisplayType
{
    Digital,
    Wheel
}

public class ContractTimer: Entity
{
    public int totalSeconds;
    public float remainingTimeSeconds;
    public TimerDisplayType displayType;
    public Contract targetContract;
    private ProgressSprite Wheel;
    private bool IsCountingDown = false;
    public bool show = false;

    public ContractTimer(
        World _world,
        GameAssets _assets,
        Contract _targetContract,
        int _totalSeconds,
        TimerDisplayType _displayType
    )
    {
        isUIElement = displayType == TimerDisplayType.Digital;
        world = _world;
        assets = _assets;
        targetContract = _targetContract;
        totalSeconds =_totalSeconds;
        remainingTimeSeconds = totalSeconds;
        displayType = _displayType;
        Wheel = displayType == TimerDisplayType.Wheel
            ? new ProgressSprite(assets.ProgressWheel, 32, 32) // Framewidth of the progress wheel. Bad magic number...
            : null;
    }

    public void Start(bool showTimer = true)
    {
        IsCountingDown = true;
        show = showTimer;
    }

    public void Stop(bool showTimer = false)
    {
        IsCountingDown = false;
        show = showTimer;
    }

    public bool HasStarted()
    {
        return remainingTimeSeconds < totalSeconds;
    }

    public void SetWheelScreenPosition(Vector2 pos)
    {
        if (Wheel != null){
            Wheel.screenPosition = pos;
        }
    }

    public static string FormatTime(float seconds)
    {
        int totalSeconds = Math.Max(0, (int)Math.Ceiling(seconds));

        int minutes = totalSeconds / 60;
        int remainingSeconds = totalSeconds % 60;

        return $"{minutes:D2}:{remainingSeconds:D2}";
    }

    public override void Update(GameTime gameTime)
    {
        if (IsCountingDown){
            remainingTimeSeconds -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (remainingTimeSeconds <= 0)
            {
                remainingTimeSeconds = 0;
                targetContract.FailContract();
            }

            Wheel?.SetProgress(remainingTimeSeconds / totalSeconds);

            int displaySeconds = (int)Math.Ceiling(remainingTimeSeconds);
        }
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if(show) {
            if (Wheel != null)
            {
                Wheel.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.DrawString(
                    assets.Pixel1Font,
                    FormatTime(remainingTimeSeconds),
                    new Vector2((world.game.VirtualResolution.width/2)-15 , 10),
                    Color.Black,
                    0,
                    new Vector2(0,0),
                    2,
                    new(),
                    1
                );   
            }
        }
    }
}
