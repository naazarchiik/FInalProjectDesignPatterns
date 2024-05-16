using ClassLibraryForTetris;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for GameScoreForm.xaml
    /// </summary>
    public partial class GameScoreForm : Window
    {
        private int _scores = 0;
        private const string _scoreFilePath = "scores.txt";

        public GameScoreForm(int scores)
        {
            InitializeComponent();
            NameTextBox.Visibility = Visibility.Hidden;
            saveBtn.Content = "OK";
            _scores = scores;

            var scoresItems = GetSavedScores();
            if (scoresItems.Length == 0)
            {
                scoreLbl.Content = "No high scores yet!";
                NameTextBox.Visibility = Visibility.Visible;
                saveBtn.Content = "Save";
                return;
            }
            else
            {
                foreach (var score in scoresItems)
                    scoreLbl.Content += $"{score.Name} - {score.Score}\n";
            }

            if (scores > scoresItems.Max(s => s.Score))
            {
                MessageBox.Show("Congratulations! You have a new high score!");
                NameTextBox.Visibility = Visibility.Visible;
                saveBtn.Content = "Save";
            }


        }

        private ScoreItem[] GetSavedScores()
        {
            if (!File.Exists(_scoreFilePath))
                return new ScoreItem[0];

            var scoresLines = File.ReadAllLines(_scoreFilePath);
            var scores = new List<ScoreItem>();

            foreach (var line in scoresLines)
                scores.Add(new ScoreItem(line.Split(' ')[0], int.Parse(line.Split(' ')[1])));

            return scores.ToArray();
        }

        private void SaveScoreToFile(string name, int score)
        {
            var existingScores = GetSavedScores();

            if (existingScores.Length == 0)
            {
                File.WriteAllText(_scoreFilePath, $"{name} {score}");
                return;
            }

            int maxScore = existingScores.Max(s => s.Score);

            if (existingScores.Length < 10 || score > maxScore)
            {
                var newScores = new List<ScoreItem>(existingScores);
                newScores.Add(new ScoreItem(name, score));
                newScores = newScores.OrderByDescending(s => s.Score).Take(10).ToList();

                File.WriteAllLines(_scoreFilePath, newScores.Select(s => $"{s.Name} {s.Score}"));
            }

            if (existingScores.Length >= 10 && score > maxScore)
            {
                var newScores = new List<ScoreItem>(existingScores);
                newScores.Add(new ScoreItem(name, score));
                newScores = newScores.OrderByDescending(s => s.Score).Take(10).ToList();

                File.WriteAllLines(_scoreFilePath, newScores.Select(s => $"{s.Name} {s.Score}"));
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveScoreToFile(NameTextBox.Text, _scores);
            Close();
        }
    }
}
