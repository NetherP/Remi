using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStack : MonoBehaviour {

    public const int cardAmount = 54;
    List<int> cards;

    public bool isGameDeck;

    public bool HasCard
    {
        get { return cards != null && cards.Count > 0; }
    }

    public event CardRemoveEventHandler CardRemoved;

    public int CardCount
    {
        get
        {
            if (cards == null)
            {
                return 0;
            }
            else
                return cards.Count;
        }
    }

    public IEnumerable<int> GetCards()
    {
        foreach (int i in cards)
        {
            yield return i;
        }
    }

    public int Pop()
    {
        int temp = cards[0];
        cards.RemoveAt(0);

        if (CardRemoved != null)
        {
            CardRemoved(this, new CardRemovedEventArgs(temp));
        }

        return temp;
    }

    public void Push(int card)
    {
        cards.Add(card);
    }

    public void CreateDeck()
    {
        cards.Clear();

        for (int i = 0; i < cardAmount; ++i)
            cards.Add(i);

        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
    }

    private void Start()
    {
        cards = new List<int>();
        if (isGameDeck)
        {
            CreateDeck();
        }
    }
}
