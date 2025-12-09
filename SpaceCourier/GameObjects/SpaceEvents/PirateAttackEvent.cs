public class PirateAttackEvent : ISpaceEvent
{
    public void Apply(Player player)
    {
        player.SpendFuel(20);
    }
}