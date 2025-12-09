using System;

public class DangerousPlanet : Planet
{
    private readonly int _dangerLevel;

    public DangerousPlanet(string name, int danger) : base(name)
    {
        _dangerLevel = danger;
    }

    public override void OnVisit(Player player)
    {
        Console.WriteLine("Entering dangerous planet!");
    }
}