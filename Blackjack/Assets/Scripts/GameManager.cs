using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int BLACK_JOKER = 52;
    public const int RED_JOKER = 53;
    public const int CARD_BACK = 54;

    public int cardSpace = 20;
    public int dealerhandY = 100;
    public int playerHandY = -100;

    public GameObject cardObjectPrefab;
    public Canvas canvas;

    Sprite[] cardSprites;
    List<Card> deck;
    List<Card> cards;
    List<CardObject> dealerHand;
    List<CardObject> playerHand;
    List<GameObject> cardGameObjects;

    public enum Suit
    {
        clubs, diamonds, hearts, spades
    }

    int gameState = 0;

    // Start is called before the first frame update
    void Start()
    {
        cardSprites = Resources.LoadAll<Sprite>("cards");
        //8 decks of cards
        cards = new List<Card>(52);
        deck = new List<Card>(416);
        dealerHand = new List<CardObject>(10);
        playerHand = new List<CardObject>(10);
        cardGameObjects = new List<GameObject>(42);

        for (int suit = 0; suit < 4; suit++)
        {
            for (int value = 1; value < 13; value++)
            {
                cards.Add(new Card((Suit)suit, value));
            }
        }

        fillDeck();

        //create cardobject pool
        for (int i = 0; i < 42; i++)
        {
            GameObject gameObject = Instantiate(cardObjectPrefab, canvas.transform);
            gameObject.SetActive(false);
            gameObject.GetComponent<CardObject>().gameManager = this;
            cardGameObjects.Add(gameObject);
        }

    }

    private void fillDeck()
    {
        deck.Clear();
        for (int i = 0; i < 8; i++)
        {
            foreach (Card card in cards)
            {
                deck.Add(card);
            }
        }
    }

    public void dealCards()
    {
        while (dealerHand.Count > 0)
        {
            dealerHand[0].gameObject.SetActive(false);
            dealerHand.RemoveAt(0);
        }

        while (playerHand.Count > 0)
        {
            playerHand[0].gameObject.SetActive(false);
            playerHand.RemoveAt(0);
        }

        dealerHand.Add(dealCard());
        dealerHand.Add(dealCard());

        playerHand.Add(dealCard());
        playerHand.Add(dealCard());

        float cardStartX = dealerHand.Count * cardSpace * -0.5f;

        foreach (CardObject cardObject in dealerHand)
        {
            Vector3 pos = cardObject.gameObject.GetComponent<RectTransform>().anchoredPosition;
            pos.x = cardStartX + cardSpace * dealerHand.IndexOf(cardObject);
            pos.y = dealerhandY;
            cardObject.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
        }

        foreach (CardObject cardObject in playerHand)
        {
            Vector3 pos = cardObject.gameObject.GetComponent<RectTransform>().anchoredPosition;
            pos.x = cardStartX + cardSpace * playerHand.IndexOf(cardObject);
            pos.y = playerHandY;
            cardObject.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    private CardObject dealCard()
    {
        int cardIndex = UnityEngine.Random.Range(0, deck.Count - 1);
        GameObject cardGameObject = null;
        foreach (GameObject obj in cardGameObjects)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                cardGameObject = obj;
                CardObject cardObject = cardGameObject.GetComponent<CardObject>();
                cardObject.setValue(deck[cardIndex]);
                deck.RemoveAt(cardIndex);
                return cardObject;
            }
        }
        return null;
    }

    public Sprite getCardSprite(Suit suit, int value)
    {
        return cardSprites[13 * (int)suit + value - 1];
    }

    public Sprite getCardBackSprite()
    {
        return cardSprites[CARD_BACK];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
