using System;

namespace HugeBawls
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (Client game = new Client())
            {
                game.Run();
            }
        }
    }
}

