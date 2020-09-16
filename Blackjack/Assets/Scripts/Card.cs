﻿using UnityEngine;
using System;
using System.Collections;

public struct Card
{
    public GameManager.Suit suit;
    public int value;

    public Card(GameManager.Suit suit, int value)
    {
        this.suit = suit;
        this.value = value;
    }

}
