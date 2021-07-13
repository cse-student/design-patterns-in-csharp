using System.Reflection;
using DesignPatterns.Creational.Singleton;

namespace DesignPatterns.Creational.UnitTests.Singleton
{
    public static class SingletonExtensions
    {
        public static void ResetSingleThreadSingleton()
        {
            typeof(SingleThreadSingleton)!
                .GetField("_instance", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, null);
        }
        
        public static void ResetThreadSafeSingleton()
        {
            typeof(ThreadSafeSingleton)!
                .GetField("_instance", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, null);
        }
    }
}