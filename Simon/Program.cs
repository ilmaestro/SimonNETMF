using System;
using Microsoft.SPOT;
using System.Threading;
using Simon.Classes;

namespace Simon
{
    public class Program
    {
        public static void Main()
        {
            Controller game = new Controller(new GameState(), new NetduinoIO());

            while (true)
            {
                game.startGame();
                Thread.Sleep(1000);
            }
        }
    }
}