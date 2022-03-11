using System.Reflection;
using CommandsHandler.Results;
using CommandsHandler.Utilities;

namespace CommandsHandler.Commands.DefaultCommands;

[Command("help", "Shows help for a command or all commands if no command is specified.", "[command] --h or --help")]
public class HelpCommand:BaseCommand
{
    private Type CommandType { get; }
    public HelpCommand(Args args,Type command) : base(args)
    {
        CommandType = command;
    }

    public override ICommandResult Execute()
    {
        var result = new CommandResult(true);
        var attribute=CommandType.GetCustomAttributes<CommandAttribute>().First();
        result.Message = $"Desc {attribute.Description}\nUsage {attribute.Usage}";
        return result;
    }
}