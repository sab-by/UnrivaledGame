using System;
using System.Collections.Generic;
using System.Text;

namespace UnrivaledPractise.Gui
{ 
    class gui
    {
        private int playerHP;
        private int lives;

        public void Initialize(int Lives,int HP)
        {
        //  score = Score;
            playerHP = HP;
            lives = Lives;
        //  gameLevel = Level;
        }
  /*  public int SCORE
    {
        get { return score; }
        set { this.score = value; }
    }
  */
    public int PlayerHP
    {
        get { return playerHP; }
        set { this.playerHP = value; }
    }
        
  
    public int LIVES
    {
        get { return lives; }
        set { this.lives = value; }
    }

    /*public int LEVEL
    {
        get { return gameLevel; }
        set { this.gameLevel = value; }
    }
    public int LEVELUPAGRADE
    {
        get { return levelUpgrade; }
        set { this.levelUpgrade = value; }
    }*/
    }
}
