using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingBasicsHW1
{
    class Program
    {
        static void Main()
        {
            double weight = double.Parse(Console.ReadLine()) * 2.54;
            double height = double.Parse(Console.ReadLine()) / 2.2;
            double age = double.Parse(Console.ReadLine());
            string gender = Console.ReadLine();
            int workoutsPerWeek = int.Parse(Console.ReadLine());
            double result = 0;
            if (gender == "m")
            {
                 result=  66.5 + (13.75 * weight) + (5.003 * height) - (6.755*age);
            }

            if (gender == "f")
            {
                result = 655 + (9.563 * weight) + (1.850 * height) - (4.676 * age);
            }

            Console.WriteLine(result);
        }
    }
}
