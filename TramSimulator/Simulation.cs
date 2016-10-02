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
                    Station st = stations.Values.ToArray()[i];
                    Console.WriteLine("Station " + st.Name + ": " + st.WaitingPersons.Count);
                }
                Console.WriteLine("From Central to PR");
                for (int i = 0; i < centralToPR.Count; i++)
                {
                    Track t = centralToPR[i];
                    Console.WriteLine("Track " + t.From + " to " + t.To);
                    t.Trams.ForEach(x => Console.WriteLine("\t tram: " + x));
                }
                Console.WriteLine("From PR to Central");
                for (int i = 0; i < prToCentral.Count; i++)
                {
                    Track t = prToCentral[i];
                    Console.WriteLine("Track " + t.From + " to " + t.To);
                    t.Trams.ForEach(x => Console.WriteLine("\t tram: " + x));
                }
                Console.WriteLine("Trams at stations:");
                simState.Trams.Values.ToList().ForEach(x => 
                {
                    if(x.State == Tram.TramState.AtStation)
                    {
                        Console.WriteLine(x.Station + " " + x.TramId);
                    }
                });
                var eventQueueSorted = eventQueue.EventList.OrderBy(x => x.StartTime).ToList();

                Console.WriteLine("Current scheduled events: ");
                eventQueueSorted.ForEach(x => Console.WriteLine("\t event: " + x.ToString()));
           
                Console.ReadLine();

                Event e = simState.EventQueue.Next();
                e.execute(simState);
                Console.WriteLine(e.ToString());
            }
        }

        public SimulationState Setup(int tramFrequency, string[] stationNames)
        {
            var rates = new SimulationRates(_passengerData);

            EventQueue eventQueue = new EventQueue();
            var stations = new Dictionary<string, Station>();
            for (int i = 0; i < stationNames.Length; i++)
            {
                Station station = new Station(stationNames[i]);
                stations.Add(stationNames[i], station);
                eventQueue.AddEvent(new PersonArrival(i * rates.PersonArrivalRate(stationNames[i]), station.Name));
            }

            var trams = new Dictionary<int, Tram>();
            for (int i = 0; i < stationNames.Length; i++)
            {
                trams[i] = new Tram(i,0);
                trams[i].Station = stationNames[stationNames.Length - 1];
                trams[i].State = Tram.TramState.AtShuntyard;
                eventQueue.AddEvent(new EnterTrack(i, i * 60, stations.Values.ToArray()[stationNames.Length - 1].Name));
            }

            var centralToPR = GenerateRoute(stationNames);
            var prToCentral = GenerateRoute(stationNames.Reverse().ToArray());

            var routes = new Routes(centralToPR, prToCentral);

            


            return new SimulationState(trams, stations, eventQueue, routes, rates);
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
