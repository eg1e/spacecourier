class SolarStormEvent : ISpaceEvent
{
    public EventResult Execute(Player player)
    {
        player.SpendFuel(GameConstants.SolarStormDamage);
        return new EventResult("Solar storm occured!", false);
    }
}