using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Collections;
using System.Threading;

namespace Simon.Classes
{
    public class NetduinoIO
    {
        private OutputPort[] leds;
        private InterruptPort[] buttons;
        private bool[] buttonStates;
        private Piezos piezos;

        public delegate void buttonHandler(string color);
        public event buttonHandler onButtonPushed;

        public NetduinoIO()
        {
            this.leds = new OutputPort[Configuration.Leds.Length];
            this.buttons = new InterruptPort[Configuration.Buttons.Length];
            this.piezos = new Piezos(Configuration.Piezos);
            this.buttonStates = new bool[Configuration.Buttons.Length];

            for (int n = 0; n < Configuration.Leds.Length; n++)
            {
                this.leds[n] = new OutputPort(Configuration.Leds[n], false);
            }

            // Defines all pins as interrupt ports
            for (int i = 0; i < Configuration.Buttons.Length; ++i)
            {
                this.buttons[i] = new InterruptPort(Configuration.Buttons[i], false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLevelHigh);
                this.buttons[i].OnInterrupt += new NativeEventHandler(buttons_OnButtonPushed);
                this.buttons[i].DisableInterrupt();
                this.buttonStates[i] = false;
            }
        }

        protected void buttons_OnButtonPushed(uint Pin, uint State, DateTime Time)
        {
            int bi = buttonIndex((Cpu.Pin)Pin);
            //make sure each button only hits when pushing down
            if (!this.buttonStates[bi])
            {
                string color = Configuration.Colors[bi];
                if (onButtonPushed != null)
                {
                    onButtonPushed(color);
                }
                this.buttonStates[bi] = true;
            }
            else
            {
                //button is now up..
                this.buttonStates[bi] = false;
            }
            Thread.Sleep(Configuration.BtnBounceTime);
            //clear the interrupt for this button, ready for the next one
            this.buttons[bi].ClearInterrupt();
        }

        public void SetButtonsEnabled(bool enabled)
        {
            for (int i = 0; i < this.buttons.Length; ++i)
            {
                if (enabled)
                {
                    this.buttons[i].EnableInterrupt();
                }
                else
                {
                    this.buttons[i].DisableInterrupt();
                }
            }
        }

        private int buttonIndex(Cpu.Pin button)
        {
            int buttonIndex = 0;
            for (int i = 0; i < Configuration.Buttons.Length; i++)
            {
                if (Configuration.Buttons[i] == button)
                {
                    buttonIndex = i;
                    break;
                }
            }
            return buttonIndex;
        }

        private int colorIndex(string color)
        {
            int colorIndex = 0;
            for(int i = 0; i < Configuration.Colors.Length; i++){
                if (Configuration.Colors[i] == color){
                    colorIndex = i;
                    break;
                }
            }
            return colorIndex;
        }
                
        protected OutputPort getLed(string color)
        {
            return (OutputPort)this.leds[colorIndex(color)];
        }

        public void Bleep(string color, int bleepTime)
        {
            Debug.Print("Bleeping " + color);
            OutputPort led = getLed(color);
            //turn on light, bleep piezo, turn off light
            led.Write(true);
            this.piezos.tone(0, Configuration.Sounds[colorIndex(color)], bleepTime);
            led.Write(false);
        }
    }
}
