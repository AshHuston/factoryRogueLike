using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public class ProgressSprite : AnimatedSprite
{
    private float progress = 0f;

    public ProgressSprite(Texture2D spriteSheet, int frameWidth, int frameHeight)
     : base()
    {
        _texture = spriteSheet;
        animationFrames = GetFrames(spriteSheet, frameWidth, frameHeight);
        currentFrameIndex = 0;
        isUIElement = true;
    }

    public void SetProgress(float newProgress)
    {
        progress = MathHelper.Clamp(newProgress, 0f, 1f);
        currentFrameIndex = (int)(progress * animationFrames.Length) % animationFrames.Length;
    }

    public float GetProgress()
    {
        return progress;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_texture != null && animationFrames.Length > 0)
        {
            spriteBatch.Draw(
                _texture,
                screenPosition,
                animationFrames[currentFrameIndex],
                Color.White
            );
        }
    }
}
