using System;
using System.Threading.Channels;

public static class EventSystem
{
    public static Random _random = new();

    public static int TriggerEvent(Player player, int risk)
    {
        int roll = _random.Next(risk);
        if (roll < 20)
        {
            int eventType = _random.Next(4);
            return eventType;
        }
        
        return 5;
    }
}