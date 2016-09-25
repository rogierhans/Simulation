using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TramSimulator;

namespace TramSimulator.States
{
    // State of Tram includes station i and if it is on track between i and i+1
    public class Tram
    {
        public enum TramState { AtStation, Waiting, InRoute, Delayed};

        public Station CurrentTrack { get; set; }
        public TramState State { get; set; }
        const int capacity = 420;
        readonly int _tramId;
        public int TramId { get { return _tramId; } }
        public double DepartureTime { get; set; }
        public List<Person> PersonsOnTram { get; set; }

        public Tram(int tramId, double departureTime)
        {
            this._tramId = tramId;
            this.DepartureTime = departureTime;
            PersonsOnTram = new List<Person>();

        }
        public void EmptyTram()
        {
            int i = (int)(PersonsOnTram.Count * CurrentTrack.EmptyRate);
            Random random = new Random();
            for (int j = 0; j < i; j++)
            {
                PersonsOnTram.RemoveAt((int)Generate.uniform(0, PersonsOnTram.Count - 1));
            }
        }
        public void FillTram()
        {
            while (PersonsOnTram.Count < capacity && CurrentTrack.WaitingPersons.Count > 0)
            {
                Person p = CurrentTrack.WaitingPersons.Dequeue();
                PersonsOnTram.Add(p);
            }

        }

        //Waar hebben we deze methoden voor nodig?
        public Tram NextTram()
        {
            //if (0 == index)
               // return trams[trams.Length - 1];
           // else
               // return trams[index - 1];
            return null;
        }
        public Tram PreviousTram()
        {
            //if (trams.Length - 1 == index)
              //  return trams[0];
           // else
                //return trams[index + 1];
            return null;
        }
    }
}
