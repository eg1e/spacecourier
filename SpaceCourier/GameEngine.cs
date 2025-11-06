using System.Collections.Generic;
using System.Linq;

public class GameEngine
{
    public List<Planet> Planets { get; } = [];
    public List<Route> Routes { get; } = [];

    public GameEngine()
    {
        var earth = new Planet("Earth");
        var alpha = new Planet("Alpha");
        var beta = new Planet("Beta");
        var dest = new Planet("Destination");

        Planets.AddRange([earth, alpha, beta, dest]);

        Routes.Add(new Route(earth, alpha, 15, 20));
        Routes.Add(new Route(earth, beta, 10, 80));
        Routes.Add(new Route(alpha, dest, 20, 90));
        Routes.Add(new Route(beta, dest, 25, 70));
    }
    
    
        public IEnumerable<Route> GetRoutesFrom(Planet planet) =>
            Routes.Where(r => r.CurrentPlanet == planet);
}