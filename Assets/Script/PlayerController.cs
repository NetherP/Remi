using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hands))]
[RequireComponent(typeof(HandsRender))]
public class PlayerController : MonoBehaviour {

    private Hands hand;
    private HandsRender handsRenderer;
    public int index;
    private List<int[]> formedCards;
    public Vector3 startPos;
    public List<int> playerHands { get; private set; }
    public bool isCekih { get; private set; }
    public void ToggleCekih()
    {
        isCekih = !isCekih;
    }

    public bool canSink { get; private set; }

    private int formedCount;
    public bool NoFormed { get { return formedCount == 0; } }

    private void OnEnable()
    {
        Messenger<int>.AddListener(GameEvent.CEKIH, Sinking);
        Messenger<int>.AddListener(GameEvent.BATAL_CEKIH, Floating);
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.CEKIH, Sinking);
        Messenger<int>.RemoveListener(GameEvent.BATAL_CEKIH, Floating);
    }

    private void Sinking(int playerIndex)
    {
        if (playerIndex == index)
        {
            canSink = true;
        }
    }

    private void Floating(int playerIndex)
    {
        if (playerIndex == index)
        {
            canSink = false;
        }
    }

    // Use this for initialization
    void Awake () {
        hand = GetComponent<Hands>();
        handsRenderer = GetComponent<HandsRender>();
        handsRenderer.offset = 1;
        handsRenderer.sideways = true;
        handsRenderer.bawah = false;
        handsRenderer.layerValue = 9;
        formedCards = new List<int[]>();
        playerHands = hand.cards;
        isCekih = false;
	}

    public void SetUp()
    {
        handsRenderer.startPos = startPos;
        handsRenderer.RenderCards();
        formedCount = formedCards.Count;
    }

    public void WrapUp()
    {
        handsRenderer.UnrenderCards();
    }

    public void ReRender()
    {
        handsRenderer.UnrenderCards();
        handsRenderer.RenderCards();
    }

    public void Push(int card)
    {
        //animate pushing card
        hand.Push(card);
    }

    public void RemoveCard(int card)
    {
        hand.cards.Remove(card);
    }

    public void PushToFormed(int[] cards)
    {
        foreach (int card in cards)
        {
            RemoveCard(card);
        }
        formedCards.Add(cards);
        Debug.Log("The content of formed is: ");
        foreach (int i in cards)
            Debug.Log(i + " ");
    }

    public int Scorer()
        // to be called by score tracker after the round finished
    {
        int score = 0;
        foreach (int i in playerHands)
        {
            if (i == 52 || i == 53)
            {
                score -= 50;
            }
            else
            {
                score -= Managers.Referee.CardScore(i) / 10;
            }
        }
        if (formedCards.Count == 0)
            return score;
        foreach(var i in formedCards)
        {
           score +=  i.Length * WhatisTheScore(i);
        }
        return score;
    }

    private int WhatisTheScore(int[] cards)
    {
        int cardType;
        if (cards[0] == 52 || cards[0] == 53)                           //if the first card is joker, use the second card (52 and 53 is joker)
            cardType = cards[1];
        else
            cardType = cards[0];
        cardType = cardType % 13;
        switch(cardType)
        {
            case 0:             //AS
                return 15;
            case 1:             // two
            case 2:             // three
            case 3:             // four
            case 4:             // five
            case 5:             // six
            case 6:             // seven
            case 7:             // eight
            case 8:             // nine
            case 9:             // ten
                return 5;
            case 10:            // jack
            case 11:            // queen
            case 12:            // king
                return 10;
            default:
                Debug.LogError("card scorer goes through default case: " + cardType);
                return 0;
        }
    }

    public bool LastPlayerCard()
    {
        return playerHands.Count < 2;
    }

    public bool HaveCapital()
    {
        return formedCards.Count != 0;
    }
}
