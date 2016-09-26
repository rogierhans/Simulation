using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TramSimulator.States;

namespace TramSimulator.Events
{
    //event voor als een persoon aan komt op een bepaald station
    //creert ook de event voor de volgende 
    public class PersonArrival : Event
    {
        string _stationName;
        public PersonArrival(double startTime, string stationName)
        {
            this.StartTime = startTime;
            this._stationName = stationName;
        }
        public override void execute(SimulationState simState)
        {
            Station station = simState.Stations[_stationName];
            station.WaitingPersons.Enqueue(new Person(StartTime));  
            double newTime = simState.Time + 60 + (Generate.negexp(station.ArrivalRate));
            simState.EventQueue.AddEvent(new PersonArrival(newTime, _stationName));
        }

        public override string ToString()
        {
            return "Person Arrived " + StartTime + " at " + _stationName;
        }
    }
}
