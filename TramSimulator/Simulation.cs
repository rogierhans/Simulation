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


        static public void run(int tramFrequency, List<int> timeTable, string dayOfWeek, string[] stationNames)
        {
            SimulationState simState = Setup(tramFrequency, stationNames);
            while (simState.EventQueue.HasEvent())
            {
                Event e = simState.EventQueue.Next();
                simState.Time = e.startTime;
                e.execute(simState);

                //debug
                Console.WriteLine(e);
                var stations = simState.Stations;
                var trams = simState.Trams;
                for (int i = 0; i < stations.Count; i++)
                {
                    Console.WriteLine("Station " + stations.Values.ToArray()[i].Name);
                    Console.WriteLine("Mensen op het station "+stations.Values.ToArray()[i].WaitingPersons.Count);
                }
                for (int i = 0; i < trams.Count; i++)
                {
                    Console.WriteLine("Tram"+ i + " op traject van station :" + trams[i].CurrentTrack.Name);
                    Console.WriteLine("Mensen in de tram "+trams[i].PersonsOnTram.Count);
                }
                Console.ReadLine();
            }
        }

        static public SimulationState Setup(int tramFrequency, string[] stationNames)
        {
            EventQueue eventQueue = new EventQueue();
            var stations = new Dictionary<string, Station>();
            for (int i = 0; i < stationNames.Length; i++)
            {
                Station station = new Station(stationNames[i],(double)1 / (double)((i + 1) * 10+ 1), 160, (double)1 / (double)8);
                stations.Add(stationNames[i], station);
                eventQueue.AddEvent(new PersonArrival(i * 60 *  Generate.negexp(station.ArrivalRate), station.Name));
            }

            var trams = new Dictionary<int, Tram>();
            for (int i = 0; i < stationNames.Length; i++)
            {
                trams[i] = new Tram(i,0);
                trams[i].CurrentTrack = stations.Values.ToArray()[0];
                trams[i].State = Tram.TramState.AtStation;
                eventQueue.AddEvent(new TramExpectedArrival(i, i * 60, stations.Values.ToArray()[0].Name));
            }


            return new SimulationState(trams, stations, eventQueue, null);
        }
        
        
    }

}
