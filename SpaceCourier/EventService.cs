using System;

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
        int roll = _rng.Next(100);
        int EventChancePercent = risk;

        if (roll < EventChancePercent)
            return _factory.CreateEvent();

        return null;
    }
}
