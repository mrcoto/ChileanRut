using System;

namespace MrCoto.ChileanRut
{
    class Program
    {
        static void Main(string[] args)
        {
            var rut1 = new Rut("12345678", "k");
            var rut2 = Rut.Parse("12345678-k");
            var rut3 = Rut.Parse("12345678K");
            var rut4 = Rut.Parse(12345678);
            Console.WriteLine(rut1);
            Console.WriteLine(rut2);
            Console.WriteLine(rut3);
            Console.WriteLine(rut4);
            Console.WriteLine($"IsValid = {rut1.IsValid()}");
            Console.WriteLine($"Format Full      = {rut1.Format()}");
            Console.WriteLine($"Format Only Dash = {rut1.Format(RutFormat.ONLY_DASH)}");
            Console.WriteLine($"Format Escaped   = {rut1.Format(RutFormat.ESCAPED)}");
            Console.WriteLine($"Equals rut 1 & rut 2 = {rut1 == rut2}");
            Console.WriteLine($"Equals rut 1 & rut 3 = {rut1 == rut4}");
            var (num, dv) = rut1;
            Console.WriteLine($"Num = {num}, dv = {dv}");

            var rut5 = Rut.Parse(12345);
            var rut6 = Rut.Parse(12346);
            Console.WriteLine($"rut5 < rut6 = {rut5 < rut6}");
            Console.WriteLine($"rut5 > rut6 = {rut5 > rut6}");
        
            Console.WriteLine($"Random Rut = {Rut.Random(max: 18000000, seed: 42)}");

            var randomList = Rut.Randoms(n: 5);
            randomList.ForEach(rut => Console.WriteLine($"Random Rut From Random List = {rut}"));

            var uniqueRandomList = Rut.Uniques(n: 5);
            randomList.ForEach(rut => Console.WriteLine($"Random Rut From Unique Random List = {rut}"));
        }
    }
}
