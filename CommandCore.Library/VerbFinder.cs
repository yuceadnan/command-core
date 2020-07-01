using System;
using System.Linq;
using System.Reflection;
using CommandCore.Library.Attributes;
using CommandCore.Library.Interfaces;

namespace CommandCore.Library
{
    public class VerbFinder : IVerbFinder
    {
        public Type? FindVerbTypeInExecutingAssembly(string verbName)
        {
            // TODO using getentryassembly might not be the perfect solution. Needs more testing here.
            var allTypes = Assembly.GetEntryAssembly()!.GetTypes()
                .Where(a => a.BaseType != null && a.BaseType!.IsGenericType &&
                            a.BaseType.GetGenericTypeDefinition() == typeof(Verb<>)).ToList();

            return allTypes.FirstOrDefault(verbType =>
            {
                var verbNameAttribute = verbType.GetCustomAttribute<VerbNameAttribute>();
                var verbTypeName = verbNameAttribute?.Name ?? verbType.Name;
                // To keep the naming predictable and consistent, making a case-sensitive comparison here.
                return verbTypeName.Equals(verbName);
            });
        }
    }
}