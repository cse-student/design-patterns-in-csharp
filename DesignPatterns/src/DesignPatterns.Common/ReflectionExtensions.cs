using System.Reflection;

namespace DesignPatterns.Common
{
    public static class ReflectionExtensions
    {
        public static object GetPrivateStaticPropertyValue<T>(string propertyName)
        {
            return typeof(T)!
                .GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Static)!
                .GetValue(null);
        }
        
        public static void SetPrivateStaticPropertyValue<T>(string propertyName, object value)
        {
            typeof(T)!
                .GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, value);
        }
    }
}