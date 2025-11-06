public class Player
{
    public Planet Current { get; set; }
    public int Fuel { get; set; }
    public bool IsCargoLost { get; set; }

    public Player(Planet start, int initialFuel)
    {
        Current = start;
        Fuel = initialFuel;
    }
}