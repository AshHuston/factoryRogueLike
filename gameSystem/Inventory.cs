using System;
using System.Collections.Generic;
using factoryRL.GameObjects.Resources;

public class Inventory
{
    private readonly Dictionary<ResourceItemType, int> _items = new();

    public int GetUniqueItemCount()
    {
        return _items.Count;    
    }

    public List<ResourceItemType> GetUniqueItems()
    {
        return [.. _items.Keys];
    }

    public int GetAmount(ResourceItemType type)
    {
        return _items.GetValueOrDefault(type);
    }

    public void Add(ResourceItemType type, int amount = 1)
    {
        _items[type] = GetAmount(type) + amount;
    }

    public int Remove(ResourceItemType type, int amount = 1)
    {
        int current = GetAmount(type);

        int removed = Math.Min(current, amount);

        if (removed == 0)
            return 0;

        int remaining = current - removed;

        if (remaining == 0)
            _items.Remove(type);
        else
            _items[type] = remaining;

        return removed;
    }

    public bool Has(ResourceItemType type, int amount = 1)
    {
        return GetAmount(type) >= amount;
    }

    public void Clone(Inventory source)
{
        _items.Clear();

        foreach (var item in source._items)
        {
            _items[item.Key] = item.Value;
        }
    }
}
