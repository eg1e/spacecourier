using System;

public class RandomEventFactory : EventFactory
{
    private Random _rng = new();

    public override ISpaceEvent CreateEvent()
    {
        int roll = _rng.Next(0, 5);

        return roll switch
        {
            0 => new PirateAttackEvent(),
            1 => new NavigationErrorEvent(),
            2 => new FuelLeakEvent(),
            _ => new SolarStormEvent()
        };
    }
}
