using System.Collections.Generic;
using System.Linq;

public class GameEngine
{
    public IReadOnlyList<Planet> Planets { get; }
    public IReadOnlyList<Route> Routes { get; }

    private readonly EventService _eventService;
    private readonly EventResolver _resolver = new();

    public GameEngine()
    {
        var factory = new RandomEventFactory();
        _eventService = new EventService(factory);
        Planets = BuildPlanets();
        Routes = BuildRoutes(Planets);
    }

    public TravelResult TryTravel(Player player, Route route)
    {
        if (!HasEnoughFuel(player, route))
            return new TravelResult("You don't have enough fuel!", isGameOver: true);

        ApplyTravelCost(player, route);

        // Polymorphic event
        var spaceEvent = _eventService.RollEvent(route.Risk);
        string message = "Trip was smooth.";

        if (spaceEvent != null)
        {
            spaceEvent.Apply(player);
            message = spaceEvent.GetType().Name.Replace("Event", " occurred!");
            if (player.Fuel <= 0)
                return new TravelResult(message + " You ran out of fuel!", isGameOver: true);
        }

        player.MoveTo(route.DestinationPlanet);

        if (IsDestination(player))
            return new TravelResult("You delivered the cargo successfully!", isGameOver: true, reachedDestination: true);

        return new TravelResult(message);
    }



    private static bool HasEnoughFuel(Player player, Route route)
        => player.Fuel >= route.FuelCost;

    private static void ApplyTravelCost(Player player, Route route)
        => player.SpendFuel(route.FuelCost);

    private static bool IsDestination(Player player)
        => player.Current.Name == "Destination";

    public IEnumerable<Route> GetRoutesFrom(Planet p)
        => Routes.Where(r => r.CurrentPlanet == p);

    private static List<Planet> BuildPlanets()
    {
        return new()
        {
            new PlanetBuilder().WithName("Earth").Build(),
            new PlanetBuilder().WithName("Alpha").Build(),
            new PlanetBuilder().WithName("Beta").Build(),
            new PlanetBuilder().WithName("Destination").Build(),
            new PlanetBuilder().WithName("Solarion Belt").AsDangerous().Build(),
            new PlanetBuilder().WithName("Zenova Obscura").AsDangerous().Build(),
            new PlanetBuilder().WithName("Relicos Delta").AsDangerous().Build(),
        };
    }

    private static List<Route> BuildRoutes(IReadOnlyList<Planet> p)
    {
        Planet earth = p.First(x => x.Name == "Earth");
        Planet alpha = p.First(x => x.Name == "Alpha");
        Planet beta = p.First(x => x.Name == "Beta");
        Planet dest = p.First(x => x.Name == "Destination");
        Planet solarion = p.First(x => x.Name == "Solarion Belt");
        Planet zenova = p.First(x => x.Name == "Zenova Obscura");
        Planet relicos = p.First(x => x.Name == "Relicos Delta");

        return new()
        {
            new Route(earth, alpha, 15, 20),
            new Route(earth, beta, 10, 80),
            new Route(alpha, dest, 20, 90),
            new Route(beta, solarion, 25, 100),
            new Route(beta, zenova, 5, 50),
            new Route(beta, relicos, 12, 70),
            new Route(solarion, dest, 7, 30),
            new Route(zenova, dest, 8, 40),
            new Route(relicos, dest, 14, 80)
        };
    }
}
