namespace DesignPatterns.Creational.Singleton
{
    public class SingleThreadSingleton
    {
        private static SingleThreadSingleton _instance;

        private SingleThreadSingleton(IInstantiationCounter counter)
        {
            counter.IncrementCounter();
        }

        public static SingleThreadSingleton GetInstance(IInstantiationCounter counter)
        {
            return _instance ??= new SingleThreadSingleton(counter);
        }
    }
}