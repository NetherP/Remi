using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartuBawah : MonoBehaviour {

    public List<int> cards;
    private Hands hands;
    private HandsRender render;
    [SerializeField] private float offsetVal;

    private void Start()
    {
        hands = GetComponent<Hands>();
        cards = hands.cards;
        render = GetComponent<HandsRender>();
        render.bawah = true;
        render.offset = offsetVal;                      //magic constant for rendering the disCARD lol
        render.layerValue = 10;
    }

    public List<int> Pull(int value)                //pull from value?
    {
        if (cards.Contains(value))
        {
            int index = cards.IndexOf(value);
            List<int> pulled = new List<int>();
            for (int i = index; i < cards.Count; ++i) // ambil element list yang mau dibuang
            {
                pulled.Add(cards[i]);
            }
            cards.RemoveRange(index, cards.Count - index); // remove from index to end
            return pulled;
        }
        else
        {
            Debug.LogError("Card does not exist in the disCARD");
            return null;

        }
    }

    public void CloseCard()
    {
        render.CloseCard();
    }

    public void Push(int value)
    {
        cards.Add(value);
    }

    public void ReRender()
    {
        render.UnrenderCards();
        render.RenderCards();
    }
}
