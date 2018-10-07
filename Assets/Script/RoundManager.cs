using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour {

    public int PlayerAmount;
    public bool isEnded;

    public void LoadRound()
    {
        isEnded = false;
        string name = "Cekih";
        SceneManager.LoadScene(name);
    }

    public void InitializeGame()
    {
        Managers.TurnManager.Init(PlayerAmount);
    }
}
