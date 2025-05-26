using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public interface IScoreManager
    {
        void SaveScore(string playerName, int score);
        List<KeyValuePair<string, int>> GetHighScores();
    }
}
