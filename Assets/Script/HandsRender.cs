using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsRender : MonoBehaviour {

    public Vector3 startPos;
    public bool sideways;
    public float offset;
    public GameObject cardPrefab;
    public bool showFace;
    public bool bawah;
    public float depthOffset = 0.01f;
    public int layerValue;
    public bool isPlayer;

    private List<int> cards;
    private List<GameObject> theCard = new List<GameObject>();


    private void Awake()
    {
        isPlayer = GetComponent<PlayerController>() != null;
        cards = GetComponent<Hands>().cards;
    }

    public void RenderCards()
    {
        Vector3 newPos = startPos;
        if (isPlayer)
            newPos = startPos - (PositionOffset(true, true) * cards.Count/(float)2.0) + new Vector3(0.5f, 0);               //keep the card rendered from the center + adjusting for center point
        Vector3 rotation = Vector3.zero;
        for (int i = 0; i < cards.Count; ++i)
        {
            if (bawah)
            {
                int kiri = 7;
                int bawah = 20;
                int kanan = 27;
                int atas = 39;
                int j = i;
                if (i >= atas)
                    j -= atas - 2;
                if (j < kiri )
                {
                    newPos += PositionOffset(false, false);              //Kartu spawn ke bawah
                    rotation = Vector3.zero;
                }
                else if (j < bawah)
                {
                    newPos += PositionOffset(true, true);                //kartu spawn ke kanan
                    rotation = new Vector3(0, 0, 270);
                }
                else if (j < kanan)
                {
                    newPos += PositionOffset(false, true);               //kartu spawn ke atas
                    rotation = Vector3.zero;
                }
                else if (j < atas)
                {
                    newPos += PositionOffset(true, false);               //kartu spawn ke kiri
                    rotation = new Vector3(0, 0, 90);
                }
            }
            else
            {
                if (i != 0)                                             //if it is the first card, newpos = startpos
                    newPos += PositionOffset(true, true);
            }
            theCard.Add(MakeCard(newPos, cards[i], rotation));
            SpriteRenderer renderer = theCard[i].GetComponent<SpriteRenderer>();
            renderer.sortingOrder = i;
            renderer.transform.position += new Vector3(0, 0, -depthOffset * i);
        }
    }

    public void UnrenderCards()
    {
        foreach(GameObject kartu in theCard)
        {
            Destroy(kartu);
        }
        theCard.Clear();
    }

    public void CloseCard()
    {
        CardModel lastCard = theCard[theCard.Count - 1].GetComponent<CardModel>();
        lastCard.ToggleFace(false);
    }

    private GameObject MakeCard(Vector3 position, int cardValue, Vector3 rotation)
    {
        GameObject card = Instantiate(cardPrefab);
        card.transform.position = position;
        card.transform.eulerAngles = rotation;
        card.layer = layerValue;

        CardModel cardModel = card.GetComponent<CardModel>();
        cardModel.cardIndex = cardValue;
        cardModel.ToggleFace(showFace);
        return card;
    }

    private Vector3 PositionOffset(bool sideways, bool positive)
    {
        if(sideways)
        {
            if (positive)
                return new Vector3(offset, 0);
            else
                return new Vector3(-offset, 0);
        }
        else
        {
            if (positive)
                return new Vector3(0, offset);
            else
                return new Vector3(0, -offset);
        }
    }
}
