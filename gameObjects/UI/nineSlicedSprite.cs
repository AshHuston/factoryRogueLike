using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factoryRL.GameObjects;

public class NineSlicedSprite : Entity
{
    public Rectangle[] slicedSegments;
    private int[][] gridPositions;
    private int[][] gridDimensions;

    public NineSlicedSprite(Texture2D sprite, int segmentWidthPixels, int segmentHeightPixels, int targetHeightPixels, int targetWidthPixels)
    {
        _texture = sprite;
        Resize(segmentWidthPixels, segmentHeightPixels, targetHeightPixels, targetWidthPixels);
    }

    public void Resize(int segmentWidthPixels, int segmentHeightPixels, int targetHeightPixels, int targetWidthPixels)
    {
        slicedSegments = GetSegments(_texture, segmentWidthPixels, segmentHeightPixels);
        
        gridPositions = [
            [0, 0], // Top-left corner
            [segmentWidthPixels, 0], // Top edge
            [targetWidthPixels - segmentWidthPixels, 0], // Top-right corner
            [0, segmentHeightPixels], // Left edge
            [segmentWidthPixels, segmentHeightPixels], // Center
            [targetWidthPixels - segmentWidthPixels, segmentHeightPixels], // Right edge
            [0, targetHeightPixels - segmentHeightPixels], // Bottom-left corner
            [segmentWidthPixels, targetHeightPixels - segmentHeightPixels], // Bottom edge
            [targetWidthPixels - segmentWidthPixels, targetHeightPixels - segmentHeightPixels] // Bottom-right corner
        ];

        gridDimensions = [
            [segmentWidthPixels, segmentWidthPixels],
            [targetWidthPixels - 2 * segmentWidthPixels, segmentHeightPixels],
            [segmentWidthPixels, segmentWidthPixels],
            [segmentWidthPixels, targetHeightPixels - 2 * segmentHeightPixels],
            [targetWidthPixels - 2 * segmentWidthPixels, targetHeightPixels - 2 * segmentHeightPixels],
            [segmentHeightPixels, targetHeightPixels - 2 * segmentHeightPixels],
            [segmentWidthPixels, segmentWidthPixels],
            [targetWidthPixels - 2 * segmentWidthPixels, segmentHeightPixels],
            [segmentWidthPixels, segmentWidthPixels]
        ];
    }

    internal Rectangle[] GetSegments(Texture2D sprite, int segmentWidth, int segmentHeight)
    {
        int columns = sprite.Width / segmentWidth;
        int rows = sprite.Height / segmentHeight;

        Rectangle[] segments = new Rectangle[columns * rows];

        int index = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                segments[index++] = new Rectangle(
                    x * segmentWidth,
                    y * segmentHeight,
                    segmentWidth,
                    segmentHeight
                );
            }
        }

        return segments;
    } 

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < 9; i++){
            Rectangle sourceRect = slicedSegments[i];
            Rectangle destRect = new Rectangle(
                gridPositions[i][0] + (int)_position.X,
                gridPositions[i][1] + (int)_position.Y,
                gridDimensions[i][0],
                gridDimensions[i][1]
            );

            spriteBatch.Draw(_texture, destRect, sourceRect, Color.White);
        }
    }
}
