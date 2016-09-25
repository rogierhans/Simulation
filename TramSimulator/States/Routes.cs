using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramSimulator.States
{
    public class Routes
    {
        List<Track> CentralToPR { get; set; }
        List<Track> PRToCentral { get; set; }

        public Routes(List<Track> centralToPR, List<Track> prToCentral)
        {
            this.CentralToPR = centralToPR;
            this.PRToCentral = prToCentral;
        }

        public int? NextTram(int tramId, string depStation)
        {
            Track t = GetTrack(tramId);
            int index = t.Trams.IndexOf(tramId);

            if (index == t.Trams.Count - 1) { return null; }
            else { return t.Trams[index + 1]; }
        }

        public string NextStation(int tramId)
        {
            Track t = GetTrack(tramId);
            return t.To;
        }

        private Track GetTrack(int tramId)
        {
            var track1 = CentralToPR.Find(x => x.Trams.Contains(tramId));
            var track2 = PRToCentral.Find(x => x.Trams.Contains(tramId));

            return (track1 == null ? track2 : track1);
        }
    }

    public class Track
    {
        public string From { get; set; }
        public string To { get; set; }
        public List<int> Trams { get; set; }

        public Track(string from, string to)
        {
            this.From = from;
            this.To = to;
        }
    }
}
