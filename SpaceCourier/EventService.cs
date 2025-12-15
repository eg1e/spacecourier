using System;
using Microsoft.Xna.Framework;

public class EventService
{
    private readonly Random _rng = new();
    private readonly EventFactory _factory;

    public EventService(EventFactory factory)
    {
        _factory = factory;
    }

    public ISpaceEvent? RollEvent(int risk)
    {
        int roll = _rng.Next(GameConstants.EventRollMax);
        int EventChancePercent = risk;

        if (roll < EventChancePercent)
            return _factory.CreateEvent();

        return null;
    }
}
