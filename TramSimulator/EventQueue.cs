using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TramSimulator.Events;

namespace TramSimulator
{
    //Priority queue for events
    //super lelijk O(n) maar n is altijd constant dus het valt mee
    // SortedList en dergelijke zorgden voor problemen
    public class EventQueue
    {
        public SortedList<double, Event> eventList = new SortedList<double, Event>();

        public void AddEvent(Event e)
        {
            eventList.Add(e.startTime, e);
        }

        public Event Next()
        {
            Event e = eventList.Values[0];
            eventList.RemoveAt(0);
            return e;
        }

        public bool HasEvent()
        {
            return eventList.Count > 0;
        }
    }
}
