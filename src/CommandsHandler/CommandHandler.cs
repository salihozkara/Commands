
using System.Reflection;
using CommandsHandler.Commands;
using CommandsHandler.Middlewares;
using CommandsHandler.Results;
using CommandsHandler.Utilities;


namespace CommandsHandler;

public class CommandHandler
{
    public CommandHandler(params Assembly[] assemblies)
    {

        var list = assemblies.ToList();
        list.Add(Assembly.GetExecutingAssembly());
        Assemblies = list.ToArray();
    }

    public Assembly[] Assemblies { get; set; }

    public ICommandResult Handle(string[] args)
    {
        var parsedArgs = ArgsParser.Parse(args);
        parsedArgs.CommandHandler = this;
        var middlewareResult = UseOnBeforeMiddlewares(parsedArgs);

        try
        {
            return !middlewareResult.Success ? middlewareResult.CommandResult : Handle(parsedArgs);
        }
        catch (Exception e)
        {
            return new CommandResult(false, e.Message);
        }
    }
    private ICommandResult Handle(Args args)
    {

        var commandHandler = CommandFactory.GetCommand(args.Command, Assemblies);
        //var subCommands = args.SubCommands;
        args.SubCommands = args.Commands.Count > 1 ? args.Commands.GetRange(1, args.Commands.Count - 1) : new List<string>();
        while (true)
        {
            if (args.SubCommands.Count <= 0) return Execute(commandHandler, args);
            var subCommand = args.SubCommands.First();
            try
            {
                commandHandler = CommandFactory.GetSubCommand(commandHandler, subCommand, Assemblies);
                args.Command = subCommand;
                args.SubCommands.Remove(subCommand);
            }
            catch (Exception)
            {
                //Console.WriteLine(e.Message);
                args.SubCommands.Remove(args.SubCommands.Last());
            }
        }
    }
    public bool IsHelpCommand(Args args)
    {
        return args.ArgsDictionary.ContainsKey("help") || args.ArgsDictionary.ContainsKey("h");
    }
    // private ICommandResult UseMiddlewares(Args args,Type type)
    // {
    //     var midelewares = MiddlewareFactory.GetMiddlewares(Assemblies);
    //     return midelewares.Any(mideleware => !mideleware.Execute(args,type).Success) ? new CommandResult(false, "Middleware executed") : new CommandResult(true, "Middleware executed");
    // }
    private IMiddelwareResult UseOnBeforeMiddlewares(Args args)
    {
        var middlewares = MiddlewareFactory.GetMiddlewares(Assemblies);
        var middlewareResult = middlewares.Select(middleware => middleware.OnBeforeExecute(args)).FirstOrDefault(result => !result.Success);
        return middlewareResult ?? new MiddelwareResult(true);
        //return middlewareResult != null ? middlewareResult.CommandResult : new CommandResult(false);
        //return middlewares.Any(middleware => !middleware.OnBeforeExecute(args).Success) ? new CommandResult(false, "Middleware executed") : new CommandResult(true, "Middleware executed");
    }
    private IMiddelwareResult UseOnAfterMiddlewares(Args args, Type type)
    {
        var middlewares = MiddlewareFactory.GetMiddlewares(Assemblies);
        var middlewareResult = middlewares.Select(middleware => middleware.OnAfterExecute(args, type)).FirstOrDefault(result => !result.Success);
        return middlewareResult ?? new MiddelwareResult(true);

        // return middlewares.Any(middleware => !middleware.OnAfterExecute(args,type).Success) ? new CommandResult(false, "Middleware executed") : new CommandResult(true, "Middleware executed");
    }
    private ICommandResult Execute(Type type, Args args)
    {
        var middlewareResult = UseOnAfterMiddlewares(args, type);
        if (!middlewareResult.Success) return middlewareResult.CommandResult;
        if (type.IsAbstract)
            throw new Exception("Command is abstract");
        var instance = (BaseCommand)Activator.CreateInstance(type, args)!;
        return instance.Execute();


    }


}
