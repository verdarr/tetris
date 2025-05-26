using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class ScoreManager : IScoreManager
    {
        private const string ScoresFile = "scores.dat";

        public void SaveScore(string playerName, int score)
        {
            List<KeyValuePair<string, int>> scores = GetHighScores();
            scores.Add(new KeyValuePair<string, int>(playerName, score));

            scores.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            if (scores.Count > 10)
                scores = scores.GetRange(0, 10);

            using (BinaryWriter writer = new BinaryWriter(File.Open(ScoresFile, FileMode.Create)))
            {
                foreach (var pair in scores)
                {
                    writer.Write(pair.Key);
                    writer.Write(pair.Value);
                }
            }
        }

        public List<KeyValuePair<string, int>> GetHighScores()
        {
            List<KeyValuePair<string, int>> scores = new List<KeyValuePair<string, int>>();

            if (File.Exists(ScoresFile))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(ScoresFile, FileMode.Open)))
                {
                    while (reader.PeekChar() != -1)
                    {
                        string name = reader.ReadString();
                        int score = reader.ReadInt32();
                        scores.Add(new KeyValuePair<string, int>(name, score));
                    }
                }
            }

            return scores;
        }
    }
}
