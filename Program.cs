using System;
using System.Diagnostics;
using System.Linq;

namespace ExGens.Poetry
{
    class Program
    {
        static void Main(string[] args)
        {
            var words = ZaliznyaksAccentuatedParadigm.Read("All_Forms.txt");
            var builder = new PoemBulder(words, 4);
            var phrase = Console.ReadLine();
            var continuation = builder.BuildSimilarStrings(phrase)
                                      .Do(_ => Debug.WriteLine(_))
                                      .First();

            Console.WriteLine(continuation);
        }
    }
}
