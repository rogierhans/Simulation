using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TramSimulator.Events;
using TramSimulator.States;

namespace TramSimulator
{

    public class Simulation
    {
        List<PassengerCount> _passengerData;

        public Simulation(List<PassengerCount> passengerData)
        {
            this._passengerData = passengerData;
        }


        public void run(int tramFrequency, List<int> timeTable, string dayOfWeek, string[] stationNames)
        {
            SimulationState simState = Setup(tramFrequency, stationNames);
            while (simState.EventQueue.HasEvent())
            {

                //debug
                var stations = simState.Stations;
                var trams = simState.Trams;
                var centralToPR = simState.Routes.CentralToPR;
                var prToCentral = simState.Routes.PRToCentral;
                var eventQueue = simState.EventQueue;
                for (int i = 0; i < stations.Count; i++)
                {
                    Console.WriteLine("Station " + stations.Values.ToArray()[i].Name);
                    Console.WriteLine("Mensen op het station "+stations.Values.ToArray()[i].WaitingPersons.Count);
                }
                for (int i = 0; i < centralToPR.Count; i++)
                {
                    Track t = centralToPR[i];
                    Console.WriteLine("Track " + t.From + " to " + t.To);
                    t.Trams.ForEach(x => Console.WriteLine("\t tram: " + x));
                }
                for (int i = 0; i < prToCentral.Count; i++)
                {
                    Track t = prToCentral[i];
                    Console.WriteLine("Track " + t.From + " to " + t.To);
                    t.Trams.ForEach(x => Console.WriteLine("\t tram: " + x));
                }
                var eventQueueSorted = eventQueue.EventList.OrderBy(x => x.StartTime).ToList();

                Console.WriteLine("Current scheduled events: ");
                eventQueueSorted.ForEach(x => Console.WriteLine("\t event: " + x.ToString()));
           
                Console.ReadLine();

                Event e = simState.EventQueue.Next();
                simState.Time = e.StartTime;
                e.execute(simState);
                Console.WriteLine(e.ToString());
            }
        }

        public SimulationState Setup(int tramFrequency, string[] stationNames)
        {
            EventQueue eventQueue = new EventQueue();
            var stations = new Dictionary<string, Station>();
            for (int i = 0; i < stationNames.Length; i++)
            {
                Station station = new Station(stationNames[i],(double)1 / (double)((i + 1) * 10+ 1), 160, (double)1 / (double)8);
                stations.Add(stationNames[i], station);
                eventQueue.AddEvent(new PersonArrival(i *  Generate.negexp(station.ArrivalRate), station.Name));
            }

            var trams = new Dictionary<int, Tram>();
            for (int i = 0; i < stationNames.Length; i++)
            {
                trams[i] = new Tram(i,0);
                trams[i].CurrentTrack = stations.Values.ToArray()[0];
                trams[i].State = Tram.TramState.AtStation;
                eventQueue.AddEvent(new EnterTrack(i, i * 60, stations.Values.ToArray()[0].Name));
            }

            var centralToPR = GenerateRoute(stationNames);
            var prToCentral = GenerateRoute(stationNames.Reverse().ToArray());

            Routes routes = new Routes(centralToPR, prToCentral);


            return new SimulationState(trams, stations, eventQueue, routes);
        }

        static private List<Track> GenerateRoute(string[] stationNames)
        {
            List<Track> route = new List<Track>();
            string from = stationNames[0];
            for (int i = 1; i < stationNames.Length; i++)
            {
                route.Add(new Track(from, stationNames[i]));
                from = stationNames[i];
            }

            return route;
        }
        
        
    }

}
