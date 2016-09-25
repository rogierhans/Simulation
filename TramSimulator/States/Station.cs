using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramSimulator.States
{
    public class Station
    {
        public Queue<Person> WaitingPersons { get; set; }
        public Queue<Tram> WaitingTrams { get; set; }
        public double ArrivalRate { get; set; }
        public double AvgDrivingTime { get; set; }
        public double EmptyRate { get; set; }
        readonly String _name;
        public String Name { get { return _name; } }
        public bool TramIsStationed { get; set; }

        public Station(String name, double arrivalRate, double avgDrivingTime, double detrainRate)
        {
            this._name = name;
            this.AvgDrivingTime = avgDrivingTime;
            this.ArrivalRate = arrivalRate;
            this.EmptyRate = detrainRate;
            this.WaitingPersons = new Queue<Person>();
            this.WaitingTrams = new Queue<Tram>();
            this.TramIsStationed = false;
        }
        public double DrivingTime()
        {
            double drivingTime = AvgDrivingTime;
            return drivingTime;
        }
        public Station NextStation()
        {
            //if (index == stations.Length - 1)
           //     return stations[0];
            //else
            //    return stations[index + 1];
            return null;
        }

    }
}
