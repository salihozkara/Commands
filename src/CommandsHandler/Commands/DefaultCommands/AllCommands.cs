using System.Reflection;
using CommandsHandler.Results;
using CommandsHandler.Utilities;

namespace CommandsHandler.Commands.DefaultCommands;

[Command("all-commands", "Shows all commands.", "all-commands")]
public class AllCommands:BaseCommand
{
   
    public AllCommands(Args args) : base(args)
    {
    }

    public override ICommandResult Execute()
    {
        var result = new CommandResult(true);
        var commands = Args.CommandHandler.Assemblies.SelectMany(c => c.GetExportedTypes());
        commands = commands.Where(c => c.GetCustomAttributes<CommandAttribute>().Any());
        var commandList = commands.Select(c => c.GetCustomAttributes<CommandAttribute>().First().Usage).ToList();
        // Where(t=>typeof(ICommand).IsAssignableFrom(t) ).Select(t=>t.GetCustomAttributes<CommandAttribute>().First().Usage).ToList();
        result.Message = string.Join("\n", commandList);
        return result;
    }
}