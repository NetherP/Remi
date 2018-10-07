using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PullWindowController : MonoBehaviour {

    [SerializeField] private Vector3 topPosition;
    [SerializeField] private Vector3 bottomPosition;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float offset;
    [SerializeField] private GameObject cover;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button buangButton;
    [SerializeField] private GameObject formButton;

    //this is for enabling raycast
    private const int layerValue = 12;
    private const int disabledLayer = 13;

    //index of splitting card
    private int splitIndex;

    private List<int> copyOfSelected;

    Dictionary<int, GameObject> cards;
    List<int> newPlayerCard;

    public void Init(int card)
        //using the index of the pulled card, split the player card. 
        //the pulled card is in top, the original player card is in bottom
    {
        formedCount = 0;
        PlayerController currentPlayer = Managers.TurnManager.CurrentPlayer;
        newPlayerCard = currentPlayer.playerHands;
        splitIndex = newPlayerCard.IndexOf(card);
        cover.SetActive(true);
        //set up the text and open pull window
        panel.SetActive(true);
        cards = new Dictionary<int, GameObject>();
        Managers.InputManager.EnabledLayer.Clear();
        Managers.InputManager.EnabledLayer.Add(layerValue);
        RenderCards(splitIndex);
        StartCoroutine(CopyingSelected());
        tempFormed = new List<int[]>();
    }

    private IEnumerator CopyingSelected()
    {
        yield return null;
        //moving selected from temp memory to real selected in InputManager
        foreach (int i in copyOfSelected)
        {
            Managers.InputManager.AddCard(cards[i].GetComponent<CardController>());
            //disable deselecting
            cards[i].layer = disabledLayer;
        }
    }

    private void RenderCards(int splitIndex)
    {
        int cardInRow = splitIndex;                                             //how much card in the bottom row
        Vector3 newPosBot = bottomPosition - new Vector3(cardInRow / 2f, 0f);
        for (int i = 0; i < splitIndex; ++i)                                        //this is rendering for the bottom stack
        {
            GameObject card = Instantiate(cardPrefab);
            card.transform.position = newPosBot;
            newPosBot += new Vector3(offset, 0);
            SpriteRenderer renderer = card.GetComponent<SpriteRenderer>();
            renderer.sortingOrder = 61;                                             //put it above the cover, so it can be clicked
            card.layer = layerValue;                

            CardModel cardModel = card.GetComponent<CardModel>();
            cardModel.cardIndex = newPlayerCard[i];
            cardModel.ToggleFace(true);

            //put in the list so it can be destroyed
            cards.Add(cardModel.cardIndex, card);
        }
        cardInRow = newPlayerCard.Count - splitIndex;                           //how much card in the top row
        Vector3 newPosTop = topPosition - new Vector3(cardInRow / 2, 0f);
        for (int i = splitIndex; i < newPlayerCard.Count; ++i)
        {
            GameObject card = Instantiate(cardPrefab);
            card.transform.position = newPosTop;
            newPosTop += new Vector3(offset, 0);
            SpriteRenderer renderer = card.GetComponent<SpriteRenderer>();
            renderer.sortingOrder = 61;
            card.layer = layerValue;

            CardModel cardModel = card.GetComponent<CardModel>();
            cardModel.cardIndex = newPlayerCard[i];
            cardModel.ToggleFace(true);

            cards.Add(cardModel.cardIndex, card);
        }
    }

    public void CopySelectedList(List<int> selected)
    {
        copyOfSelected = new List<int>(selected);
    }
    //use a temporary to make cancelation easier
    //allocate formed in confirmation or discard in cancelation
    private List<int[]> tempFormed;

    public void FormCard()
    {
        List<int> selected = Managers.InputManager.selected;
        if(!Managers.Referee.CheckCards(selected))
        {
            Messenger<string>.Broadcast(GameEvent.SHOW_PROMPT, "The Selected card cannot be formed");
            Managers.InputManager.RemoveAll();
            return;
        }
        foreach (int i in selected)
        {
            Destroy(cards[i]);
        }
        int[] formed = selected.ToArray();
        tempFormed.Add(formed);
        Managers.InputManager.RemoveAll();
        formedCount++;
    }

    //this is to keep track of how many card formed, min 1
    private int formedCount;
    public void ConfirmPull()
    {
        //check if the initial pulling card is formed or not
        if (formedCount < 1)
        {
            Messenger<string>.Broadcast(GameEvent.SHOW_PROMPT, "Please form at least the initial card first");
            return;
        }
        PlayerController currentPlayer = Managers.TurnManager.CurrentPlayer;
        //return if player card amount is more than 8
        int cardCounts = currentPlayer.playerHands.Count;
        // subtract the card count by formed cards
        foreach (var array in tempFormed)
        {
            cardCounts -= array.Length;
        }
        if (cardCounts > 8)
        {
            Messenger<string>.Broadcast(GameEvent.SHOW_PROMPT, "You have more than 8 cards");
            return;
        }
        Managers.InputManager.RemoveAll();
        DestroyCards();
        panel.SetActive(false);
        cover.SetActive(false);
        buangButton.gameObject.SetActive(true);
        formButton.SetActive(true);
        foreach(int[] temp in tempFormed)
        {
            Managers.TurnManager.CurrentPlayer.PushToFormed(temp);
        }
        Managers.TurnManager.CurrentPlayer.ReRender();
    }

    public void CancelPull()
    {
        for (int i = splitIndex; i < newPlayerCard.Count; ++i)
        {
            Managers.TurnManager.Bawah.Push(newPlayerCard[i]);
        }
        newPlayerCard.RemoveRange(splitIndex, newPlayerCard.Count - splitIndex);
        DestroyCards();
        Managers.TurnManager.CurrentPlayer.ReRender();
        Managers.TurnManager.Bawah.ReRender();
        tempFormed.Clear();
        Managers.InputManager.RemoveAll();
    }

    private void DestroyCards()
    {
        foreach(var pairs in cards)
        {
            Destroy(pairs.Value);
        }
        cards.Clear();
    }

    
}
