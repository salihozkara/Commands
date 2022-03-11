using CommandsHandler.Results;

namespace CommandsHandler.Middlewares;

public class MiddelwareResult:IMiddelwareResult
{
    public MiddelwareResult(bool success)
    {
        Success = success;
    }

    public MiddelwareResult(bool success,ICommandResult commandResult)
    {
        Success = success;
        CommandResult = commandResult;
    }
    public bool Success { get; set; }
    public ICommandResult CommandResult { get; set; }
}
public interface IMiddelwareResult
{
    public bool Success { get; set; }
    public ICommandResult CommandResult { get; set; }
}