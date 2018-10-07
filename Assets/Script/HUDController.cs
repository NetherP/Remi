using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    [SerializeField] private Image turnHUD;
    [SerializeField] private Button FormButton;
    [SerializeField] private Button buangButton;
    [SerializeField] private Button endturn;
    [SerializeField] private Text whosTurnText;

    public void TakeFromDeck()
    {
        PlayerController currentPlayer = Managers.TurnManager.CurrentPlayer;
        DealerController dealer = Managers.TurnManager.Dealer;
        currentPlayer.Push(dealer.TakeFromTop());
        currentPlayer.ReRender();
        dealer.ReRender();
        Managers.InputManager.RemoveAll();
        pullAmount = -1;                        //Dummy buat ngereset cekihan
    }

    private int[] baseLayer;
    public void Tarik()
    {
        Managers.InputManager.EnabledLayer.Remove(9);
        Managers.InputManager.EnabledLayer.Add(10);
        baseLayer = Managers.InputManager.LockSelected();
    }

    public void ConfirmPull()
    {
        Managers.InputManager.UnlockSelected(baseLayer);
        InputManager inputManager = Managers.InputManager;
        TurnManager turnManager = Managers.TurnManager;
        List<int> selected = inputManager.selected;
        int card = EarliestInstance();
        if (Managers.Referee.PullCheck(selected))
        {
            List<int> pulledCards = turnManager.Bawah.Pull(card);
            pullAmount = pulledCards.Count;
            if (pulledCards != null)
            {
                foreach (int i in pulledCards)
                {
                    Managers.TurnManager.CurrentPlayer.Push(i);
                }
                //turnManager.CurrentPlayer.PushToFormed(inputManager.selected.ToArray());                        //the cards that is used for pulling must be thrown to formedCard
                PullWindowController pullWindowController = GetComponent<PullWindowController>();
                pullWindowController.CopySelectedList(Managers.InputManager.selected);
                inputManager.RemoveAll();                                                                       //clear the selection
                turnManager.Bawah.ReRender();
                Managers.InputManager.EnabledLayer.Remove(10);
                
                pullWindowController.Init(card);
            }
            else
                Debug.LogError("Pull target is not valid");
        }
        else
        {
            inputManager.RemoveAll();
            Managers.InputManager.EnabledLayer.Remove(10);
            string promptText = "Cannot Pull";
            Messenger<string>.Broadcast(GameEvent.SHOW_PROMPT, promptText);
            turnHUD.gameObject.SetActive(true);
            FormButton.gameObject.SetActive(true);              
        }
        CancelPull();
    }

    public void CancelPull()
    {
        Managers.InputManager.UnlockSelected(baseLayer);
        Managers.InputManager.RemoveAll();
        Managers.InputManager.EnabledLayer.Add(9);
    }
    private int pullAmount;
    private int lastDiscard;

    public void Buang()
    {
        TurnManager turnManager = Managers.TurnManager;
        InputManager inputManager = Managers.InputManager;
        List<int> selected = inputManager.selected;
        bool isEnd = turnManager.CurrentPlayer.LastPlayerCard() || turnManager.Dealer.DeckIsEmpty();
        if (!Managers.Referee.IsJoker(selected[0]) || isEnd)
        {
            int card = selected[0];
            inputManager.RemoveAll();                                                    //Deselect and clear selected
            turnManager.Bawah.Push(card);
            turnManager.CurrentPlayer.RemoveCard(card);
            turnManager.CurrentPlayer.ReRender();
            turnManager.Bawah.ReRender();
            if (isEnd)
            {
                if (!turnManager.Dealer.DeckIsEmpty())
                {
                    turnManager.Bawah.CloseCard();
                    int closingScore = Managers.Referee.CardScore(card);
                    //Tutup tangan
                    if (turnManager.CurrentPlayer.NoFormed && pullAmount == -1)
                    {
                        turnManager.CloseHand(turnManager.playerTurn);
                    }
                    turnManager.IncreaseScore(turnManager.playerTurn, closingScore);
                    if (turnManager.CurrentPlayer.isCekih && pullAmount == 1)
                    //pemain sebelumnya nyemplung kalau kartunya(1) ditarik dan udah declare cekih sebelumnya
                    {
                        int sinkingScore = -Managers.Referee.CardScore(lastDiscard);
                        int prevPlayer = turnManager.playerTurn - 1;
                        if (prevPlayer < 0)
                        {
                            prevPlayer = turnManager.PlayerCount - 1;
                        }
                        turnManager.IncreaseScore(prevPlayer, sinkingScore);
                    }
                }
                Messenger.Broadcast(GameEvent.GAME_ENDED);
            }
            buangButton.gameObject.SetActive(false);
            endturn.gameObject.SetActive(true);
            FormButton.gameObject.SetActive(false);
            lastDiscard = card;                                 //taruh kartu terakhir buat cekih
        }
        else
        {
            Messenger<string>.Broadcast(GameEvent.SHOW_PROMPT, "Cannot throw joker");
            Debug.Log("Cannot throw joker");
            buangButton.gameObject.SetActive(true);
            endturn.gameObject.SetActive(false);
            inputManager.RemoveAll();
        }
    }

    private int EarliestInstance()
        //search for the index of the top pulled card
    {
        int smallest = -1;
        List<int> bawahCard = Managers.TurnManager.Bawah.cards;
        List<int> selectedCard = Managers.InputManager.selected;
        foreach(int i in selectedCard)
        {
            if (bawahCard.Contains(i))
            {
                if (smallest == -1 || bawahCard.IndexOf(i) < smallest)
                    smallest = bawahCard.IndexOf(i);
            }
        }
        if (smallest == -1)
            Debug.LogError("smallest is somehow still -1");
        return bawahCard[smallest];
    }

    public void FormCards()
    {
        List<int> cards = Managers.InputManager.selected;
        if (Managers.Referee.CheckCards(cards))
        {
            Managers.TurnManager.CurrentPlayer.PushToFormed(cards.ToArray());
            Managers.InputManager.RemoveAll();
            Managers.TurnManager.CurrentPlayer.ReRender();
        }
        else
        {
            Debug.LogError("cannot form selected cards");
            Messenger<string>.Broadcast(GameEvent.SHOW_PROMPT, "Selected card cannot be formed");
            Managers.InputManager.RemoveAll();
        }
    }

    public void NextRound()
    {
        Managers.RoundManager.LoadRound();
    }

    public void StartRound()
    {
        Managers.RoundManager.InitializeGame();
    }

    public void StartTurn()
    {
        whosTurnText.text = "Giliran: \n" + Managers.TurnManager.GetName;
        Managers.TurnManager.StartTurn();
    }

    public void EndTurn()
    {
        Managers.TurnManager.EndTurn();
        whosTurnText.text = "Giliran: \n" + Managers.TurnManager.GetName;
    }
}
