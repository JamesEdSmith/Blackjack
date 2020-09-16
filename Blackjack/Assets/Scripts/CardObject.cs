using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    private Card card;
    private SpriteRenderer spriteRenderer;
    public GameManager gameManager;

    private void Start()
    {
        spriteRenderer = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
    }

    public void setValue(Card card)
    {
        this.card = card;
        GetComponent<SpriteRenderer>().sprite = gameManager.getCardSprite(card.suit, card.value);
    }

}
