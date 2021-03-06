﻿//Ferdinand van Walree, 3874389
//Rogier Wuijts 

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TramSimulator
{
    class Program
    {
        static void Main(string[] args)
        {

            //We probably want a different method of input for the file paths
            Console.WriteLine("Enter a file path");
            String path = Console.ReadLine();

            Stream stream = File.Open(path, FileMode.Open);
            List<PassengerCount> passengerCountsA = Parser.ParsePassengerCounts(stream);


            //Some example output
            Console.WriteLine("Number of passengercounts: " + passengerCountsA.Count);
            Console.WriteLine(passengerCountsA[0].Trip);
            Console.WriteLine(passengerCountsA[0].Date);
            Console.WriteLine(passengerCountsA[0].Time);
            foreach(var kvp in passengerCountsA[0].EnteringCounts)
            {
                Console.WriteLine(kvp.Key + ": " + kvp.Value);
            }
            foreach (var kvp in passengerCountsA[0].DepartingCounts)
            {
                Console.WriteLine(kvp.Key + ": " + kvp.Value);
            }
            Console.ReadLine();
        }
    }
}
