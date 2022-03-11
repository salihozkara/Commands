namespace CommandsHandler.Utilities;

public class Args
{
    public CommandHandler? CommandHandler { get; set; }
    public string[] ArgsArray { get; init; } = null!;
    public List<string>? SubCommands { get; set; } 
    public string Command { get; set; } = null!;

    public string? CommandArg { get; set; } 

    // commands
    public List<string> Commands { get; set; } = null!;
    public Dictionary<string, string> ArgsDictionary { get; set; } = null!;

    internal Args()
    {
        
    }

    // internal Args(string[] argsArray, List<string> subCommands, string command, string commandArg, List<string> commands, Dictionary<string, string> argsDictionary)
    // {
    //     ArgsArray = argsArray;
    //     SubCommands = subCommands;
    //     Command = command;
    //     CommandArg = commandArg;
    //     Commands = commands;
    //     ArgsDictionary = argsDictionary;
    // }
}