namespace CommandsHandler.Commands;

[AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface, AllowMultiple = true)]
public class CommandAttribute : Attribute
{
    public string Command { get; set; }
    public string Description { get; set; }
    public string Usage { get; set; }
    public CommandAttribute(string command, string description="", string usage="")
    {
        Command = command;
        Description = description;
        Usage = usage;
    }
}