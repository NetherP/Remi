using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CekihController : MonoBehaviour {

    [SerializeField] private Button CekihButton;
    [SerializeField] private Text cekihText;
    [SerializeField] private Text warningText;
    
    public void CekihActive(bool active)
    {
        if (Managers.RoundManager.isEnded)
        {
            active = false;
        }
        if (Managers.TurnManager.CurrentPlayer.isCekih)
        {
            cekihText.text = "Cancel\nCekih";
        }
        else
        {
            cekihText.text = "Cekih";
        }
        CekihButton.gameObject.SetActive(active);
    }

    public void CekihClick()
    {
        int prevPlayer = Managers.TurnManager.playerTurn - 1;
        if (prevPlayer < 0)
        {
            prevPlayer = Managers.RoundManager.PlayerAmount - 1;
        }
        if (Managers.TurnManager.CurrentPlayer.isCekih)
        {
            Managers.TurnManager.CurrentPlayer.ToggleCekih();
            Messenger<int>.Broadcast(GameEvent.BATAL_CEKIH, prevPlayer);
        }
        else
        {
            Managers.TurnManager.CurrentPlayer.ToggleCekih();
            Messenger<int>.Broadcast(GameEvent.CEKIH, prevPlayer);
        }
        CekihButton.gameObject.SetActive(false);
    }

    public void WarningText()
    {
        if (Managers.TurnManager.CurrentPlayer.canSink)
        {
            int nextPlayer = Managers.TurnManager.playerTurn + 1;
            if (nextPlayer > Managers.RoundManager.PlayerAmount - 1)
            {
                nextPlayer = 0;
            }
            string name = Managers.TurnManager.playerName[nextPlayer];
            warningText.text = "Player selanjutnya (" + name + ") cekih";
            warningText.gameObject.SetActive(true);
        }
        else
        {
            warningText.gameObject.SetActive(false);
        }
    }
}
