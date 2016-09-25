using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TramSimulator.States;
using TramSimulator.Events;

namespace TramSimulator
{
    public class SimulationState
    {
        public Dictionary<int, Tram> Trams { get; private set; }
        public Dictionary<string, Station> Stations { get; private set; }
        public EventQueue EventQueue { get; private set; }
        public Double Time { get; set; }
        public Routes Routes { get; set; }

        public SimulationState(Dictionary<int, Tram> trams, Dictionary<string, Station> stations, EventQueue eventQueue, Routes routes)
        {
            this.Trams = trams;
            this.Stations = stations;
            this.EventQueue = eventQueue;
            this.Time = 0;
            this.Routes = routes;
        }
    }
}
