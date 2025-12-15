using System;

public class DangerousPlanet : Planet
{
    private readonly int _dangerLevel;
    private readonly int _fuelPricePerDangerLevel = GameConstants.FuelPricePerDangerLevel;

    public DangerousPlanet(string name, int danger) : base(name)
    {
        _dangerLevel = danger;
    }

    public override string GetPlanetType()
    {
        return "Danger " + _dangerLevel.ToString();
    }

    public override void OnVisit(Player player)
    {
        player.SpendFuel(_dangerLevel * _fuelPricePerDangerLevel);
    }
}