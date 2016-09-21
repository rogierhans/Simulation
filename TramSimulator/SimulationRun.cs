using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramSimulator
{

    class SimulationRun
    {

        enum eventType { PersonArrival, TramExpArrival, TramArrival, TramExpDeparture, TramDeparture };
        PriorQ queue = new PriorQ();
        double time = 0;

        double personRate = 10;
        public void run()
        {
            while (true)
            {
                Event e = queue.next();
                time = e.startTime;
                switch (e.type)
                {
                    case eventType.PersonArrival:
                        PersonArrival(e);
                        break;
                    case eventType.TramExpArrival:
                        TramExpArrival(e);
                        break;
                    case eventType.TramArrival:
                        TramArrival(e);
                        break;
                    case eventType.TramExpDeparture:
                        TramExpDeparture(e);
                        break;
                    case eventType.TramDeparture:
                        TramDeparture(e);
                        break;

                }
            }
        }

        //Methods for the events
        void PersonArrival(Event e)
        {
            double newTime = time + (LibraryRoutine.negexp(personRate));
            Event newEvent = new Event(newTime, eventType.PersonArrival, null);


        }
        void TramExpArrival(Event e) { }
        void TramArrival(Event e) { }
        void TramExpDeparture(Event e) { }
        void TramDeparture(Event e) { }

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

        //Class for events
        class Event
        {
            public double startTime;
            public eventType type;
            Tram tram;
            public Event(double time, eventType type, Tram tram)
            {
                this.startTime = time;
                this.type = type;
                this.tram = tram;
            }
        }

        class Person
        {
            double arrivalTime;

            public Person(double arrivalTime)
            {
                this.arrivalTime = arrivalTime;
            }
        }

        // State of Tram includes station i and if it is on track between i and i+1
        class Tram
        {
            Tram nextTram;
            Station currentStation;
            Boolean onTrack;
            List<Person> personsOnTram;
            public Tram(Tram nextTram)
            {
                personsOnTram = new List<Person>();
                this.nextTram = nextTram;
            }
        }
        class Station
        {
            Queue<Person> waitingPersons;
            public Station()
            {
                waitingPersons = new Queue<Person>();
            }

        }

    }

}
