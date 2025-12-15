public class PirateAttackEvent : ISpaceEvent
{
    public void Execute(Player player)
    {
        player.LoseCargo();
    }
}