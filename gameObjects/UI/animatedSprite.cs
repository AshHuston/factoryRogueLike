using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public class AnimatedSprite : Entity
{
    public Rectangle[] animationFrames;
    public int currentFrameIndex;

    public AnimatedSprite(Texture2D spriteSheet, int frameWidth, int frameHeight)
     : base()
    {
        animationFrames = GetFrames(spriteSheet, frameWidth, frameHeight);
        currentFrameIndex = 0;
    }

    protected AnimatedSprite()
    {
    }

    internal Rectangle[] GetFrames(Texture2D spriteSheet, int frameWidth, int frameHeight)
    {
        int columns = spriteSheet.Width / frameWidth;
        int rows = spriteSheet.Height / frameHeight;

        Rectangle[] frames = new Rectangle[columns * rows];

        int index = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                frames[index++] = new Rectangle(
                    x * frameWidth,
                    y * frameHeight,
                    frameWidth,
                    frameHeight
                );
            }
        }

        return frames;
    }

    public override void Update(GameTime gameTime)
    {
        currentFrameIndex = (currentFrameIndex + 1) % animationFrames.Length;
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

    public void Draw(SpriteBatch spriteBatch, int frame)
    {
        if (_texture != null && animationFrames.Length > 0)
        {
            spriteBatch.Draw(
                _texture,
                screenPosition,
                animationFrames[frame],
                Color.White
            );
        }
    }
}
