using System;
using Microsoft.SPOT;
using System.IO;
using System.Collections;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Microsoft.SPOT.Hardware;

namespace Simon.Classes
{
    public static class Configuration
    {
        // times, ms
        public static int StartRoundTime = 1000;
        public static int RoundTimeDecrease = 100;
        public static int PauseTime = 200;
        public static int PauseTimeDecrease = 10;
        public static int PlayerTime = 500;
        public static int BtnBounceTime = 100;
        public static int TimeBetweenRounds = 1000;

        // stack sizes, in increasing difficulty
        public static int[] GameStacks = { 8, 10, 12, 14 };
        
        // everything in the same order as the colors;
        public static string[] Colors   = { "red", "green", "blue", "yellow" };
        public static Cpu.Pin[] Leds    = { Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D3 };
        public static Cpu.Pin[] Buttons = { Pins.GPIO_PIN_D7, Pins.GPIO_PIN_D8, Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D10 };
        public static Cpu.Pin[] Piezos  = { Pins.GPIO_PIN_D5, Pins.GPIO_PIN_D6 };
        public static float[] Sounds    = { 110.0f, 220.0f, 440.0f, 880.0f };


    }
}
