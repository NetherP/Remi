using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject normalScreen;
    [SerializeField] private Button formButton;
    [SerializeField] private Button nextRound;

    private PlayerController winningPlayer;

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.GAME_ENDED, Init);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_ENDED, Init);
    }

    public void Init()
    {
        normalScreen.SetActive(false);
        if (Managers.TurnManager.HandClosed)
        {
            EndRound();
            Messenger<string>.Broadcast(GameEvent.SHOW_PROMPT, Managers.TurnManager.GetName + " TUTUP TANGAN!");
        }
        else
            endScreen.SetActive(true);
        winningPlayer = Managers.TurnManager.CurrentPlayer;
    }

    public void NextPlayer()
    {
        Managers.TurnManager.CalculateScore();
        Managers.TurnManager.EndTurn();
        formButton.gameObject.SetActive(false);
        if (Managers.TurnManager.CurrentPlayer == winningPlayer)
        {
            EndRound();
        }
        else
        {
            Managers.TurnManager.StartTurn();
            formButton.gameObject.SetActive(true);
        }
    }

    private void EndRound()
    {
        ScoreController scoreControl = GetComponent<ScoreController>();
        endScreen.gameObject.SetActive(false);
        scoreControl.OpenScoreBoard();
        nextRound.gameObject.SetActive(true);
        Debug.Log("End Screen");
        int[] score = Managers.TurnManager.CurrentScore();
        for (int i = 0; i < score.Length; ++i)
        {
            Debug.Log("player" + i + " score is: " + score[i]);
        }
    }
}
