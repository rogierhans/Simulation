using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramSimulator
{
    static class LibraryRoutine
    {
        static Random random = new Random();

        static public double negexp(double u)
        {
            return Math.Log(1 - random.NextDouble()) / (-(1 / u));
        }

        static public double uniform(double low, double high)
        {
            return random.NextDouble() * (high - low) + low;
        }
    }
}
