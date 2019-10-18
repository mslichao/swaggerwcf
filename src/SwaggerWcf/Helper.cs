using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SwaggerWcf
{
    public static class GetCustomAttributeHelper
    {
        public static T GetCustomAttribute<T>(this ParameterInfo element) where T : Attribute
        {
            return Attribute.GetCustomAttribute(element, typeof(T)) as T;
        }

        public static T[] GetCustomAttributes<T>(this ParameterInfo element) where T : Attribute
        {
            return Attribute.GetCustomAttributes(element, typeof(T), false).Select(item => item as T).ToArray();
        }

        public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute
        {
            return Attribute.GetCustomAttribute(element, typeof(T)) as T;
        }

        public static T[] GetCustomAttributes<T>(this MemberInfo element) where T : Attribute
        {
            return Attribute.GetCustomAttributes(element, typeof(T), false).Select(item => item as T).ToArray();
        }
    }
}
