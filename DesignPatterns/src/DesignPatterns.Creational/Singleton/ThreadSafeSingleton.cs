namespace DesignPatterns.Creational.Singleton
{
    public class ThreadSafeSingleton
    {
        private static ThreadSafeSingleton _instance;
        private static object _instanceLock = new();

        private ThreadSafeSingleton(IInstantiationCounter counter)
        {
            counter.IncrementCounter();
        }

        public static ThreadSafeSingleton GetInstance(IInstantiationCounter counter)
        {
            lock (_instanceLock)
            {
                return _instance ??= new ThreadSafeSingleton(counter);
            }
        }
    }
}