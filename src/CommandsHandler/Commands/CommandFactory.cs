using System.Reflection;
using System.Text.RegularExpressions;

namespace CommandsHandler.Commands;

internal static class CommandFactory
{
    private const string DefaultCommand = "[command]";
    private const string DefaultCommandReplacement = "Command";
    public static Type GetCommand(string command, params Assembly[] assemblies)
    {
        //var commandType = GetCommandType(command, assemblies);
        var commandType = GetSubCommandType(typeof(BaseCommand),command, assemblies);
        if (commandType == null)
        {
            throw new ArgumentException($"Command {command} not found!");
        }
        return commandType;
    }
    public static Type GetSubCommand(Type commandType, string subCommand, IEnumerable<Assembly> assemblies)
    {
        var subCommandType = GetSubCommandType(commandType, subCommand, assemblies);
        if (subCommandType == null)
        {
            throw new ArgumentException($"SubCommand {subCommand} not found!");
        }
        return subCommandType;
    }

    private static Type? GetSubCommandType(Type getType, string subCommand, IEnumerable<Assembly> assemblies)
    {
        var subCommandType = assemblies
            .SelectMany(a => a?.GetExportedTypes() ?? throw new InvalidOperationException())
            .SingleOrDefault(t => t.BaseType != null && t.BaseType == getType &&
                                  SubCommandEquals(t, subCommand));
        return subCommandType;
    }
    // private static bool CommandEquals(MemberInfo type, string command)
    // {
    //     var attribute = type.GetCustomAttributes<CommandAttribute>().FirstOrDefault();
    //     var command2 = attribute!.Command == DefaultCommand
    //         ? type.Name.Replace(DefaultCommandReplacement, "").ToLower()
    //         : attribute.Command!;
    //     return command2.Equals(command, StringComparison.OrdinalIgnoreCase);
    // }

    private static bool SubCommandEquals(Type type, string command)
    {
        var attribute = type.GetCustomAttributes<CommandAttribute>().FirstOrDefault()!;
        if (attribute.Command != DefaultCommand)
            if (attribute.Command.Equals(command, StringComparison.OrdinalIgnoreCase))
                return true;
        var attributives = type.GetCustomAttributes<CommandAttribute>().ToList();
        attributives.Remove(attributives.Last());
        var baseTypes = new Stack<Type>();
        var baseCommands = new List<string>();
        var baseType = type.BaseType!;
        while (baseType != typeof(BaseCommand))
        {
            baseTypes.Push(baseType);
            baseType = baseType.BaseType!;
        }

        while (baseTypes.Count > 0)
        {
            baseType = baseTypes.Pop();
            foreach (var commandAttribute in baseType.GetCustomAttributes<CommandAttribute>())
            {
                attributives.Remove(commandAttribute);
            }

            var baseCommand = baseType.Name.SplitCamelCaseToString().Replace(DefaultCommandReplacement, "").Trim()
                .ToLower();
            var lastCommand = baseCommands.LastOrDefault() ?? string.Empty;
            baseCommand = string.IsNullOrEmpty(lastCommand) ? baseCommand : baseCommand.Replace(lastCommand, "");
            baseCommands.Add(baseCommand.Trim());
        }

        if (attributives.Count == 0)
            baseCommands.Add(command);
        else
        {
            foreach (var c in attributives.Select(a => a.Command))
            {
                if (c == DefaultCommand)
                {
                    baseCommands.Add(command);
                }

                if (c.Equals(command, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }

        var command2 = string.Join(" ", baseCommands);
        var name = type.Name.SplitCamelCaseToString().Replace(DefaultCommandReplacement, "").Trim().ToLower();
        return name.Equals(command2, StringComparison.OrdinalIgnoreCase);
    }

   

    private static string[] SplitCamelCase(string str)
    {
        return Regex.Split(str, @"(?<!^)(?=[A-Z])");
    }

    private static string SplitCamelCaseToString(this string str)
    {
        var split = SplitCamelCase(str);
        return string.Join(" ", split);
    }
}