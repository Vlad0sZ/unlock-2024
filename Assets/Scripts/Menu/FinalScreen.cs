using System.Collections.Generic;
using Models;
using UI;
using UnityEngine;

namespace Menu
{
    public class FinalScreen : ScreenWithButtons
    {
        [SerializeField] private PlayerScoreGrid playerGrid;
        
        public void UpdateScores(IEnumerable<PlayerWithScore> playerWithScores) =>
            playerGrid.UpdateGrid(playerWithScores);

        protected override void OnPlay()
        {
            // TODO without tutorial
            GameController.StartGame();
            gameObject.SetActive(false);
        }
    }
}