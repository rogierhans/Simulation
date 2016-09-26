using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TramSimulator.States;

namespace TramSimulator.Events
{
    //event voor tram expected departure
    public class TramExpectedDeparture : Event
    {
        int _tramId;
        string _depStation;

        public TramExpectedDeparture(int tramId, double startTime, string depStation)
        {
            this._tramId = tramId;
            this.StartTime = startTime;
            this._depStation = depStation;
        }

        public override void execute(SimulationState simState)
        {
            var eventQueue = simState.EventQueue;
            Tram tram = simState.Trams[_tramId];
            int? id = simState.Routes.NextTram(tram.TramId, _depStation);
            double currTime = simState.Time;
            if (id.HasValue)
            {
                Tram nextTram = simState.Trams[id.Value];
                if(currTime - nextTram.DepartureTime >= 40)
                {
                    eventQueue.AddEvent(new TramExpectedArrival(_tramId, 0, simState.Routes.NextStation(_tramId)));
                }
                else
                {
                    double timeDiff = currTime + (40 - currTime - nextTram.DepartureTime);
                    eventQueue.AddEvent(new TramExpectedDeparture(_tramId, timeDiff, _depStation));
                }
            }
            else
            {
                eventQueue.AddEvent(new TramExpectedArrival(_tramId, 0, simState.Routes.NextStation(_tramId)));
            }
            //als de volgende tram op het zelfde traject zit en minder dan 40 seconde gelden is vertrokken van een station
            //dan vertrekt de tram pas over (40 sec - het verschil)
            //if (tram.nextTram().currentTrack == tram.currentTrack && (time - tram.nextTram().departureTime < 40))
                //queue.addEvent(new TramExpDeparture(time + (40 - (time - tram.nextTram().departureTime)), tram));
            //else
                //queue.addEvent(new TramDeparture(time, tram));
        }

        public override string ToString()
        {
            return "Tram expected departure " + StartTime + " from " + _depStation;
        }
    }
}
