using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using System.Threading;

namespace Simon.Classes
{
    public class Piezos
    {
        private PWM[] piezos;
        public Piezos(Cpu.Pin[] pins)
        {
            this.piezos = new PWM[pins.Length];
            for (int n = 0; n < pins.Length; n++)
            {
                PWM piezo = new PWM(pins[n]);
                piezo.SetDutyCycle(0);
                this.piezos[n] = piezo;
            }
        }
                
        public void tone(int piezo, float frequency, int duration)
        {
            PWM pin = this.piezos[piezo];
            // calculate the actual period and turn the
            // speaker on for the defined period of time
            uint period = (uint)(1000000 / frequency);
            pin.SetPulse(period, period / 2);
            Thread.Sleep(duration);
            // turn the speaker off
            pin.SetDutyCycle(0);
        }

        public void StartTone(int piezo, float frequency)
        {
            PWM pin = this.piezos[piezo];
            // calculate the actual period and turn the
            // speaker on for the defined period of time
            uint period = (uint)(1000000 / frequency);
            pin.SetPulse(period, period / 2);
        }
        public void StopTone(int piezo)
        {
            PWM pin = this.piezos[piezo];
            pin.SetDutyCycle(0);
        }
    }
}
