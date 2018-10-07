using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerController : MonoBehaviour {

    private Hands hand;
    private HandsRender deckRenderer;

    public void Init()
    {
        hand = GetComponent<Hands>();
        deckRenderer = GetComponent<HandsRender>();
        deckRenderer.startPos = new Vector3(-1, 0, 0);
        deckRenderer.offset = 0.02f;
        deckRenderer.sideways = true;
        deckRenderer.RenderCards();
        deckRenderer.layerValue = 11;
    }

    public void ReRender()
    {
        deckRenderer.UnrenderCards();
        deckRenderer.RenderCards();
    }

    public int TakeFromTop()
    {
        if (hand == null)
        {
            Debug.Log("dealer hand is null");
            hand = GetComponent<Hands>();
            Debug.Log("dealer hand initialized");
        }
        return hand.Pop();
    }

    public bool DeckIsEmpty()
    {
        return hand.cards.Count == 0;
    }
}
