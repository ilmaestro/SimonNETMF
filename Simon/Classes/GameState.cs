using System;
using Microsoft.SPOT;
using System.Collections;

namespace Simon.Classes
{
    public class GameState
    {
        #region Private
        private int[] stack;
        private int stackSize = Classes.Configuration.GameStacks[0];
        private int currentRound = 0;
        private int playerIndex = 0;
        private int pauseTime = Classes.Configuration.PauseTime;
        private int roundTime = Classes.Configuration.StartRoundTime;
        private int roundTimeDecrease = Classes.Configuration.RoundTimeDecrease;
        private int pauseTimeDecrease = Classes.Configuration.PauseTimeDecrease;
        private int playerTime = Classes.Configuration.PlayerTime;
        private string[] colors = Classes.Configuration.Colors;
        
        #endregion

        #region Properties

        public enum GameStates
        {
            Computer,
            Player,
            WinRound,
            WinGame,
            Lose
        }

        public GameStates currentState
        {
            get;
            set;
        }

        public bool hasPlayerWon
        {
            get { return (this.currentRound >= this.stackSize); }
        }

        public int getRoundTime
        {
            get { return this.roundTime - (this.roundTimeDecrease * this.currentRound); }
        }

        public int getPauseTime
        {
            get { return this.pauseTime - (this.pauseTimeDecrease * this.currentRound); }
        }

        public int getScore
        {
            get { return this.currentRound; }
        }

        public int getPlayerTime
        {
            get { return this.playerTime; }
        }
        #endregion

        #region Constructor
        public GameState()
        {
            this.currentState = GameStates.Computer;
        }
        #endregion
        
        public void resetStack() {
		    this.stack = new int[this.stackSize];
            Random r = new Random();
		    for(int i = 0; i<this.stackSize; i++){
			    this.stack[i] = r.Next(Configuration.Colors.Length);
		    }
	    }

        public int nextRound()
        {
            this.currentRound++;
            return this.currentRound;
        }

        public string getColorFromStack(int index)
        {
            return this.colors[this.stack[index]];
        }

        public void startComputersTurn()
        {
            this.currentState = GameStates.Computer;
            this.playerIndex = 0;
        }

	    public void startPlayersTurn() {
		    this.playerIndex = 0;
            this.currentState = GameStates.Player;
	    }

        public void playerMove(string color)
        {
            bool colorMatches = this.colors[this.stack[this.playerIndex]] == color;
		    this.playerIndex++;
            bool roundIsOver = (this.playerIndex >= this.currentRound);
            bool gameIsOver = roundIsOver && (this.currentRound >= this.stackSize);

            //check if player chose the wrong color
            if (!colorMatches)
            {
                this.currentState = GameStates.Lose;
            }
            //check if game is over
            else if (gameIsOver)
            {
                this.currentState = GameStates.WinGame;
            }
            //check if round is over
            else if (roundIsOver)
            {
                this.currentState = GameStates.WinRound;
            }
            else
            {
                //player continues..
                this.currentState = GameStates.Player;
            }
	    }

	    public void reset(){
            this.currentState = GameStates.Computer;
            this.currentRound = 0; //start in the middle of the game for now..
		    this.playerIndex = 0;
		    this.resetStack();
	    }

    }

    public struct playerMoveResult
    {
        public bool succeeded;
        public bool roundIsOver;
        public playerMoveResult(bool isMatch, bool roundIsOver)
        {
            this.succeeded = isMatch;
            this.roundIsOver = roundIsOver;
        }
    }
}

