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
        public Queue<int> WaitingTrams { get; set; }
        readonly String _name;
        public String Name { get { return _name; } }
        public bool TramIsStationed { get; set; }

        public Station(String name)
        {
            this._name = name;
            this.WaitingPersons = new Queue<Person>();
            this.WaitingTrams = new Queue<int>();
            this.TramIsStationed = false;
        }
        
    }
}
