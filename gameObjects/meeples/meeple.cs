using System.Collections.Generic;
using Microsoft.Xna.Framework;
using factoryRL.GameObjects.Resources;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;
using System;

namespace factoryRL.GameObjects;

public class Meeple : Entity
{
    public int mvSpdPx;
    public Vector2 worldPosition;
    public Vector2 targetWorldPosition;
    public List<(ResourceItemType Type, int Amount)> inventory = [];

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

    public void AddToInventory(ResourceItemType type, int amount)
    {
        var existingItem = inventory.Find(item => item.Type == type);
        if (existingItem != default)
        {
            existingItem.Amount += amount;
        }
        else
        {
            inventory.Add((type, amount));
        }
    }

    public (ResourceItemType Type, int Amount) RemoveFromInventory(ResourceItemType type, int amount)
    {
        var existingItem = inventory.Find(item => item.Type == type);
        if (existingItem != default)
        {
            int amountToRemove = Math.Min(existingItem.Amount, amount);
            existingItem.Amount -= amountToRemove;
            if (existingItem.Amount <= 0)
            {
                inventory.Remove(existingItem);
            }
            return (type, amountToRemove);
        }
        return (type, 0);
    }

}
