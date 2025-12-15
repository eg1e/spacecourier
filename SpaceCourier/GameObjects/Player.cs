using System;

public class Player
{
    public Planet Current { get; private set; }
    private int _fuel;
    public int Fuel => _fuel;
    public bool IsCargoLost { get; private set; }
    private const int _fuelTankCapacity = GameConstants.FuelTankCapacity;

    public Player(Planet start, int initialFuel)
    {
        Current = start;
        _fuel = Math.Clamp(initialFuel, 0, _fuelTankCapacity);
    }

    public void SpendFuel(int amount)
    {
        _fuel = Math.Max(0, _fuel - amount);
    }
    public void AddFuel(int amount)
    {
        _fuel = Math.Min(_fuel + amount, _fuelTankCapacity);
    }

    public void MoveTo(Planet next)
    {
        Current = next;
    }

    public void LoseCargo()
    {
        IsCargoLost = true;
    }
}