class FuelLeakEvent : ISpaceEvent
{
    public EventResult Execute(Player player)
    {
        player.SpendFuel(GameConstants.FuelLeakDamage);
        return new EventResult("Fuel leak!", false);
    }
}