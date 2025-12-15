public class NavigationErrorEvent : ISpaceEvent
{
    public void Execute(Player player)
    {
        player.SpendFuel(GameConstants.NavigationErrorDamage);
    }
}