using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChangeCard : MonoBehaviour {

    CardFlipper flipper;
    CardModel cardModel;
    int cardIndex = 0;

    public GameObject card;

    private void Awake()
    {
        cardModel = card.GetComponent<CardModel>();
        flipper = card.GetComponent<CardFlipper>();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 20), "hit me!"))
        {
            if (cardIndex >= cardModel.faces.Length)
            {
                cardIndex = 0;
                flipper.FlipCard(cardModel.faces[cardModel.faces.Length - 1], cardModel.cardBack, -1);
                
            }
            else
            {
                if(cardIndex > 0)
                {
                    flipper.FlipCard(cardModel.faces[cardIndex - 1], cardModel.faces[cardIndex], cardIndex);
                }
                else
                    flipper.FlipCard(cardModel.cardBack, cardModel.faces[cardIndex], cardIndex);
            }
            cardIndex++;
            
        }
    }
}
