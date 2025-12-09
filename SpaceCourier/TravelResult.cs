public class TravelResult
{
    public string Message { get; }
    public bool IsGameOver { get; }
    public bool ReachedDestination { get; }

    public TravelResult(string message, bool isGameOver = false, bool reachedDestination = false)
    {
        Message = message;
        IsGameOver = isGameOver;
        ReachedDestination = reachedDestination;
    }
}
