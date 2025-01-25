using System;
using System.Windows.Forms;

namespace RockPaperScissorsGUI
{
    public class MainForm : Form
    {
        private Button rockButton;
        private Button paperButton;
        private Button scissorsButton;
        private Label resultsLabel;
        private Label statsLabel;
        private Random rand;
        private int rounds, wins, losses, draws;

        public MainForm()
        {
            Text = "Ro Sham Bo";
            Size = new System.Drawing.Size(400, 300);

            rockButton = new Button { Text = "Rock", Left = 50, Top = 50, Width = 100 };
            paperButton = new Button { Text = "Paper", Left = 150, Top = 50, Width = 100 };
            scissorsButton = new Button { Text = "Scissors", Left = 250, Top = 50, Width = 100 };

            resultsLabel = new Label { Text = "", Left = 50, Top = 150, Width = 300, Height = 30 };
            statsLabel = new Label { Text = "", Left = 50, Top = 200, Width = 300, Height = 30 };

            rockButton.Click += new EventHandler(OnChoiceClick);
            paperButton.Click += new EventHandler(OnChoiceClick);
            scissorsButton.Click += new EventHandler(OnChoiceClick);

            Controls.Add(rockButton);
            Controls.Add(paperButton);
            Controls.Add(scissorsButton);
            Controls.Add(resultsLabel);
            Controls.Add(statsLabel);

            rand = new Random();
            rounds = wins = losses = draws = 0;
        }

        private void OnChoiceClick(object sender, EventArgs e)
        {
            string playerChoice = (sender as Button).Text;
            string[] choices = { "Rock", "Paper", "Scissors" };
            string computerChoice = choices[rand.Next(choices.Length)];

            string result = DetermineWinner(playerChoice, computerChoice);
            resultsLabel.Text = $"Computer chose: {computerChoice}. {result}";
            statsLabel.Text = $"Rounds: {rounds}, Wins: {wins}, Losses: {losses}, Draws: {draws}";

        }

        private string DetermineWinner(string playerChoice, string computerChoice)
        {
            rounds++;
            if (playerChoice == computerChoice)
            {
                draws++;
                return "It's a draw!";
            }
            else if ((playerChoice == "Rock" && computerChoice == "Scissors") ||
                     (playerChoice == "Paper" && computerChoice == "Rock") ||
                     (playerChoice == "Scissors" && computerChoice == "Paper"))
            {
                wins++;
                return "You're the Champion!!!";
            }
            else
            {
                losses++;
                return "You lose!";

            }
        }
    }   
}