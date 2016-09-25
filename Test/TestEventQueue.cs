using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TramSimulator.Events;
using TramSimulator.States;
using TramSimulator;

namespace Test
{
    [TestClass]
    public class TestEventQueue
    {
        [TestMethod]
        public void TestPriority()
        {
            EventQueue eventQueue = new EventQueue();
            eventQueue.AddEvent(new PersonArrival(5.0, new Station("alpha", 0, 0, 0, 0)));
            eventQueue.AddEvent(new PersonArrival(10.0, new Station("beta", 0, 0, 0, 0)));
            eventQueue.AddEvent(new PersonArrival(3.0, new Station("gamma", 0, 0, 0, 0)));
            eventQueue.AddEvent(new PersonArrival(6.0, new Station("delta", 0, 0, 0, 0)));

            Event e1 = eventQueue.Next();
            Event e2 = eventQueue.Next();
            Event e3 = eventQueue.Next();
            Event e4 = eventQueue.Next();

            Assert.AreEqual(e1.startTime, 3.0);
            Assert.AreEqual(e2.startTime, 5.0);
            Assert.AreEqual(e3.startTime, 6.0);
            Assert.AreEqual(e4.startTime, 10.0);
        }
    }
}
