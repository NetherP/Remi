﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour {

    public Color highlightColor = Color.cyan;
    public SpriteRenderer sprite;
    public Color baseColor;
    private CardModel card;
    public int cardIndex;

    public void Select()
    {
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        sprite.color = highlightColor;
    }

    public void Deselect()
    {
        sprite.color = baseColor;
    }

    // Use this for initialization
    void Start () {
        sprite = GetComponent<SpriteRenderer>();
        baseColor = sprite.color;
        card = GetComponent<CardModel>();
        cardIndex = card.cardIndex;
    }
}
