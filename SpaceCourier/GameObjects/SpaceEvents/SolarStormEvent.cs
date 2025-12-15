class SolarStormEvent : ISpaceEvent
{
    public void Execute(Player player)
    {
        player.SpendFuel(GameConstants.SolarStormDamage);
    }
}