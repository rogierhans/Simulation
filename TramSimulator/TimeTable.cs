using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TramSimulator.Events;
using TramSimulator.States;

namespace TramSimulator
{
    public class TimeTable
    {
        public Dictionary<Track, double> expectedTime;
        public Dictionary<Track, double> totalDelay;
        public Dictionary<Track, double> maxDelay;
        public Dictionary<Track, int> numberOverOneMinute;
        public int numberOfRounds;

        public double startTime;
        public double halfTime; // voor het wachten op een eindstation
        public double totalTime; //voor de volgende timeTable



        public TimeTable(SimulationRates rates, Routes routes, double startTime)
        {
            totalDelay = new Dictionary<Track, double>();
            maxDelay = new Dictionary<Track, double>();
            numberOverOneMinute = new Dictionary<Track, int>();
            foreach (Track t in routes.CentralToPR)
            {
                totalDelay.Add(t, 0);
                maxDelay.Add(t, 0);
                numberOverOneMinute.Add(t, 0);
            }
            foreach (Track t in routes.PRToCentral)
            {
                totalDelay.Add(t, 0);
                maxDelay.Add(t, 0);
                numberOverOneMinute.Add(t, 0);
            }
            numberOfRounds = 0;
            renewTimeTable(rates, routes, startTime);
        }

        public void renewTimeTable(SimulationRates rates, Routes routes, double startTime)
        {
            numberOfRounds++;
            this.startTime = startTime;
            double dwellTime = 30; //hier moet iets logisch staan

            expectedTime = new Dictionary<Track, double>();

            double time = startTime;
            for (int i = 0; i < routes.CentralToPR.Count; i++)
            {
                Track track = routes.CentralToPR[i];
                expectedTime.Add(track, time);
                time += rates.AvgTramArrival( track.To,track.From) + dwellTime;

            }
            double reverse = 180;
            time += reverse;
            halfTime = time;
            for (int i = 0; i < routes.PRToCentral.Count; i++)
            {
                Track track = routes.PRToCentral[i];
                expectedTime.Add(track, time);
                time += rates.AvgTramArrival(track.To, track.From) + dwellTime;

            }
            time += reverse;
            totalTime = time;
        }

        public void addTime(Track track, double time)
        {
            double delay = time - expectedTime[track];
            if (delay > 0)
            {
                totalDelay[track] += delay;
            }
            if (delay > 1)
            {
                numberOverOneMinute[track] += 1;
            }
            if (delay > maxDelay[track])
                maxDelay[track] = delay;
        }
    }
}
