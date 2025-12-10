public class FuelStationPlanet : Planet
{
    private readonly int _refuelAmount = 40;

    public FuelStationPlanet(string name) : base(name) { }

    public override void OnVisit(Player player)
    {
        player.AddFuel(_refuelAmount);
    }

    public override string GetPlanetType()
    {
        return "Fuel station";
    }
}