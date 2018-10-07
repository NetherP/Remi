using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour {

    private int playerAmount;
    private int lastAmount = 0;
    [SerializeField] private List<InputField> namesField;
    [SerializeField] private Text amountText;
    private string[] names;

    private void Start()
    {
        playerAmount = 4;
        lastAmount = 4;
        //create string array of maximum amount of player: 4
        names = new string[4];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = "player" + (i + 1);
        }
    }

    public void SetPlayerAmount(float player)
    {
        lastAmount = playerAmount;
        playerAmount = (int)player;
        amountText.text = playerAmount.ToString();
        SetName(playerAmount);
    }

    private void SetName(int amount)
    {
        string[] temp = new string[amount];
        for(int i = 0; i < temp.Length; ++i)
        {
            if (i < names.Length)
                temp[i] = names[i];
            else
                temp[i] = "player" + (i + 1);
        }
        names = temp;
    }

    public void Player0(string name)
    {
        names[0] = name;
    }

    public void Player1(string name)
    {
        names[1] = name;
    }

    public void Player2(string name)
    {
        names[2] = name;
    }

    public void Player3(string name)
    {
        names[3] = name;
    }

    public void StartGame()
    {
        Managers.RoundManager.PlayerAmount = playerAmount;
        Managers.TurnManager.playerName = new List<string>(names);
        Managers.RoundManager.LoadRound();
    }

    // Update is called once per frame
    void Update () {
        if (playerAmount != lastAmount)
        {
            if (playerAmount > namesField.Count)
                Debug.LogError("player Amount exceed names count: " + playerAmount);
            foreach (InputField input in namesField)
            {
                input.gameObject.SetActive(false);
            }
            for (int i = 0; i < playerAmount; ++i)
            {
                namesField[i].gameObject.SetActive(true);
            }
            lastAmount = playerAmount;
        }
	}
}
