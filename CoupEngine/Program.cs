using System;

namespace CoupEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            CoupEngine engine = new CoupEngine(args);
            engine.PlayGame();
            Console.ReadLine();
        }
    }
}
