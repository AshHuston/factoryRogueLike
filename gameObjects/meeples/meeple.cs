using System.Collections.Generic;
using Microsoft.Xna.Framework;
using factoryRL.GameObjects.Resources;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace factoryRL.GameObjects;

public class Meeple : Entity
{
    public int mvSpdPx;
    public Vector2 worldPosition;
    public Vector2 targetWorldPosition;
    public List<ResourceType> inventory = new List<ResourceType>();

    public void StepTowards(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - worldPosition;

        if (direction == Vector2.Zero) return;

        direction.Normalize();

        if (Vector2.Distance(worldPosition, targetPosition) <= mvSpdPx)
        {
            worldPosition = targetPosition;
        }else
        {
            worldPosition += direction * mvSpdPx;
        }
    }
}
