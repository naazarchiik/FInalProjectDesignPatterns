namespace ClassLibraryForTetris
{
    public class ScoreItem
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public ScoreItem(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
