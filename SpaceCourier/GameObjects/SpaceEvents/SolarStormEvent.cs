class SolarStormEvent : ISpaceEvent
{
    public void Apply(Player player)
    {
        player.SpendFuel(10);
    }
}