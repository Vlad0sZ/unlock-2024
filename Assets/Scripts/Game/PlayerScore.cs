using System.Collections.Generic;
using System.Linq;
using Models;

namespace Game
{
    public class PlayerScore
    {
        private readonly Dictionary<string, PlayerWithScore> _playerWithScores = new();


        public void AddScore(WallDataModel wallDataModel, int incrementScore)
        {
            if (wallDataModel == null || string.IsNullOrEmpty(wallDataModel.UserName))
                return;

            string playerName = wallDataModel.UserName;
            var model = _playerWithScores.GetValueOrDefault(playerName);
            model.Name = playerName;
            model.Score += incrementScore;

            _playerWithScores[playerName] = model;
        }

        public IList<PlayerWithScore> GetScores() =>
            _playerWithScores.Values.ToList();

        public void Clear() =>
            _playerWithScores.Clear();
    }
}