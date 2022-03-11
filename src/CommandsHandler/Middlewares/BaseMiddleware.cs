using CommandsHandler.Utilities;

namespace CommandsHandler.Middlewares;

public abstract class BaseMiddleware
{
    public virtual IMiddelwareResult OnBeforeExecute(Args args)
    {
        return new MiddelwareResult(true);
    }
    public virtual IMiddelwareResult OnAfterExecute(Args args,Type type)
    {
        return new MiddelwareResult(true);
    }
}