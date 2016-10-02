﻿using System;
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
            var tram = simState.Trams[_tramId];

            //Find the next tram
            int? id = simState.Routes.NextTram(tram.TramId, _depStation);
            double currTime = StartTime;
            if (id.HasValue) //If there exists a tram on the same track
            {
                Tram nextTram = simState.Trams[id.Value];
                //Tram may only depart if there is 40s between the next tram and this one
                if(currTime - nextTram.DepartureTime >= 40)
                {
                    HandleDepartureEvent(simState);
                }
                else //Tram was not able to depart and has to schedule new departure
                {
                    double timeDiff = currTime + (40 - currTime - nextTram.DepartureTime);
                    eventQueue.AddEvent(new TramExpectedDeparture(_tramId, timeDiff, _depStation));
                }
            }
            else
            {
                HandleDepartureEvent(simState);
            }
        }

        private void HandleDepartureEvent(SimulationState simState)
        {
            var eventQueue = simState.EventQueue;
            var tram = simState.Trams[_tramId];
            var station = simState.Stations[_depStation];
            var nextStation = simState.Routes.NextStation(_tramId, _depStation);
            var arrRate = simState.Rates.TramArrivalRate(_depStation, nextStation);

            //Handle departure of a tram
            tram.State = Tram.TramState.OnTrack;
            station.TramIsStationed = false;
            eventQueue.AddEvent(new TramExpectedArrival(_tramId, arrRate, nextStation));
            simState.Routes.MoveToNextTrack(_tramId, _depStation);

            //if there was a tram waiting create an arrival event
            if(station.WaitingTrams.Count > 0)
            {
                var wTramId = station.WaitingPersons.Dequeue();
                eventQueue.AddEvent(new TramExpectedArrival(_tramId, 0, _depStation));
            }
        }

        public override string ToString()
        {
            return "Tram " + _tramId + " expected departure " + StartTime + " from " + _depStation;
        }
    }
}
