using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TramSimulator.States;

namespace TramSimulator.Events
{
    //event voor tram expected arrival
    public class TramExpectedArrival : Event
    {
        int _tramId;
        string _arrStation;
        public TramExpectedArrival(int tramId, double startTime, string arrStation)
        {
            this._tramId = tramId;
            this._arrStation = arrStation;
            this.StartTime = startTime;
        }
        public override void execute(SimulationState simState)
        {
            var station = simState.Stations[_arrStation];
            var tram = simState.Trams[_tramId];
            if (station.WaitingTrams.Count > 0 || station.TramIsStationed)
            {
                station.WaitingTrams.Enqueue(tram);
                Event e = new TramExpectedArrival(tram.TramId, simState.Time + 60 + Generate.negexp(60), _arrStation);
                simState.EventQueue.AddEvent(e);
            }
            else
            {
                station.TramIsStationed = true;
                Event e = new TramExpectedDeparture(_tramId, simState.Time + 60 + Generate.negexp(60), _arrStation);
                simState.EventQueue.AddEvent(e);
            }
            // Als zijn voorganger nog op de zelfde baan zit of op het volgede station
            // dan kan de trein niet aankomen en wacht hij totdat de andere vertrekt
           // if (tram.nextTram().currentTrack == tram.currentTrack || (tram.nextTram().currentTrack.nextStation() == tram.currentTrack && tram.nextTram().onStation))
              //  tram.waitingOnNextTram = true;
           // else
                //queue.addEvent(new TramArrival(startTime, tram));
        }

        public override string ToString()
        {
            return "Tram expected arrival " + StartTime + " at " + _arrStation;
        }
    }
}
