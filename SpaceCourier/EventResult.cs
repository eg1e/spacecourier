public class EventResult
{
    public string Message { get; }
    public bool IsGameOver { get; }

    public EventResult(string message, bool isGameOver = false)
    {
        Message = message;
        IsGameOver = isGameOver;
    }
}
