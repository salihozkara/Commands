using CommandsHandler.Commands.DefaultCommands;
using CommandsHandler.Results;
using CommandsHandler.Utilities;

namespace CommandsHandler.Middlewares.DefaultMiddlewares;

public class Help:BaseMiddleware
{
    public override IMiddelwareResult OnAfterExecute(Args args, Type type)
    {
        if (args.ArgsDictionary.ContainsKey("help") || args.ArgsDictionary.ContainsKey("h"))
        {
            return new MiddelwareResult(false, new HelpCommand(args, type).Execute());
        }

        return new MiddelwareResult((true));
    }
}
