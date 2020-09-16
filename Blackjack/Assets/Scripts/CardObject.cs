using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    private Card card;
    private SpriteRenderer spriteRenderer;
    public GameManager gameManager;
    private bool flipped = false;

    private void Start()
    {
        spriteRenderer = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
    }

    public void setValue(Card card)
    {
        this.card = card;
        if (spriteRenderer == null)
        {
            spriteRenderer = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
        }
        spriteRenderer.sprite = gameManager.getCardSprite(card.suit, card.value);
    }

    public void setFlipped(bool flipped)
    {
        this.flipped = flipped;

        if (spriteRenderer == null)
        {
            spriteRenderer = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
        }
        spriteRenderer.sprite = gameManager.getCardSprite(card.suit, card.value);

        if (flipped)
        {
            spriteRenderer.sprite = gameManager.getCardBackSprite();
        }
        else
        {
            spriteRenderer.sprite = gameManager.getCardSprite(card.suit, card.value);
        }
    }

}
