using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramSimulator
{
    static class LibraryRoutine
    {
        static Random random = new Random();

        static public double negexp(double u)
        {
            return Math.Log(1 - random.NextDouble()) / (-(1 / u));
        }

        static public double uniform(double low, double high)
        {
            return random.NextDouble() * (high - low) + low;
        }


    }

    //Priority queue for events
    class PriorQ
    {
        SortedList<double, Event> eventList = new SortedList<double, Event>();
        public void addEvent(Event e)
        {
            eventList.Add(e.startTime, e);
        }
        public Event next()
        {
            Event e = eventList.Values[0];
            eventList.RemoveAt(0);
            return e;
        }
    }

    //Class for events yolo
    class Event
    {
        public double startTime;
        public Event(Double time)
        {
            startTime = time;
        }
        override public String ToString()
        {
            return startTime.ToString();
        }
    }
}
