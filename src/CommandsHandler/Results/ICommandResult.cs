namespace CommandsHandler.Results;

public interface ICommandResult
{
    bool Success { get; }
    string? Message { get; }
}