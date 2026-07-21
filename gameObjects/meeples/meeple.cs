using System;
using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects;

public class Meeple : Entity
{
    public int mvSpdPx;
    public Vector2 targetWorldPosition;
    public Inventory Inventory { get; } = new();
    internal int interactionRange = 5;

    public void StepTowards(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - WorldPosition;

        if (direction == Vector2.Zero) return;

        direction.Normalize();

        if (Vector2.Distance(WorldPosition, targetPosition) <= mvSpdPx)
        {
            WorldPosition = targetPosition;
        }else
        {
            WorldPosition += direction * mvSpdPx;
        }
    }
}
