using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace RockPaperScissorsGUI
{
    public abstract class GameBase : Form
    {
        protected int rounds, wins, losses, draws;
        protected Random rand;

        public GameBase()
        {
            rand = new Random();
            rounds = wins = losses = draws = 0;
        }

        protected abstract void OnChoiceClick(object? sender, EventArgs e);
        protected abstract string DetermineWinner(string playerChoice, string computerChoice);
        protected abstract void SaveStats();
        protected abstract void LoadStats();
    }

    public class MainForm : GameBase
    {
        private Button rockButton;
        private Button paperButton;
        private Button scissorsButton;
        private Label resultsLabel;
        private Label statsLabel;

        public MainForm()
        {
            Text = "Ro Sham Bo";
            Size = new System.Drawing.Size(1067, 800);

            rockButton = new Button { Text = "Rock", Left = 50, Top = 50, Width = 400, Height = 400 };
            paperButton = new Button { Text = "Paper", Left = 500, Top = 50, Width = 400, Height = 400 };
            scissorsButton = new Button { Text = "Scissors", Left = 950, Top = 50, Width = 400, Height = 400 };

            rockButton.Paint += new PaintEventHandler(OnButtonPaint);
            paperButton.Paint += new PaintEventHandler(OnButtonPaint);
            scissorsButton.Paint += new PaintEventHandler(OnButtonPaint);

            rockButton.Image = LoadImage(@"C:\Code\Git\CSE310\RoShamBo\RockPaperScissorsGUI\Images\Rock.jpg");
            paperButton.Image = LoadImage(@"C:\Code\Git\CSE310\RoShamBo\RockPaperScissorsGUI\Images\Paper.jpg");
            scissorsButton.Image = LoadImage(@"C:\Code\Git\CSE310\RoShamBo\RockPaperScissorsGUI\Images\Scissors.jpg");

            resultsLabel = new Label { Text = "", Left = 50, Top = 500, Width = 1000, Height = 50, Font = new Font("Arial", 24) };
            statsLabel = new Label { Text = "", Left = 50, Top = 600, Width = 1000, Height = 50, Font = new Font("Arial", 24) };

            rockButton.Click += new EventHandler(OnChoiceClick);
            paperButton.Click += new EventHandler(OnChoiceClick);
            scissorsButton.Click += new EventHandler(OnChoiceClick);

            Controls.Add(rockButton);
            Controls.Add(paperButton);
            Controls.Add(scissorsButton);
            Controls.Add(resultsLabel);
            Controls.Add(statsLabel);

            LoadStats();
        }

        private Image? LoadImage(string path)
        {
            try
            {
                return Image.FromFile(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {path}\n{ex.Message}");
                return null;
            }
        }

        protected override void OnChoiceClick(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string playerChoice = button.Text;
                string[] choices = { "Rock", "Paper", "Scissors" };
                string computerChoice = choices[rand.Next(choices.Length)];

                string result = DetermineWinner(playerChoice, computerChoice);
                resultsLabel.Text = $"Computer chose: {computerChoice}. {result}";
                statsLabel.Text = $"Rounds: {rounds}, Wins: {wins}, Losses: {losses}, Draws: {draws}";

                SaveStats();
            }
        }

        protected override string DetermineWinner(string playerChoice, string computerChoice)
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

        protected override void SaveStats()
        {
            using (StreamWriter writer = new StreamWriter("stats.txt"))
            {
                writer.WriteLine(rounds);
                writer.WriteLine(wins);
                writer.WriteLine(losses);
                writer.WriteLine(draws);
            }
        }

        protected override void LoadStats()
        {
            if (File.Exists("stats.txt"))
            {
                using (StreamReader reader = new StreamReader("stats.txt"))
                {
                    rounds = int.Parse(reader.ReadLine() ?? "0");
                    wins = int.Parse(reader.ReadLine() ?? "0");
                    losses = int.Parse(reader.ReadLine() ?? "0");
                    draws = int.Parse(reader.ReadLine() ?? "0");
                }
            }
        }

        private void OnButtonPaint(object? sender, PaintEventArgs e)
        {
            if (sender is Button button)
            {
                e.Graphics.Clear(button.BackColor);
                if (button.Image != null)
                {
                    e.Graphics.DrawImage(button.Image, 0, 0, button.Width, button.Height);
                }

                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                using (StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddString(button.Text, font.FontFamily, (int)font.Style, font.Size, new Rectangle(0, 0, button.Width, button.Height), format);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(Brushes.White, path);
                    e.Graphics.DrawPath(new Pen(Color.Black, 1.5f), path);
                }
            }
        }
    }

}