using System;
using Microsoft.SPOT;
using Simon.Classes;
using System.Threading;

namespace Simon
{
    public class Controller
    {
        private GameState gamestate;
        private NetduinoIO netduino;

        public Controller(GameState gamestate, NetduinoIO netduino) {
	        this.gamestate = gamestate;
	        this.netduino = netduino;
            this.netduino.onButtonPushed += netduino_onButtonPushed;
        }
        
        public void startGame(){
            this.gamestate.reset();
            this.nextRound();
        }

	    private void nextRound(){
            Debug.Print("Computers turn...");
            this.gamestate.startComputersTurn();
            this.netduino.SetButtonsEnabled(false);
            this.playRound(this.gamestate.nextRound(), this.gamestate.getRoundTime);
            Debug.Print("players turn now...");
            this.gamestate.startPlayersTurn();
            this.netduino.SetButtonsEnabled(true);
            this.awaitPlayersTurn();
	    }

        private void awaitPlayersTurn()
        {
            //wait for player to make choices and gamestate to change
            //TODO: add a timeout feature, so the game won't wait indefinately for someone
            while (this.gamestate.currentState == GameState.GameStates.Player)
            {
                Thread.Sleep(1);
            }

            switch (this.gamestate.currentState)
            {
                case GameState.GameStates.WinRound:
                    Thread.Sleep(Configuration.TimeBetweenRounds);
                    this.nextRound();
                    break;
                case GameState.GameStates.WinGame:
                    //you win!
                    this.gameOver();
                    break;
                default:
                    //game over
                    this.netduino.Bleep("red", 2000);
                    this.gameOver();
                    break;
            }
        }

	    private void playRound (int round, int bleepTime) {
		    Debug.Print("Starting round: " + round);
		    for(int i = 0; i < round; i++)
            {
	            string color = this.gamestate.getColorFromStack(i);
                this.netduino.Bleep(color, bleepTime);
                Thread.Sleep(this.gamestate.getPauseTime);
		    }
	    }

        private void netduino_onButtonPushed(string color)
        {
            if (this.gamestate.currentState == GameState.GameStates.Player)
            {
                Debug.Print("Player move: " + color);
                this.netduino.Bleep(color, this.gamestate.getPlayerTime);
                this.gamestate.playerMove(color);
            }
        }

	    private void gameOver(){
		    Debug.Print("Score: " + this.gamestate.getScore);
	    }
    }

}
