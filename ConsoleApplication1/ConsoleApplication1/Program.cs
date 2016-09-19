using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
        }



        
        class LibraryRoutine {
            Random random = new Random();

            public double negexp(double u)
            {
                return Math.Log(1 - random.NextDouble()) / (-(1 / u));
            }

            public double uniform(double low, double high)
            {
                return random.NextDouble()*(high-low) + low;
            }
        }

    }
}
