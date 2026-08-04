using System;
using System.Collections.Generic;
using System.Linq;

namespace factoryRL.perks;

public enum Perk
{
    INCREASE_WORKER_MINESPEED,
    INCREASE_WORKER_MOVESPEED,
    GOLD_GENERATES_INTEREST,
    INCREASE_ROUND_TIMER_LENGTH,
    DISCOUNT_TIMBERMILL,
    DISCOUNT_MINE,
    DISCOUNT_WORKER,
    INCREASE_CART_FREQUENCY,
    INCREASE_SPEED_TIMBERMILL,
    INCREASE_SPEED_MINE,
    INCREASE_CART_GOLD,
    INCREASE_CART_TIME,
    INCREASE_WORKERS_MINE,
    INCREASE_WORKERS_TIMBERMILL
}

public class PerkManager
{
    public PerkManager() { }

    private Dictionary<Perk, (bool isActive, string label)> perks = new Dictionary<Perk, (bool isActive, string label)>()
    {
        {
            Perk.INCREASE_WORKER_MINESPEED,
            (
                isActive: false,
                label: "+ Worker production efficiency"
            )
        },
        {
            Perk.INCREASE_WORKER_MOVESPEED,
            (
                isActive: false,
                label: "+ Worker movement speed"
            )
        },
        {
            Perk.GOLD_GENERATES_INTEREST,
            (
                isActive: false,
                label: "Gain +1 gold for each 10 you have at the end of each round."
            )
        },
        {
            Perk.INCREASE_ROUND_TIMER_LENGTH,
            (
                isActive: false,
                label: "Round timers are 1 minute longer"
            )
        },
        {
            Perk.DISCOUNT_TIMBERMILL,
            (
                isActive: false,
                label: "Timbermills cost 10gp less"
            )
        },
        {
            Perk.DISCOUNT_MINE,
            (
                isActive: false,
                label: "Mines cost 10gp less"
            )
        },
        {
            Perk.DISCOUNT_WORKER,
            (
                isActive: false,
                label: "Workers cost 5gp less"
            )
        },
        {
            Perk.INCREASE_CART_FREQUENCY,
            (
                isActive: false,
                label: "Carts spawn 2x as often (still only one at a time)"
            )
        },
        {
            Perk.INCREASE_SPEED_TIMBERMILL,
            (
                isActive: false,
                label: "Timbermills are 25% faster"
            )
        },
        {
            Perk.INCREASE_SPEED_MINE,
            (
                isActive: false,
                label: "Mines are 25% faster"
            )
        },
        {
            Perk.INCREASE_CART_GOLD,
            (
                isActive: false,
                label: "Cart contracts payout 2x what they used to"
            )
        },
        {
            Perk.INCREASE_CART_TIME,
            (
                isActive: false,
                label: "Cart contracts give more time to complete"
            )
        },
        {
            Perk.INCREASE_WORKERS_MINE,
            (
                isActive: false,
                label: "Mines gain +1 max workers"
            )
        },
        {
            Perk.INCREASE_WORKERS_TIMBERMILL,
            (
                isActive: false,
                label: "Timbermills gain +1 max workers"
            )
        },
    };


    public void Activate(Perk p)
    {
        var perk = perks[p];
        perk.isActive = true;
        perks[p] = perk;
    }

    public void Deactivate(Perk p)
    {
        var perk = perks[p];
        perk.isActive = false;
        perks[p] = perk;
    }

    public bool IsActive(Perk p)
    {
        return perks[p].isActive;
    }

    public List<KeyValuePair<Perk, (bool isActive, string label)>> GetRandomPerks(int numOfPerks = 3)
    {
        return [.. perks
        .Where(kvp => !kvp.Value.isActive)
        .OrderBy(_ => Random.Shared.Next())
        .Take(numOfPerks)];
    }
}
