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

namespace Space_Race
{
    public partial class Form1 : Form
    {
        // Players
        Rectangle player1 = new Rectangle(70, 100000, 10, 30); // Set as a high number so they instantly go to the bottom of the screen
        Rectangle player2 = new Rectangle(165, 100000, 10, 30);

        // The 'asteroids', "dot" is the best short term I could think of
        Rectangle dot = new Rectangle(295, 195, 6, 6);

        // Player scores are saved here and default to 0
        int player1Score = 0;
        int player2Score = 0;

        // Speed of objects in scene
        int playerSpeed = 4;
        int dotSpeed = 6;

        // Screen Bounderies
        int screenY = 340;

        // Input check
        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        // Brushes
        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
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

                player1ScoreLabel.Text = player1Score.ToString();
            }

            if (player2.Y < 0)
            {
                // When the player reaches the TOP of the screen
                player2.Y = 0;
                player2Score++;
                player2.Y = screenY;

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

            Refresh(); // Called at the end of the game loop
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(blueBrush, player1);
            e.Graphics.FillRectangle(blueBrush, player2);
            e.Graphics.FillRectangle(whiteBrush, dot);
        }
    }
}
