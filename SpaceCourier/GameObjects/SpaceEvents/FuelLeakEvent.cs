class FuelLeakEvent : ISpaceEvent
{
    public  void Execute(Player player)
    {
        player.SpendFuel(GameConstants.FuelLeakDamage);
    }
}