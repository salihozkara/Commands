using CommandsHandler.Results;
using CommandsHandler.Utilities;

namespace CommandsHandler.Commands;

//[Command("[command]")]
// public interface ICommand
// {
//     ICommandResult Execute();
// }



[Command("[command]")]
public abstract class BaseCommand
{
    protected BaseCommand(Args args)
    {
        Args = args;
    }

    protected Args Args { get; set; }
    public abstract ICommandResult Execute();
}