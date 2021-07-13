using System.Linq;
using System.Threading.Tasks;
using DesignPatterns.Creational.Singleton;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DesignPatterns.Creational.UnitTests.Singleton
{
    [Collection("Sequential")]
    public class ThreadSafeSingletonConcurrentSystemTests
    {
        private readonly IInstantiationCounter _counter;
        
        public ThreadSafeSingletonConcurrentSystemTests()
        {
            SingletonExtensions.ResetThreadSafeSingleton();
            _counter = Substitute.For<IInstantiationCounter>();
        }

        [Fact]
        public void ThreadSafeSingleton_GetInstance_InstantiateOnce_InMultiThreadedSystem()
        {
            var singletons = new ThreadSafeSingleton[100];
            Parallel.For(0, 100, i =>
            {
                singletons[i] = ThreadSafeSingleton.GetInstance(_counter);
            });
            _counter.Received(1).IncrementCounter();
        }

        [Fact]
        public void ThreadSafeSingleton_GetInstance_AlwaysReturnsSameInstance_InMultiThreadedSystem()
        {
            var singletons = new ThreadSafeSingleton[100];
            Parallel.For(0, 100, i =>
            {
                singletons[i] = ThreadSafeSingleton.GetInstance(_counter);
            });

            singletons.ToList().ForEach(x => x.Should().Be(singletons[0]));
        }
    }
}