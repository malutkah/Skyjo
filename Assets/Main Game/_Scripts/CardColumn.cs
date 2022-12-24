using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardColumn : MonoBehaviour
{
    public int columnNumber;
    public Card[] cards;
    public bool deleted;

    public void Awake()
    {
        cards = new Card[3];
        deleted = false;
    }

    public void SetColumn(Card c, int row)
    {
        cards[row] = c;
    }
}
