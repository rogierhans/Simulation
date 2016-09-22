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
        static Station[] stations;
        static Tram[] trams;


        static public void run()
        {
            setup();

            while (true)
            {


                Event e = queue.next();
                time = e.startTime;
                e.execute();

                //debug
                Console.WriteLine(e);
                for (int i = 0; i < stations.Length; i++)
                {
                    Console.WriteLine("Station " + stations[i].name);
                    Console.WriteLine("Mensen op het station "+stations[i].waitingPersons.Count);
                }
                for (int i = 0; i < trams.Length; i++)
                {
                    Console.WriteLine("Tram"+ i + " op traject van station :" + trams[i].currentTrack.name);
                    Console.WriteLine("Mensen in de tram "+trams[i].personsOnTram.Count);
                }
                Console.ReadLine();
            }
        }
        static public void setup()
        {
            //om te testen dit
            //verdient geen schoonheids prijs
            stations = new Station[8];
            for (int i = 0; i < stations.Length; i++)
                stations[i] = new Station(i + "", (double)1 / (double)((i + 1) * 10+ 1), 160, (double)1 / (double)8);
            for (int i = 0; i < stations.Length - 1; i++)
                stations[i].nextStation = stations[i + 1];
            stations[stations.Length - 1].nextStation = stations[0];

            trams = new Tram[10];
            for (int i = 0; i < trams.Length; i++)
            {
                trams[i] = new Tram();
                trams[i].currentTrack = stations[stations.Length - 1];
                trams[i].onStation = false;
            }
            for (int i = 0; i < trams.Length - 1; i++)
            {
                trams[i + 1].nextTram = trams[i];
                trams[i].previousTram = trams[i + 1];
            }
            trams[trams.Length - 1].previousTram = trams[0];
            trams[0].nextTram = trams[trams.Length - 1];

            queue.addEvent(new TramArrival(0, trams[0]));
            for (int i = 1; i < trams.Length; i++)
            {
                queue.addEvent(new TramExpArrival(i * 60, trams[i]));
            }
            for (int i = 0; i < stations.Length; i++)
            {
                    queue.addEvent(new PersonArrival(time + (LibraryRoutine.negexp(stations[i].arrivalRate)), stations[i]));
            }



        }
        
        //event voor als een persoon aan komt op een bepaald station
        //creert ook de event voor de volgende 
        class PersonArrival : Event
        {
            Station station;
            public PersonArrival(double startTime, Station station)
            {
                this.startTime = startTime;
                this.station = station;
            }
            public override void execute()
            {
                station.waitingPersons.Enqueue(new Person(time));
                double newTime = time + (LibraryRoutine.negexp(station.arrivalRate));
                Event newEvent = new PersonArrival(newTime, station);
                queue.addEvent(newEvent);

            }
        }

        //event voor tram expected arrival
        class TramExpArrival : Event
        {
            Tram tram;
            public TramExpArrival(double startTime, Tram tram)
            {
                this.startTime = startTime;
                this.tram = tram;
            }
            public override void execute()
            {
                // Als zijn voorganger nog op de zelfde baan zit of op het volgede station
                // dan kan de trein niet aankomen en wacht hij totdat de andere vertrekt
                if (tram.nextTram.currentTrack == tram.currentTrack || (tram.nextTram.currentTrack.nextStation == tram.currentTrack && tram.nextTram.onStation))
                    tram.waitingOnNextTram = true; 
                else
                    queue.addEvent(new TramArrival(startTime, tram));
            }
        }

        //event voor tram arrival
        class TramArrival : Event
        {
            Tram tram;
            public TramArrival(double startTime, Tram tram)
            {
                this.startTime = startTime;
                this.tram = tram;
            }
            public override void execute()
            {
                tram.currentTrack = tram.currentTrack.nextStation;
                tram.onStation = true;
                // in en uitladen van mensen
                tram.detrain();
                tram.entrain();

                double dwellTime = 10; // hier moet dus nog iets zinnigs komen te staan
                double newTime = time + dwellTime;
                queue.addEvent(new TramExpDeparture(newTime, tram));
            }
        }

        //event voor tram expected departure
        class TramExpDeparture : Event
        {
            Tram tram;
            public TramExpDeparture(double startTime, Tram tram)
            {
                this.startTime = startTime;
                this.tram = tram;
            }
            public override void execute()
            {
                //als de volgende tram op het zelfde traject zit en minder dan 40 seconde gelden is vertrokken van een station
                //dan vertrekt de tram pas over (40 sec - het verschil)
                if (tram.nextTram.currentTrack == tram.currentTrack && (time - tram.nextTram.departureTime < 40))
                    queue.addEvent(new TramExpDeparture(time + (40 - (time - tram.nextTram.departureTime)), tram));
                else
                    queue.addEvent(new TramDeparture(time, tram));
            }
        }

        //event voor tram departure
        class TramDeparture : Event
        {
            Tram tram;
            public TramDeparture(double startTime, Tram tram)
            {
                this.startTime = startTime;
                this.tram = tram;
            }
            public override void execute()
            {
                tram.onStation = false;
                tram.departureTime = time;
                // als de vorige tram nog wacht op de tram dan 
                //kan de volgende op het station komen na de queDelay die misschien 0 is ofzo geen idee
                if (tram.previousTram.waitingOnNextTram)
                {
                    tram.previousTram.waitingOnNextTram = false;
                    double queueDelay = 10;//Hoe lang het duurt om op het station te komen als er een voor hem was
                    queue.addEvent(new TramExpArrival(time + queueDelay, tram.previousTram));
                }
                queue.addEvent(new TramExpArrival(time + tram.currentTrack.drivingTime(), tram));
            }
        }
        //Priority queue for events
        //super lelijk O(n) maar n is altijd constant dus het valt mee
        // SortedList en dergelijke zorgden voor problemen
        class PriorQ
        {
            public List<Event> eventList = new List<Event>();
            public void addEvent(Event e)
            {
                eventList.Add(e);
            }
            public Event next()
            {
                double minTime = Int32.MaxValue;
                int index = -1;
                Event e = null;
                for (int i=0; i < eventList.Count; i++)
                {
                    if( eventList[i].startTime < minTime) {
                        index = i;
                        minTime = eventList[i].startTime;
                        e = eventList[i];
                    }
                }
                eventList.RemoveAt(index);
                return e;
            }
        }

        //Class for events
        class Event
        {
            public double startTime;


            public virtual void execute() { }
            public override String ToString()
            {
                return startTime + "";
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
            public Tram nextTram;
            public Tram previousTram;
            public Station currentTrack;
            public bool onStation;
            public bool waitingOnNextTram = false;
            public double departureTime;
            int capacity = 420;
            public List<Person> personsOnTram;
            public Tram()
            {
                personsOnTram = new List<Person>();

            }
            public void detrain()
            {
                int i = (int)(personsOnTram.Count * currentTrack.detrainRate);
                Random random = new Random();
                for (int j = 0; j < i; j++)
                {
                    int randomNumber = random.Next(0, personsOnTram.Count - 1);
                    personsOnTram.RemoveAt(randomNumber);
                }
            }
            public void entrain()
            {
                while (personsOnTram.Count < capacity && currentTrack.waitingPersons.Count > 0)
                {
                    Person p = currentTrack.waitingPersons.Dequeue();
                    personsOnTram.Add(p);
                }

            }
        }
        class Station
        {
            public Queue<Person> waitingPersons;
            public double arrivalRate;
            public Station nextStation;
            public double avgDrivingTime;
            public double detrainRate;
            public String name; // good to know for sanity check
            public Station(String name, double arrivalRate, double avgDrivingTime, double detrainRate)

            {
                this.name = name;
                this.avgDrivingTime = avgDrivingTime;
                this.arrivalRate = arrivalRate;
                this.detrainRate = detrainRate;
                waitingPersons = new Queue<Person>();
            }
            public double drivingTime()
            {
                double drivingTime = avgDrivingTime;
                return drivingTime;
            }


        }

    }

}
