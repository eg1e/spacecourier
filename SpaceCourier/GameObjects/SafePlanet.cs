public class SafePlanet : Planet
{
    public SafePlanet(string name) : base(name) { }

    public override string GetPlanetType()
    {
        return "Safe";
    }

    public override void OnVisit(Player player) { }
}