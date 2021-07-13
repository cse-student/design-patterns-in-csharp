using System.Linq;
using DesignPatterns.Common;
using DesignPatterns.Creational.Singleton;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DesignPatterns.Creational.UnitTests.Singleton
{
    
    [Collection("Sequential")]
    public class SingleThreadSingletonSingleThreadSystemTests
    {
        private readonly IInstantiationCounter _counter;
        
        public SingleThreadSingletonSingleThreadSystemTests()
        {
            SingletonExtensions.ResetSingleThreadSingleton();
            _counter = Substitute.For<IInstantiationCounter>();
        }
        
        
        [Fact]
        public void GetInstance_Instantiate_PrivateProperty()
        {
            SingleThreadSingleton.GetInstance(_counter);
            var sut = ReflectionExtensions.GetPrivateStaticPropertyValue<SingleThreadSingleton>("_instance");

            sut.Should().NotBeNull();
        }

        [Fact]
        public void GetInstance_InstantiateClassOnce_InSingleThreadedSystem()
        {
            for (var i = 0; i < 100; i++)
            {
                SingleThreadSingleton.GetInstance(_counter);
            }
            
            _counter.Received(1).IncrementCounter();
        }
        
        [Fact]
        public void GetInstance_ReturnsSameInstance_InSingleThreadedSystem()
        {
            var expected = SingleThreadSingleton.GetInstance(_counter);
            var singletons = new SingleThreadSingleton[100];
            for (var i = 0; i < 100; i++)
            {
                singletons[i] = SingleThreadSingleton.GetInstance(_counter);
            }

            singletons.ToList().ForEach(x => x.Should().Be(expected));
        }
    }
}