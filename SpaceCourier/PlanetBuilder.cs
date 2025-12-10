using System;

public class PlanetBuilder
{
    private string _name;
    private bool _isFuelPlanet;
    private bool _isDangerous;

    public PlanetBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public PlanetBuilder AsDangerous()
    {
        _isDangerous = true;
        return this;
    }

    public PlanetBuilder AsRefuelStationPlanet()
    {
        _isFuelPlanet = true;
        return this;
    }

    public Planet Build()
    {
        if (_isFuelPlanet) return new FuelStationPlanet(_name);
        if (_isDangerous)
        {
            int dangerLevel = Random.Shared.Next(1, 11);
            return new DangerousPlanet(_name, dangerLevel);
        }
        
        return new SafePlanet(_name);
    }
}
