using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Space_Race
{
    public partial class Form1 : Form
    {
        // Global Variables
        #region
        // Screen the players are on
        string screen = "title";

        // Players
        Rectangle player1 = new Rectangle(70, 100000, 10, 20); // Set as a high number so they instantly go to the bottom of the screen
        Rectangle player2 = new Rectangle(165, 100000, 10, 20);

        // The 'asteroids', "dot" is the best short term I could think of
        List<Rectangle> dots = new List<Rectangle>();
        List<int> dotSpeeds = new List<int>(); 
        int xSpot = 0;
        
        // Have the range be 4-7
        int minDotSpeed = 2;
        int maxDotSpeed = 4;

        int dotSizeX = 5, dotSizeY = 5;
        int spawnChance = 30;

        Random randGen = new Random();

        // Player scores are saved here and default to 0
        int player1Score = 0;
        int player2Score = 0;

        // Speed of objects in scene
        int playerSpeed = 4;

        // Screen Bounderies
        int screenY = 340;

        // Input check
        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        // Sounds
        SoundPlayer hurtSound;
        SoundPlayer pointSound;

        // Brushes
        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        #endregion
        public Form1()
        {
            InitializeComponent();
            gameLoop.Enabled = false;
            winnerText.Text = "";
            pointSound = new SoundPlayer(Properties.Resources.pickupCoin);
            hurtSound = new SoundPlayer(Properties.Resources.explosion);
            hurtSound.Stop();
            pointSound.Stop();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Checking "KeyDOWN" Input
            #region
            switch (e.KeyCode) // Detect when the input is used
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
            }
            #endregion
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // Checking "KeyUP" Input
            #region
            switch (e.KeyCode) // Detect when input is let go of
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
            }
            #endregion
        }


        private void gameLoop_Tick(object sender, EventArgs e)
        {
            // Player Movement
            #region
            if (wDown)
            {
                player1.Y -= playerSpeed;
            }

            if (sDown)
            {
                player1.Y += playerSpeed;
            }

            if (upArrowDown)
            {
                player2.Y -= playerSpeed;
            }

            if (downArrowDown)
            {
                player2.Y += playerSpeed;
            }
            #endregion

            // Area Bounds & Player Score Management
            #region
            if (player1.Y < 0) 
            {
                // When the player reaches the TOP of the screen
                player1.Y = 0;
                player1Score++;
                player1.Y = screenY;
                pointSound.Stop();
                pointSound.Play();

                player1ScoreLabel.Text = player1Score.ToString();
            }

            if (player2.Y < 0)
            {
                // When the player reaches the TOP of the screen
                player2.Y = 0;
                player2Score++;
                player2.Y = screenY;
                pointSound.Stop();
                pointSound.Play();

                player2ScoreLabel.Text = player2Score.ToString();
            }

            if (player1.Y > screenY)
            {
                // When the player reaches the BOTTOM of the screen
                player1.Y = screenY;
            }

            if (player2.Y > screenY)
            {
                // When the player reaches the BOTTOM of the screen
                player2.Y = screenY;
            }
            #endregion

            // Move 'Dots'
            #region
            for (int i = 0; i < dots.Count(); i++)
            {
                int x = dots[i].X + dotSpeeds[i];

                dots[i] = new Rectangle(x, dots[i].Y, dotSizeX, dotSizeY);
            }

            // Should a new dot be created?
            int randNum = randGen.Next(1, 101);

            // Actually make a new 'dot' if it should
            if (randNum < spawnChance)
            {
                int y = randGen.Next(10, 301);
                // Determine which side it will spawn on
                int sideNum = randGen.Next(1, 3);

                switch (sideNum)
                {
                    case 1: // Left
                        dotSpeeds.Add(randGen.Next(minDotSpeed, maxDotSpeed + 1));
                        xSpot = dotSizeX * 2;
                        break;
                    case 2: // Right
                        int tempDotSpeed = randGen.Next(minDotSpeed, maxDotSpeed + 1);
                        // Setting it to a negative number to make it move the other way
                        dotSpeeds.Add(-tempDotSpeed);
                        xSpot = this.Width - dotSizeX * 2;
                        break;
                }

                dots.Add(new Rectangle(xSpot, y, dotSizeX, dotSizeY));
            }

            // See if the dot should be removed
            for (int i = 0; i < dots.Count(); i++)
            {
                if (dots[i].X > this.Width - dotSizeX)
                {
                    dots.RemoveAt(i);
                    dotSpeeds.RemoveAt(i);
                }
            }

            // Check for collision with players
            for (int i = 0; i < dots.Count(); i++)
            {
                if (player1.IntersectsWith(dots[i]))
                {
                    player1.Y = 10000; // High number to make it go to the bottom of the play area
                    hurtSound.Stop();
                    hurtSound.Play();
                }

                if (player2.IntersectsWith(dots[i]))
                {
                    player2.Y = 10000; // High number to make it go to the bottom of the play area
                    hurtSound.Stop();
                    hurtSound.Play();
                }
            }
            #endregion

            // Player Win
            #region
            if (player1Score >= 3)
            {
                EndGame("Player1");
            } else if (player2Score >= 3)
            {
                EndGame("Player2");
            }
            #endregion

            Refresh(); // Called at the end of the game loop
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            // Start the game under any condition
            titleLabel.Text = "";
            gameLoop.Enabled = true;
            startButton.Enabled = false;
            startButton.Visible = false;
            winnerText.Text = "";
            player1Score = 0;
            player2Score = 0;
            player1ScoreLabel.Text = player1Score.ToString();
            player2ScoreLabel.Text = player2Score.ToString();


            screen = "play";
        }

        private void EndGame(string player)
        {
            gameLoop.Enabled = false;
            winnerText.Text = $"{player} wins!";
            startButton.Enabled = true;
            startButton.Visible = true;
            startButton.Text = "Play Again";
            screen = "GameOver";
            dots.Clear();
            dotSpeeds.Clear();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Drawing Graphics
            #region
            e.Graphics.FillRectangle(blueBrush, player1);
            e.Graphics.FillRectangle(blueBrush, player2);

            for (int i = 0; i < dots.Count(); i++)
            {
                e.Graphics.FillRectangle(whiteBrush, dots[i]);
            }
            #endregion
        }
    }
}
