using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TramSimulator.States;

namespace TramSimulator.Events
{
    public class EnterTrack : Event
    {
        int _tramId;
        string _station;

        public EnterTrack(int tramId, double startTime, string station)
        {
            this._tramId = tramId;
            this._station = station;
            this.StartTime = startTime;
        }

        public override void execute(SimulationState simState)
        {
            var route = simState.Routes.PRToCentral;
            route[0].Trams.Add(_tramId);
            var eventQueue = simState.EventQueue;
            eventQueue.AddEvent(new TramExpectedDeparture(_tramId, simState.Time, _station));
        }

        public override string ToString()
        {
            return "Tram " + _tramId + " enters " + _station + " at " + StartTime;
        }
    }
}
