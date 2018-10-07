using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    [SerializeField] GameObject playerPrefab;
    [SerializeField] private GameObject dealerPrefab;
    public DealerController Dealer { get; private set; }

    [SerializeField] private GameObject kartuBawahPrefab;
    public KartuBawah Bawah { get; private set; }

    //dictionary with <playerindex, score> 
    private Dictionary<int, int> playerScore;
    public int Score(int playerIndex)
    {
        return playerScore[playerIndex];
    }

    //List of player name
    public List<string> playerName;
    public string GetName { get { return playerName[playerTurn]; } }
    private List<PlayerController> players;
    public int PlayerCount { get { return players.Count; } }
    public PlayerController CurrentPlayer { get; private set; }
    public int playerTurn;
    public bool HandClosed { get; private set; }

    public void Startup()
    {
        Debug.Log("Turn manager is starting...");

        status = ManagerStatus.Started;
    }

    public void Init(int playerAmount)
    {
        HandClosed = false;
        if(playerScore == null)
        {
            playerScore = new Dictionary<int, int>();
            for(int i = 0; i < playerAmount; ++i)
            {
                playerScore.Add(i, 0);
            }
            playerTurn = 0;
        }
        else
        {
            if (playerScore.Count != playerAmount)
                Debug.LogError("playerAmount and playerScore.Count differ: " + playerAmount + " != " + playerScore.Count);
            int highScore = playerScore[0];
            int playerIndex = 0;
            foreach (KeyValuePair<int, int> player in playerScore)
            {
                if (player.Value > highScore)
                {
                    highScore = player.Value;
                    playerIndex = player.Key;
                }
            }
            playerTurn = playerIndex;
        }
        players = new List<PlayerController>();
        for(int i = 0; i < playerAmount; ++i)
        {
            PlayerController playerController = CreatePlayer();
            players.Add(playerController);
            playerController.index = i;
        }
        Debug.Log("Player turn: " + playerTurn);
        CurrentPlayer = players[playerTurn];
        GameObject currentDealer = Instantiate(dealerPrefab);
        Dealer = currentDealer.GetComponent<DealerController>();
        GameObject kartuBawah = Instantiate(kartuBawahPrefab);
        Bawah = kartuBawah.GetComponent<KartuBawah>();
        Dealer.Init();
        StartCoroutine(DistributeCards(playerAmount));
    }

    private IEnumerator DistributeCards(int playerAmount)
    {
        yield return null;
        for (int i = 0; i < playerAmount; ++i)
        {
            for (int j = 0; j < 7; ++j)
            {
                players[i].Push(Dealer.TakeFromTop());
            }
        }
    }

    public void StartTurn()
    {
        CurrentPlayer.SetUp();
        Dealer.ReRender();                                          //replace this with distribute animation
        Managers.InputManager.IsTurnEnded = false;
        Managers.InputManager.EnabledLayer.Add(9);
    }

    public void EndTurn()
    {
        Managers.InputManager.RemoveAll();
        players[playerTurn].WrapUp();
        if (playerTurn < players.Count - 1)
        {
            playerTurn++;
        }
        else
        {
            playerTurn = 0;
        }
        CurrentPlayer = players[playerTurn];
        Managers.InputManager.RemoveAll();
        Managers.InputManager.IsTurnEnded = true;
        Managers.InputManager.EnabledLayer.Clear();
    }

    private PlayerController CreatePlayer()
    {
        GameObject temp = Instantiate(playerPrefab);
        PlayerController controller = temp.GetComponent<PlayerController>();
        return controller;
    }

    public void IncreaseScore (int playerIndex, int score)
    {
        playerScore[playerIndex] += score;
    }

    public void CalculateScore()
    {
        int score = CurrentPlayer.Scorer();
        playerScore[playerTurn] += score;
    }

    public int[] CurrentScore()
    {
        int[] score = new int[playerScore.Count];
        foreach(var temp in playerScore)
        {
            score[temp.Key] = temp.Value;
        }
        return score;
    }

    public void CloseHand(int playerIndex)
    {
        IncreaseScore(playerIndex, 250);
        HandClosed = true;
    }
}
