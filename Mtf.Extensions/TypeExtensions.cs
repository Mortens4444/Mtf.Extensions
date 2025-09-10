using System;
using System.Collections.Generic;
using System.Linq;

namespace Mtf.Extensions
{
    public static class TypeExtensions
    {
        public static IList<T> InstantiateSubclassesOfAbstractClass<T>(this Type abstractClass, IEnumerable<Type> allTypes)
            where T : class
        {
            var filteredTypes = allTypes
                .Where(type => type.IsSubclassOf(abstractClass) && !type.IsAbstract && type.GetConstructor(Type.EmptyTypes) != null);

            return ConstructResult<T>(filteredTypes);
        }

        public static IList<T> InstantiateSubtypes<T>(this Type targetType, IEnumerable<Type> allTypes)
            where T : class
        {
            var filteredTypes = allTypes
                .Where(type => targetType.IsAssignableFrom(type) && !type.IsAbstract && type.GetConstructor(Type.EmptyTypes) != null);

            return ConstructResult<T>(filteredTypes);
        }

        private static List<T> ConstructResult<T>(IEnumerable<Type> filteredTypes) where T : class
        {
            var result = new List<T>();
            foreach (var filteredType in filteredTypes)
            {
                var obj = (T)Activator.CreateInstance(filteredType);
                result.Add(obj);
            }
            return result;
        }

        public static IEnumerable<Type> GetTypesInNamespace(this Type searchedType, string @namespace)
        {
            if (searchedType == null)
            {
                throw new ArgumentNullException(nameof(searchedType));
            }

            var assembly = searchedType.Assembly;
            return assembly.GetTypes().Where(type => String.Equals(type.Namespace, @namespace, StringComparison.Ordinal));
        }

        public static Type GetTypeByName(string typeFullname)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.SelectMany(assembly => assembly.GetTypes())
                .First(type => type.FullName == typeFullname);
        }

        public static bool IsArray(this Type type)
        {
            return type != null && type.FullName == "System.Array";
        }

        public static bool IsPrimitiveOrString(this Type type)
        {
            return type != null && (type.IsPrimitive || type == typeof(string));
        }

        public static bool IsGenericList(this Type type, out Type elementsType)
        {
            while (type != null)
            {
                if (type.FullName.StartsWith("System.Collections.Generic.List`", StringComparison.Ordinal))
                {
                    elementsType = type.GenericTypeArguments.First();
                    return true;
                }
                type = type.BaseType;
            }
            elementsType = null;
            return false;
        }

    }
}
