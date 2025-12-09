public class NavigationErrorEvent : ISpaceEvent
{
    public void Apply(Player player)
    {
        player.SpendFuel(15);
    }
}