using System.Reflection;

namespace IdentityHub.BFF.Extensions.ServiceCollections
{
    public static class EndpointsExtension
    {
        public static void AddEndpoints(this IEndpointRouteBuilder builder, Assembly assembly)
        {
            var endpoints = assembly.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where
                (
                    m => m.Name.StartsWith("Map") && 
                    m.ReturnType == typeof(void) && 
                    m.GetParameters() is [{ParameterType: Type paramType}] && 
                    paramType == typeof(IEndpointRouteBuilder)
                );

            foreach (var item in endpoints)
                item.Invoke(null, [builder]);
        }
    }
}