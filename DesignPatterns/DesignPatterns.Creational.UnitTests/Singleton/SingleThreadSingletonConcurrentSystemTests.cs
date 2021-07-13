using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DesignPatterns.Creational.Singleton;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Xunit.Sdk;

namespace DesignPatterns.Creational.UnitTests.Singleton
{
    [Collection("Singleton")]
    public class SingleThreadSingletonConcurrentSystemTests
    {
        private readonly IInstantiationCounter _counter;
        
        public SingleThreadSingletonConcurrentSystemTests()
        {
            SingletonExtensions.ResetSingleThreadSingleton();
            _counter = Substitute.For<IInstantiationCounter>();
        }
        
        [Fact]
        public void SingleThreadSingleton_GetInstance_InstantiateMoreThanOnce_InMultiThreadedSystem()
        {
            var threads = new List<Thread>();
            for (var i = 0; i < 100; i++)
            {
                threads.Add(new Thread(GetInstance));
            }
            Parallel.ForEach(threads, thread =>
            {
                thread.Start();
                thread.Join();
            });
            
            var receivedCalls = _counter
                .ReceivedCalls()
                .Where(c => c.GetMethodInfo().Name == "IncrementCounter");
            receivedCalls.Count().Should().BeGreaterThan(1);
        }
        
        [Fact]
        public void SingleThreadSingleton_GetInstance_ReturnsDifferentInstance_InMultiThreadedSystem()
        {
            var singletons = new SingleThreadSingleton[100];
            Parallel.For(0, 100, i =>
            {
                singletons[i] = SingleThreadSingleton.GetInstance(_counter);
            });

            Action act = () => singletons.ToList().ForEach(x => x.Should().Be(singletons[0]));
            act.Should().Throw<XunitException>();
        }

        private void GetInstance()
        {
            SingleThreadSingleton.GetInstance(_counter);
        }
    }
}