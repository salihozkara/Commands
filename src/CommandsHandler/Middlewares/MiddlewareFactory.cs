using System.Reflection;

namespace CommandsHandler.Middlewares;

internal static class MiddlewareFactory
{
    public static IEnumerable<BaseMiddleware> GetMiddlewares(IEnumerable<Assembly> assemblies)
    {
        return (from assembly in assemblies from type in assembly.GetTypes() where type.IsSubclassOf(typeof(BaseMiddleware)) select (BaseMiddleware) Activator.CreateInstance(type)).ToList();
    }
}