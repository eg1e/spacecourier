public class NavigationErrorEvent : ISpaceEvent
{
    public EventResult Execute(Player player)
    {
        player.SpendFuel(GameConstants.NavigationErrorDamage);
        return new EventResult("Navigation error!", false);
    }
}