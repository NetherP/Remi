using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private List<Text> scores;

	public void OpenScoreBoard()
    {
        InitScore();
        scoreBoard.gameObject.SetActive(true);
    }

    private void InitScore()
    {
        TurnManager manager = Managers.TurnManager;
        int playerCount = manager.PlayerCount;
        for (int i = 0; i < playerCount; ++i)
        {
            scores[i].text = manager.playerName[i] + ": " + manager.Score(i);
        }
    }
}
