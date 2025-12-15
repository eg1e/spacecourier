public class PirateAttackEvent : ISpaceEvent
{
    public EventResult Execute(Player player)
    {
        player.LoseCargo();
        return new EventResult("Pirates attacked! Cargo lost!", true);
    }
}