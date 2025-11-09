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
        var solarion = new Planet("Solarion Belt");
        var zenova = new Planet("Zenova Obscura");
        var relicos = new Planet("Relicos Delta");

        Planets.AddRange([earth, alpha, beta, dest]);

        Routes.Add(new Route(earth, alpha, 15, 20));
        Routes.Add(new Route(earth, beta, 10, 80));
        Routes.Add(new Route(alpha, dest, 20, 90));
        Routes.Add(new Route(beta, solarion, 25, 100));
        Routes.Add(new Route(beta, zenova, 5, 50));
        Routes.Add(new Route(beta, relicos, 12, 70));
        Routes.Add(new Route(solarion, dest, 7, 30));
        Routes.Add(new Route(zenova, dest, 8, 40));
        Routes.Add(new Route(relicos, dest, 14, 80));
    }
    
    
        public IEnumerable<Route> GetRoutesFrom(Planet planet) =>
            Routes.Where(r => r.CurrentPlanet == planet);
}