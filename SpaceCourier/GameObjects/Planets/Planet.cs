public abstract class Planet
{
    public string Name { get;}

    public Planet(string name) => Name = name;

    public abstract void OnVisit(Player player);
    public abstract string GetPlanetType();
}