using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour {

    public bool isDealer;

    public List<int> cards = new List<int>();


    private void Start()
    {
        if (isDealer)
        {
            //initialize
            for (int i = 0; i < 54; ++i)
            {
                cards.Add(i);
            }

            // shuffling
            for (int i = 0; i < cards.Count; i++)
            {
                int temp = cards[i];
                int swap = Random.Range(i, cards.Count);
                cards[i] = cards[swap];
                cards[swap] = temp;
            }
        }
    }
    public int Pop()
    {
        int temp = cards[cards.Count - 1];
        cards.RemoveAt(cards.Count - 1);

        return temp;
    }

    public void Push(int card)
    {
        cards.Add(card);
    }

}
