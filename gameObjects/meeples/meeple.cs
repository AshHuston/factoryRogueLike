using System;
using factoryRL.perks;
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

        float perkSpeedMultiplier = 1.5f;
        int stepLength = (int)(this is Worker && world.game.perks.IsActive(Perk.INCREASE_WORKER_MOVESPEED) ? mvSpdPx*perkSpeedMultiplier : mvSpdPx);

        if (Vector2.Distance(WorldPosition, targetPosition) <= stepLength)
        {
            WorldPosition = targetPosition;
        }else
        {
            WorldPosition += direction * stepLength;
        }
    }
}
