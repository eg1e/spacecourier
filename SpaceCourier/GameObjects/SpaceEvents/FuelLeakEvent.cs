class FuelLeakEvent : ISpaceEvent
{
    public  void Apply(Player player)
    {
        player.SpendFuel(18);
    }
}