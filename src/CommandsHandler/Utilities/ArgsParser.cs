namespace CommandsHandler.Utilities;

internal static class ArgsParser
{
    // parse to Args
    public static Args Parse(string[] args)
    {
        var result = new Args
        {
            ArgsArray = args,
        };
        var dictionary = new Dictionary<string, string>();
        var list = new List<string>();
        result.Commands = list;
        result.ArgsDictionary = dictionary;
        var currentArg = "";
        foreach (var arg in args)
        {
            if (arg.StartsWith("-") || arg.StartsWith("--"))
            {
                if (currentArg != "")
                {
                    dictionary.Add(currentArg, "");
                }

                currentArg = arg.StartsWith("--") ? arg[2..] : arg[1..];
            }
            else if (currentArg != "")
            {
                dictionary[currentArg] = arg;
                currentArg = "";
            }
            else
            {
                list.Add(arg);
            }
        }

        if (currentArg != "")
        {
            dictionary.Add(currentArg, "");
        }

        result.Command = list.Count > 0 ? list[0] : result.ArgsArray[0];
        
        return result;
    }
    
}