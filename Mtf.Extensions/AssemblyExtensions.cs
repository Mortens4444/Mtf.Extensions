using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mtf.Extensions
{
    public static class AssemblyExtensions
    {
        public static IOrderedEnumerable<Type> GetTypesInNamespace(this Assembly assembly, string @namespace)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return assembly.GetTypes()
                .Where(type => type.Namespace == @namespace)
                .OrderBy(type => type.Name);
        }

        public static void InitializeStaticObjects(this Assembly assembly, string @namespace)
        {
            var types = assembly.GetTypesInNamespace(@namespace);
            var classes = types.Where(type => type.IsClass && !type.IsAbstract).ToList();
            foreach (var @class in classes)
            {
                RuntimeHelpers.RunClassConstructor(@class.TypeHandle);
            }
        }
    }

}
