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
            setup();
            while (true)
            {
                Event e = queue.next();
                time = e.startTime;
                e.excute();
            }
        }
        static public void setup() {

       }

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
                if (tram.nextTram.currentTrack != tram.currentTrack && !(tram.nextTram.currentTrack.nextStation == tram.currentTrack && tram.nextTram.onStation))
                    queue.addEvent(new TramArrival(startTime, tram));
                else
                    tram.waitingOnNextTram = true;
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
                double dwellTime = 10; // hier moet dus nog iets zinnigs komen te staan
                double newTime = time + dwellTime;
                tram.currentTrack = tram.currentTrack.nextStation;
                tram.onStation = true;
                queue.addEvent(new TramExpDeparture(newTime, tram));
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
                if (tram.nextTram.currentTrack == tram.currentTrack)
                {
                    double departureDifference = time - tram.nextTram.departureTime;
                    if (departureDifference < 40)
                    {
                        queue.addEvent(new TramExpDeparture(time + (40 - departureDifference), tram));
                        return;
                    }
                }
                queue.addEvent(new TramDeparture(time, tram));
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
                tram.onStation = false;
                tram.departureTime = time;
                if (tram.previousTram.waitingOnNextTram && tram.previousTram.currentTrack.nextStation == tram.currentTrack) {
                    tram.previousTram.waitingOnNextTram = false;
                    double queueDelay = 0;//Hoe lang het duurt om op het station te komen als er een voor hem was
                    queue.addEvent(new TramExpArrival(time + queueDelay, tram.previousTram));
                }
                queue.addEvent(new TramExpArrival(time + tram.currentTrack.drivingTime(), tram));
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
            public Tram nextTram;
            public Tram previousTram;
            public Station currentTrack;
            public bool onStation;
            public bool waitingOnNextTram;
            public double departureTime;
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
            public Station nextStation;
            public double avgDrivingTime;
            public Station(double arrivalRate, Station nextStation,double avgDrivingTime)
            {
                this.arrivalRate = arrivalRate;
                this.nextStation = nextStation;
                waitingPersons = new Queue<Person>();
            }
            public double drivingTime() {
                double drivingTime = LibraryRoutine.negexp(avgDrivingTime);
                return drivingTime;
            }

        }

    }

}
