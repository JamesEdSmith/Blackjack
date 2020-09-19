using UnityEngine;
using System;
using System.Collections;

public class Card
{
    public GameManager.Suit suit;
    public int value;

    public Card(GameManager.Suit suit, int value)
    {
        this.suit = suit;
        this.value = value;
    }

}
