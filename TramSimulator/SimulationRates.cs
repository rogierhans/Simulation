using System;
using System.Linq;
using System.Collections.Generic;

namespace TramSimulator
{
    public class SimulationRates
    {

        public SimulationRates(List<PassengerCount> passengerCounts)
        {

        }

        public double PersonArrivalRate(string station)
        {
            throw new NotImplementedException();
        }

        public double TramArrivalRate(string depStation, string arrStation)
        {
            throw new NotImplementedException();
        }

        public double AvgTramArrival(string depStation, string arrStation)
        {
            throw new NotImplementedException();
        }

        public double DelayRate(string station)
        {
            throw new NotImplementedException();
        }

        public double TramEmptyRate(string station)
        {
            throw new NotImplementedException();
        }

        public double TramFillRate(string station)
        {
            throw new NotImplementedException();
        }


    }
}