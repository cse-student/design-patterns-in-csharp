using System.Linq;
using DesignPatterns.Common;
using DesignPatterns.Creational.Singleton;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DesignPatterns.Creational.UnitTests.Singleton
{
    [Collection("Sequential")]
    public class ThreadSafeSingletonSingleThreadSystemTests
    {
        private readonly IInstantiationCounter _counter;
        
        public ThreadSafeSingletonSingleThreadSystemTests()
        {
            SingletonExtensions.ResetThreadSafeSingleton();
            _counter = Substitute.For<IInstantiationCounter>();
        }
        
        [Fact]
        public void GetInstance_Instantiate_PrivateProperty()
        {
            ThreadSafeSingleton.GetInstance(_counter);
            var sut = ReflectionExtensions.GetPrivateStaticPropertyValue<ThreadSafeSingleton>("_instance");

            sut.Should().NotBeNull();
        }

        [Fact]
        public void GetInstance_InstantiateClassOnce_InSingleThreadedSystem()
        {
            for (var i = 0; i < 100; i++)
            {
                ThreadSafeSingleton.GetInstance(_counter);
            }
            
            _counter.Received(1).IncrementCounter();
        }
        
        [Fact]
        public void GetInstance_ReturnsSameInstance_InSingleThreadedSystem()
        {
            var expected = ThreadSafeSingleton.GetInstance(_counter);
            var singletons = new ThreadSafeSingleton[100];
            for (var i = 0; i < 100; i++)
            {
                singletons[i] = ThreadSafeSingleton.GetInstance(_counter);
            }

            singletons.ToList().ForEach(x => x.Should().Be(expected));
        }
    }
}