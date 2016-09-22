using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramSimulator
{

    static class SimulationRun
    {

        static PriorQ queue = new PriorQ();
        static double time = 0;

        static double personRate = 10;

        static public void run()
        {
            
            while (true)
            {
                Event e = queue.next();
                time = e.startTime;
                e.excute();
            }
        }
        static public void setup() { }

        //Methods for the events
        class PersonArrival : Event
        {
            Station station;
            public PersonArrival(double startTime, Station station)
            {
                this.startTime = startTime;
                this.station = station;
            }
            public new void excute()
            {
                double newTime = time + (LibraryRoutine.negexp(station.arrivalRate));
                Event newEvent = new PersonArrival(newTime, station);

            }
        }
        class TramExpArrival : Event
        {
            Tram tram;
            public TramExpArrival(double startTime, Tram tram)
            {
                this.startTime = startTime;
                this.tram = tram;
            }
            public new void excute()
            {

            }
        }
        class TramArrival : Event
        {
            Tram tram;
            public TramArrival(double startTime, Tram tram)
            {
                this.startTime = startTime;
                this.tram = tram;
            }
            public new void excute()
            {

            }
        }
        class TramExpDeparture : Event
        {
            Tram tram;
            public TramExpDeparture(double startTime, Tram tram)
            {
                this.startTime = startTime;
                this.tram = tram;
            }
            public new void excute()
            {

            }
        }
        class TramDeparture : Event
        {
            Tram tram;
            public TramDeparture(double startTime, Tram tram)
            {
                this.startTime = startTime;
                this.tram = tram;
            }
            public new void excute()
            {

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

        //Class for events
        class Event
        {
            public double startTime;
            public void excute() { }
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
            public double arrivalRate;
            public Station(double arrivalRate)
            {
                this.arrivalRate = arrivalRate;
                waitingPersons = new Queue<Person>();
            }

        }

    }

}
