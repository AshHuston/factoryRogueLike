using Microsoft.Xna.Framework;

namespace factoryRL.GameObjects;

public class Meeple : Entity
{
    public int mvSpdPx;
    public Vector2 targetWorldPosition;
    public Inventory Inventory { get; } = new();
    internal int interactionRange = 5;
    public Vector2 worldPosition;

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
