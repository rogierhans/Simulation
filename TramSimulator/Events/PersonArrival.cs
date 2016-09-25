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
        double _startTime;
        public PersonArrival(double startTime, string stationName)
        {
            this._startTime = startTime;
            this._stationName = stationName;
        }
        public override void execute(SimulationState simState)
        {
            simState.Stations[_stationName].WaitingPersons.Enqueue(new Person(_startTime));
            /*station.waitingPersons.Enqueue(new Person(time));
            double newTime = time + (Generate.negexp(station.arrivalRate));
            Event newEvent = new PersonArrival(newTime, station);
            queue.addEvent(newEvent);*/
        }

        public override string ToString()
        {
            return "Arrived " + startTime + " at " + _stationName;
        }
    }
}
