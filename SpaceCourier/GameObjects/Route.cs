public class Route
{
    public Planet CurrentPlanet { get; }
    public Planet DestinationPlanet { get; }
    public int FuelCost { get; }
    public int Risk { get; }

    public Route(Planet from, Planet to, int fuelCost, int risk)
    {
        CurrentPlanet = from;
        DestinationPlanet = to;
        FuelCost = fuelCost;
        Risk = risk;
    }
}