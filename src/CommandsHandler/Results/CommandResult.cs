namespace CommandsHandler.Results;

public class CommandResult : ICommandResult
{
    public CommandResult(bool success, string? message):this(success)
    {
        Message = message;
    }

    public CommandResult(bool success)
    {
        Success = success;
    }

    public bool Success { get; set; }
    public string? Message { get; set; }
    
}