using CommandsHandler.Commands.DefaultCommands;
using CommandsHandler.Results;
using CommandsHandler.Utilities;

namespace CommandsHandler.Middlewares.DefaultMiddlewares;

public class AllCommend:BaseMiddleware
{

    public override IMiddelwareResult OnAfterExecute(Args args, Type type)
    {
        if (!args.ArgsDictionary.ContainsKey("all")) return new MiddelwareResult(true);
        var allCommand = new AllCommands(args);
        return new MiddelwareResult(false, allCommand.Execute());
    }
}